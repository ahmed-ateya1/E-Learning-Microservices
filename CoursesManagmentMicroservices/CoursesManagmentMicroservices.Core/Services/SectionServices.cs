using AutoMapper;
using CoursesManagmentMicroservices.Core.Domain.Entities;
using CoursesManagmentMicroservices.Core.Domain.RepositoryContract;
using CoursesManagmentMicroservices.Core.Dtos.SectionDto;
using CoursesManagmentMicroservices.Core.ServiceContract;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace CoursesManagmentMicroservices.Core.Services
{
    public class SectionServices : ISectionServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SectionServices> _logger;

        public SectionServices(IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<SectionServices> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _logger.LogInformation("SectionServices initialized.");
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

        public async Task<SectionResponse> CreateAsync(SectionAddRequest? request)
        {
            _logger.LogInformation("Attempting to create a new section with request: {@Request}", request);
            if (request == null)
            {
                _logger.LogWarning("SectionAddRequest is null.");
                throw new ArgumentNullException(nameof(request));
            }

            var course = await _unitOfWork.Repository<Course>()
                .GetByAsync(x => x.CourseID == request.CourseID);
            if (course == null)
            {
                _logger.LogWarning("Course not found for CourseID: {CourseID}", request.CourseID);
                throw new KeyNotFoundException("Course not found");
            }

            var section = _mapper.Map<Section>(request);
            section.Course = course;
            section.CourseID = course.CourseID;

            await ExecuteWithTransactionAsync(async () =>
            {
                await _unitOfWork.Repository<Section>().CreateAsync(section);
                await _unitOfWork.CompleteAsync();
            });

            var response = _mapper.Map<SectionResponse>(section);
            _logger.LogInformation("Section created successfully with SectionID: {SectionID}", section.SectionID);
            return response;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            _logger.LogInformation("Attempting to delete section with SectionID: {SectionID}", id);
            var section = await _unitOfWork.Repository<Section>()
                .GetByAsync(x => x.SectionID == id, includeProperties: "Lectures,Course");

            if (section == null)
            {
                _logger.LogWarning("Section not found for SectionID: {SectionID}", id);
                throw new KeyNotFoundException("Section not found");
            }

            await ExecuteWithTransactionAsync(async () =>
            {
                if (section.Lectures.Any())
                {
                    _logger.LogInformation("Deleting {Count} lectures for SectionID: {SectionID}", section.Lectures.Count(), id);
                    foreach (var lecture in section.Lectures)
                    {
                        await _unitOfWork.Repository<Lecture>().DeleteAsync(lecture);
                    }
                }
                await _unitOfWork.Repository<Section>().DeleteAsync(section);
                await _unitOfWork.CompleteAsync();
            });

            _logger.LogInformation("Section deleted successfully with SectionID: {SectionID}", id);
            return true;
        }

        public async Task<IEnumerable<SectionResponse>> GetAllAsync(Expression<Func<Section, bool>>? predicate = null)
        {
            _logger.LogInformation("Retrieving sections with predicate: {Predicate}", predicate?.ToString() ?? "None");
            var sections = await _unitOfWork.Repository<Section>()
                .GetAllAsync(predicate, includeProperties: "Lectures,Course");

            if (!sections.Any())
            {
                _logger.LogInformation("No sections found for the given predicate.");
                return Enumerable.Empty<SectionResponse>();
            }

            var response = _mapper.Map<IEnumerable<SectionResponse>>(sections);
            _logger.LogInformation("Retrieved {Count} sections successfully.", sections.Count());
            return response;
        }

        public async Task<SectionResponse?> GetByAsync(Expression<Func<Section, bool>> predicate, bool isTracked = false)
        {
            _logger.LogInformation("Retrieving section with predicate: {Predicate}, IsTracked: {IsTracked}", predicate.ToString(), isTracked);
            var section = await _unitOfWork.Repository<Section>()
                .GetByAsync(predicate, isTracked, includeProperties: "Lectures,Course");

            if (section == null)
            {
                _logger.LogWarning("No section found matching the predicate: {Predicate}", predicate.ToString());
                return null;
            }

            var response = _mapper.Map<SectionResponse>(section);
            _logger.LogInformation("Section retrieved successfully with SectionID: {SectionID}", section.SectionID);
            return response;
        }

        public async Task<SectionResponse> UpdateAsync(SectionUpdateRequest? request)
        {
            _logger.LogInformation("Attempting to update section with request: {@Request}", request);
            if (request == null)
            {
                _logger.LogWarning("SectionUpdateRequest is null.");
                throw new ArgumentNullException(nameof(request));
            }

            var course = await _unitOfWork.Repository<Course>()
                .GetByAsync(x => x.CourseID == request.CourseID);
            if (course == null)
            {
                _logger.LogWarning("Course not found for CourseID: {CourseID}", request.CourseID);
                throw new KeyNotFoundException("Course not found");
            }

            var section = await _unitOfWork.Repository<Section>()
                .GetByAsync(x => x.SectionID == request.SectionID, includeProperties: "Lectures,Course");
            if (section == null)
            {
                _logger.LogWarning("Section not found for SectionID: {SectionID}", request.SectionID);
                throw new KeyNotFoundException("Section not found");
            }

            _mapper.Map(request, section);
            await ExecuteWithTransactionAsync(async () =>
            {
                await _unitOfWork.Repository<Section>().UpdateAsync(section);
                await _unitOfWork.CompleteAsync();
            });

            var response = _mapper.Map<SectionResponse>(section);
            _logger.LogInformation("Section updated successfully with SectionID: {SectionID}", section.SectionID);
            return response;
        }
    }
}