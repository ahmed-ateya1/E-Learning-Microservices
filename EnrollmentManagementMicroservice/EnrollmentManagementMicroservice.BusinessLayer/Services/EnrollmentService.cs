using AutoMapper;
using EnrollmentManagementMicroservice.BusinessLayer.Dtos;
using EnrollmentManagementMicroservice.BusinessLayer.Dtos.ExternalDto;
using EnrollmentManagementMicroservice.BusinessLayer.HttpClients;
using EnrollmentManagementMicroservice.DataAccessLayer.Entities;
using EnrollmentManagementMicroservice.DataAccessLayer.Repositories;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EnrollmentManagementMicroservice.BusinessLayer.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly ILogger<EnrollmentService> _logger;
        private readonly IUserMicroserviceClient _userClient;
        private readonly ICourseMicroserviceClient _courseClient;
        private readonly IMapper _mapper;
        private readonly IEnrollmentRepository _enrollmentRepository;

        public EnrollmentService(
            ILogger<EnrollmentService> logger,
            IUserMicroserviceClient userClient,
            ICourseMicroserviceClient courseClient,
            IMapper mapper,
            IEnrollmentRepository enrollmentRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userClient = userClient ?? throw new ArgumentNullException(nameof(userClient));
            _courseClient = courseClient ?? throw new ArgumentNullException(nameof(courseClient));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _enrollmentRepository = enrollmentRepository ?? throw new ArgumentNullException(nameof(enrollmentRepository));
        }

        public async Task<EnrollmentResponse> AddEnrollmentAsync(EnrollmentAddRequest request)
        {
            if (request == null)
            {
                _logger.LogWarning("AddEnrollmentAsync received null request");
                throw new ArgumentNullException(nameof(request));
            }

            _logger.LogInformation("Starting enrollment creation for UserID: {UserId}, CourseID: {CourseId}",
                request.UserID, request.CourseID);

            var (user, course) = await GetUserAndCourseAsync(request.UserID, request.CourseID);

            var enrollment = _mapper.Map<Enrollment>(request);
            enrollment.EnrollmentDate = DateTime.UtcNow;
            enrollment.EnrollmentID = Guid.NewGuid();

            _logger.LogDebug("Creating enrollment with ID: {EnrollmentId}", enrollment.EnrollmentID);
            await _enrollmentRepository.CreateAsync(enrollment);

            var response = MapToResponse(enrollment, user, course);
            _logger.LogInformation("Enrollment created successfully with ID: {EnrollmentId}", enrollment.EnrollmentID);
            return response;
        }

        public async Task<bool> DeleteEnrollmentAsync(Guid enrollmentId)
        {
            _logger.LogInformation("Starting enrollment deletion for ID: {EnrollmentId}", enrollmentId);

            var enrollment = await _enrollmentRepository.GetByAsync(x => x.EnrollmentID == enrollmentId);
            if (enrollment == null)
            {
                _logger.LogWarning("Enrollment not found for deletion with ID: {EnrollmentId}", enrollmentId);
                throw new KeyNotFoundException("Enrollment not found");
            }

            _logger.LogDebug("Deleting enrollment with ID: {EnrollmentId}", enrollmentId);
            await _enrollmentRepository.DeleteAsync(enrollmentId);

            _logger.LogInformation("Enrollment deleted successfully with ID: {EnrollmentId}", enrollmentId);
            return true;
        }

        public async Task<IEnumerable<EnrollmentResponse>> GetAllAsync(Expression<Func<Enrollment, bool>>? expression = null)
        {
            _logger.LogInformation("Retrieving enrollments with expression: {HasExpression}", expression != null);

            var enrollments = await _enrollmentRepository.GetAllAsync(expression);
            if (!enrollments.Any())
            {
                _logger.LogInformation("No enrollments found matching the criteria");
                return Enumerable.Empty<EnrollmentResponse>();
            }

            _logger.LogDebug("Found {Count} enrollments, processing responses", enrollments.Count());
            var responses = new List<EnrollmentResponse>();
            foreach (var enrollment in enrollments)
            {
                _logger.LogDebug("Processing enrollment ID: {EnrollmentId}", enrollment.EnrollmentID);
                var (user, course) = await GetUserAndCourseAsync(enrollment.UserID, enrollment.CourseID);
                responses.Add(MapToResponse(enrollment, user, course));
            }

            _logger.LogInformation("Successfully retrieved {Count} enrollments", responses.Count);
            return responses;
        }

        public async Task<EnrollmentResponse?> GetByAsync(Expression<Func<Enrollment, bool>> predicate)
        {
            _logger.LogInformation("Retrieving enrollment by predicate");

            var enrollment = await _enrollmentRepository.GetByAsync(predicate);
            if (enrollment == null)
            {
                _logger.LogInformation("No enrollment found matching the predicate");
                return null;
            }

            _logger.LogDebug("Found enrollment ID: {EnrollmentId}, retrieving related data", enrollment.EnrollmentID);
            var (user, course) = await GetUserAndCourseAsync(enrollment.UserID, enrollment.CourseID);
            var response = MapToResponse(enrollment, user, course);

            _logger.LogInformation("Successfully retrieved enrollment with ID: {EnrollmentId}", enrollment.EnrollmentID);
            return response;
        }

        public async Task<EnrollmentResponse> UpdateEnrollmentAsync(EnrollmentUpdateRequest request)
        {
            if (request == null)
            {
                _logger.LogWarning("UpdateEnrollmentAsync received null request");
                throw new ArgumentNullException(nameof(request));
            }

            _logger.LogInformation("Starting enrollment update for ID: {EnrollmentId}", request.EnrollmentID);

            var (user, course) = await GetUserAndCourseAsync(request.UserID, request.CourseID);

            var enrollment = await _enrollmentRepository.GetByAsync(x => x.EnrollmentID == request.EnrollmentID);
            if (enrollment == null)
            {
                _logger.LogWarning("Enrollment not found for update with ID: {EnrollmentId}", request.EnrollmentID);
                throw new KeyNotFoundException("Enrollment not found");
            }

            _logger.LogDebug("Updating enrollment with ID: {EnrollmentId}", enrollment.EnrollmentID);
            _mapper.Map(request, enrollment);
            await _enrollmentRepository.UpdateAsync(enrollment);

            var response = MapToResponse(enrollment, user, course);
            _logger.LogInformation("Enrollment updated successfully with ID: {EnrollmentId}", enrollment.EnrollmentID);
            return response;
        }

        private async Task<(UserDto?, CourseDto?)> GetUserAndCourseAsync(Guid userId, Guid courseId)
        {
            _logger.LogInformation("Fetching user and course data for UserID: {UserId}, CourseID: {CourseId}",
                userId, courseId);

            var userTask = _userClient.GetUserInfo(userId);
            var courseTask = _courseClient.GetCourseInfo(courseId);

            try
            {
                await Task.WhenAll(userTask, courseTask);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch user or course data for UserID: {UserId}, CourseID: {CourseId}",
                    userId, courseId);
                throw;
            }

            var user = userTask.Result;
            var course = courseTask.Result;

            if (user == null)
            {
                _logger.LogWarning("User not found for ID: {UserId}", userId);
                throw new KeyNotFoundException("User not found");
            }

            if (course == null)
            {
                _logger.LogWarning("Course not found for ID: {CourseId}", courseId);
                throw new KeyNotFoundException("Course not found");
            }

            _logger.LogDebug("Successfully retrieved user and course data for UserID: {UserId}, CourseID: {CourseId}",
                userId, courseId);
            return (user, course);
        }

        private EnrollmentResponse MapToResponse(Enrollment enrollment, UserDto user, CourseDto course)
        {
            _logger.LogDebug("Mapping enrollment to response for ID: {EnrollmentId}", enrollment.EnrollmentID);

            var response = _mapper.Map<EnrollmentResponse>(enrollment);
            response.UserName = user.FullName;
            response.CourseName = course.Title;
            response.PosterUrl = course.PosterUrl;
            response.CategoryID = course.CategoryID;
            response.CategoryName = course.CategoryName;

            return response;
        }
    }
}