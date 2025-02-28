using AutoMapper;
using CoursesManagmentMicroservices.Core.Domain.Entities;
using CoursesManagmentMicroservices.Core.Domain.RepositoryContract;
using CoursesManagmentMicroservices.Core.Dtos.LectureDto;
using CoursesManagmentMicroservices.Core.ServiceContract;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace CoursesManagmentMicroservices.Core.Services
{
    public class LectureService : ILectureService
    {
        private readonly ILogger<LectureService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileServices _fileServices;

        public LectureService(ILogger<LectureService> logger, IUnitOfWork unitOfWork, IMapper mapper, IFileServices fileServices)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileServices = fileServices;
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
        public async Task<LectureResponse> CreateAsync(LectureAddRequest? request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var section = await _unitOfWork.Repository<Section>()
                .GetByAsync(x => x.SectionID == request.SectionID)
                ?? throw new ArgumentNullException(nameof(request.SectionID));

            var lecture = _mapper.Map<Lecture>(request);
            if(request.Video != null)
            {
                lecture.VideoUrl = await _fileServices.CreateFileAsync(request.Video);
            }
            if (request.File != null)
            {
                lecture.FileURL = await _fileServices.CreateFileAsync(request.File);
            }
            lecture.Section = section;
            await ExecuteWithTransactionAsync(async () =>
            {
                await _unitOfWork.Repository<Lecture>().CreateAsync(lecture);
                section.DurationInHours += lecture.DurationInMinutes;
                await _unitOfWork.CompleteAsync();
            });
            return _mapper.Map<LectureResponse>(lecture);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var lecture = await _unitOfWork.Repository<Lecture>()
                .GetByAsync(x => x.LectureID == id);

            if (lecture == null)
            {
                throw new ArgumentNullException(nameof(lecture));
            }
            await ExecuteWithTransactionAsync(async () =>
            {
                await _unitOfWork.Repository<Lecture>().DeleteAsync(lecture);
                await _unitOfWork.CompleteAsync();
            });
            return true;
        }

        public async Task<IEnumerable<LectureResponse>> GetAllAsync(Expression<Func<Lecture, bool>>? predicate = null)
        {
            var lectures = await _unitOfWork.Repository<Lecture>()
                .GetAllAsync(predicate,includeProperties: "Section,Section.Course");
            if(!lectures.Any())
                return Enumerable.Empty<LectureResponse>();

            return _mapper.Map<IEnumerable<LectureResponse>>(lectures);
        }

        public async Task<LectureResponse?> GetByAsync(Expression<Func<Lecture, bool>> predicate, bool isTracked = false)
        {
            var lecture = await _unitOfWork.Repository<Lecture>()
                .GetByAsync(predicate, isTracked,includeProperties: "Section,Section.Course");
            if (lecture == null)
                return null;
            return _mapper.Map<LectureResponse>(lecture);
        }

        public async Task<LectureResponse> UpdateAsync(LectureUpdateRequest? request)
        {
           if (request == null)
                throw new ArgumentNullException(nameof(request));

           var lecture = await _unitOfWork.Repository<Lecture>()
                .GetByAsync(x => x.LectureID == request.LectureID,includeProperties: "Section,Section.Course")
                ?? throw new ArgumentNullException(nameof(request.LectureID));

            var section = await _unitOfWork.Repository<Section>()
                .GetByAsync(x => x.SectionID == request.SectionID)
                ?? throw new ArgumentNullException(nameof(request.SectionID));

            _mapper.Map(request, lecture);

            if (request.Video != null)
            {
                lecture.VideoUrl = await _fileServices.CreateFileAsync(request.Video);
            }
            if (request.File != null)
            {
                lecture.FileURL = await _fileServices.CreateFileAsync(request.File);
            }
            lecture.Section = section;

            await ExecuteWithTransactionAsync(async () =>
            {
                section.DurationInHours -= lecture.DurationInMinutes;
                await _unitOfWork.Repository<Lecture>().UpdateAsync(lecture);
                section.DurationInHours += lecture.DurationInMinutes;
                await _unitOfWork.CompleteAsync();
            });
            return _mapper.Map<LectureResponse>(lecture);
        }
    }
}
