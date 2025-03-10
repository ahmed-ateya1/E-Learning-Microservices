using AutoMapper;
using Microsoft.Extensions.Logging;
using QuizManagementMicroservice.Core.Domain.Entities;
using QuizManagementMicroservice.Core.Domain.RepositoryContracts;
using QuizManagementMicroservice.Core.Dtos;
using QuizManagementMicroservice.Core.Dtos.QuestionDto;
using QuizManagementMicroservice.Core.Dtos.QuizzDto;
using QuizManagementMicroservice.Core.HttpClients;
using QuizManagementMicroservice.Core.ServiceContracts;
using System.Linq.Expressions;

namespace QuizManagementMicroservice.Core.Services
{
    public class QuizzService : IQuizzService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserMicroserviceClient _userMicroserviceClient;
        private readonly ILectureMicroserviceClient _lectureMicroserviceClient;
        private readonly ILogger<QuizzService> _logger;

        public QuizzService(IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<QuizzService> logger,
            IUserMicroserviceClient userMicroserviceClient,
            ILectureMicroserviceClient lectureMicroserviceClient)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _userMicroserviceClient = userMicroserviceClient;
            _lectureMicroserviceClient = lectureMicroserviceClient;
            _logger.LogInformation("QuizzService initialized");
        }

        private async Task ExecuteWithTransactionAsync(Func<Task> action)
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    _logger.LogInformation("Starting transaction for operation: {Operation}", action.Method.Name);
                    await action();
                    await _unitOfWork.CommitTransactionAsync();
                    _logger.LogInformation("Transaction committed successfully for operation: {Operation}", action.Method.Name);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Transaction failed for operation: {Operation}", action.Method.Name);
                    await _unitOfWork.RollbackTransactionAsync();
                    throw;
                }
            }
        }

        private async Task HandleQuestionsResponse(IEnumerable<QuizzResponse> quizzes)
        {
            _logger.LogInformation("Handling questions response for {Count} quizzes", quizzes.Count());
            foreach (var quizz in quizzes)
            {
                var questions = await _unitOfWork.Repository<Question>()
                    .GetAllAsync(x => x.QuizzID == quizz.QuizzID);
                _logger.LogDebug("Retrieved {QuestionCount} questions for quiz {QuizzID}",
                    questions.Count(), quizz.QuizzID);
                var questionResponses = _mapper.Map<IEnumerable<QuestionResponse>>(questions);
                quizz.Questions = questionResponses.ToList();
            }
        }

        public async Task<QuizzResponse> CreateAsync(QuizzAddRequest? request)
        {
            _logger.LogInformation("Attempting to create new quiz");
            if (request == null)
            {
                _logger.LogWarning("CreateAsync called with null request");
                throw new ArgumentNullException(nameof(request));
            }

            _logger.LogDebug("Validating user {InstructorID} and lecture {LectureID}",
                request.InstructorID, request.LectureID);
            var user = await _userMicroserviceClient.GetUserAsync(request.InstructorID);
            var lecture = await _lectureMicroserviceClient.GetLectureAsync(request.LectureID);

            if (user == null || lecture == null)
            {
                _logger.LogError("User or Lecture not found for InstructorID: {InstructorID}, LectureID: {LectureID}",
                    request.InstructorID, request.LectureID);
                throw new InvalidOperationException("User or Lecture not found");
            }

            var quizz = _mapper.Map<Quizz>(request);

            await ExecuteWithTransactionAsync(async () =>
            {
                await _unitOfWork.Repository<Quizz>().CreateAsync(quizz);
                await _unitOfWork.CompleteAsync();
            });

            _logger.LogInformation("Quiz created successfully with ID: {QuizzID}", quizz.QuizzID);
            return _mapper.Map<QuizzResponse>(quizz);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            _logger.LogInformation("Attempting to delete quiz with ID: {QuizzID}", id);
            var quizz = await _unitOfWork.Repository<Quizz>()
                .GetByAsync(x => x.QuizzID == id , includeProperties: "Questions");

            if (quizz == null)
            {
                _logger.LogWarning("Quiz not found for deletion with ID: {QuizzID}", id);
                return false;
            }

            await ExecuteWithTransactionAsync(async () =>
            {
                if (quizz.Questions.Any())
                {
                    _logger.LogInformation("Deleting {Count} questions for quiz {QuizzID}", quizz.Questions.Count(), id);
                    await _unitOfWork.Repository<Question>().RemoveRangeAsync(quizz.Questions);
                }
                await _unitOfWork.Repository<Quizz>().DeleteAsync(quizz);
            });

            _logger.LogInformation("Quiz deleted successfully with ID: {QuizzID}", id);
            return true;
        }

        public async Task<PaginatedResponse<QuizzResponse>> GetAllAsync
            (Expression<Func<Quizz, bool>>? predicate = null, PaginationDto? pagination = null)
        {
            _logger.LogInformation("Retrieving all quizzes with pagination");
            pagination ??= new PaginationDto();

            var quizzes = await _unitOfWork.Repository<Quizz>()
                .GetAllAsync(predicate, includeProperties: "Questions");

            if (quizzes == null || !quizzes.Any())
            {
                _logger.LogInformation("No quizzes found matching the criteria");
                return new PaginatedResponse<QuizzResponse>
                {
                    Items = new List<QuizzResponse>(),
                    PageIndex = pagination.PageIndex,
                    PageSize = pagination.PageSize,
                    TotalCount = 0
                };
            }

            var response = _mapper.Map<IEnumerable<QuizzResponse>>(quizzes);
            //await HandleQuestionsResponse(response);

            _logger.LogInformation("Retrieved {Count} quizzes successfully", response.Count());
            return new PaginatedResponse<QuizzResponse>
            {
                Items = response,
                PageIndex = pagination.PageIndex,
                PageSize = pagination.PageSize,
                TotalCount = response.Count()
            };
        }

        public async Task<QuizzResponse> GetByAsync(Expression<Func<Quizz, bool>> predicate)
        {
            _logger.LogInformation("Retrieving quiz by predicate");
            var quizz = await _unitOfWork.Repository<Quizz>().GetByAsync(predicate);

            if (quizz == null)
            {
                _logger.LogWarning("No quiz found matching the predicate");
                return null;
            }

            var response = _mapper.Map<QuizzResponse>(quizz);
            //await HandleQuestionsResponse(new List<QuizzResponse> { response });

            _logger.LogInformation("Quiz retrieved successfully with ID: {QuizzID}", quizz.QuizzID);
            return response;
        }

        public async Task<QuizzResponse> UpdateAsync(QuizzUpdateRequest? request)
        {
            _logger.LogInformation("Attempting to update quiz");
            if (request == null)
            {
                _logger.LogWarning("UpdateAsync called with null request");
                throw new ArgumentNullException(nameof(request));
            }

            _logger.LogDebug("Validating user {InstructorID} and lecture {LectureID} for update",
                request.InstructorID, request.LectureID);
            var user = await _userMicroserviceClient.GetUserAsync(request.InstructorID);
            var lecture = await _lectureMicroserviceClient.GetLectureAsync(request.LectureID);

            if (user == null || lecture == null)
            {
                _logger.LogError("User or Lecture not found for update - InstructorID: {InstructorID}, LectureID: {LectureID}",
                    request.InstructorID, request.LectureID);
                throw new InvalidOperationException("User or Lecture not found");
            }

            var quizz = await _unitOfWork.Repository<Quizz>()
                .GetByAsync(x => x.QuizzID == request.QuizzID);

            if (quizz == null)
            {
                _logger.LogError("Quiz not found for update with ID: {QuizzID}", request.QuizzID);
                throw new InvalidOperationException("Quizz not found");
            }

            quizz = _mapper.Map(request, quizz);

            await ExecuteWithTransactionAsync(async () =>
            {
                await _unitOfWork.Repository<Quizz>().UpdateAsync(quizz);
            });

            var response = _mapper.Map<QuizzResponse>(quizz);
            //await HandleQuestionsResponse(new List<QuizzResponse> { response });

            _logger.LogInformation("Quiz updated successfully with ID: {QuizzID}", quizz.QuizzID);
            return response;
        }
    }
}