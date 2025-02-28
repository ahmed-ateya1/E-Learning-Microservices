using AutoMapper;
using CoursesManagmentMicroservices.Core.Domain.Entities;
using CoursesManagmentMicroservices.Core.Domain.RepositoryContract;
using CoursesManagmentMicroservices.Core.Dtos;
using CoursesManagmentMicroservices.Core.Dtos.CourseDto;
using CoursesManagmentMicroservices.Core.HttpClients;
using CoursesManagmentMicroservices.Core.ServiceContract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace CoursesManagmentMicroservices.Core.Services
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CourseService> _logger;
        private readonly IFileServices _fileServices;
        private readonly IUserMicroserviceClient _userMicroserviceClient;

        public CourseService(IUnitOfWork unitOfWork,
            ILogger<CourseService> logger,
            IMapper mapper,
            IWebHostEnvironment environment,
            IHttpContextAccessor httpContextAccessor,
            IFileServices fileServices,
            IUserMicroserviceClient userMicroserviceClient)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _fileServices = fileServices;
            _userMicroserviceClient = userMicroserviceClient;
            _logger.LogInformation("CourseService initialized.");
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

        public async Task<CourseResponse> CreateAsync(CourseAddRequest? courseAddRequest)
        {
            _logger.LogInformation("Attempting to create a new course with request: {@Request}", courseAddRequest);
            if (courseAddRequest == null)
            {
                _logger.LogWarning("CourseAddRequest is null.");
                throw new ArgumentNullException(nameof(courseAddRequest));
            }

            var user = await _userMicroserviceClient.GetUserAsync(courseAddRequest.UserID);
            if (user == null)
            {
                _logger.LogWarning("User not found for UserID: {UserID}", courseAddRequest.UserID);
                throw new Exception("User not found");
            }

            var category = await _unitOfWork.Repository<Category>()
                .GetByAsync(x => x.CategoryID == courseAddRequest.CategoryID);
            if (category == null)
            {
                _logger.LogWarning("Category not found for CategoryID: {CategoryID}", courseAddRequest.CategoryID);
                throw new Exception("Category not found");
            }

            var course = _mapper.Map<Course>(courseAddRequest);
            course.Category = category;
            course.CategoryID = category.CategoryID;
            course.PosterUrl = await _fileServices.CreateFileAsync(courseAddRequest.Poster);
            course.CreatedAt = DateTime.UtcNow;

            await ExecuteWithTransactionAsync(async () =>
            {
                await _unitOfWork.Repository<Course>().CreateAsync(course);
                await _unitOfWork.CompleteAsync();
            });

            var response = _mapper.Map<CourseResponse>(course);
            response.UserID = user.UserID;
            response.UserName = user.FullName;
            response.PictuteUrl = user.ProfilePictureUrl;

            _logger.LogInformation("Course created successfully with CourseID: {CourseID}", course.CourseID);
            return response;
        }

        public async Task<bool> DeleteAsync(Guid courseID)
        {
            _logger.LogInformation("Attempting to delete course with CourseID: {CourseID}", courseID);
            var course = await _unitOfWork.Repository<Course>()
                .GetByAsync(x => x.CourseID == courseID, includeProperties: "Sections");
            if (course == null)
            {
                _logger.LogWarning("Course not found for CourseID: {CourseID}", courseID);
                return false;
            }

            await ExecuteWithTransactionAsync(async () =>
            {
                if (!string.IsNullOrEmpty(course.PosterUrl))
                {
                    await _fileServices.DeleteFileAsync(course.PosterUrl);
                    _logger.LogInformation("Deleted poster file: {PosterUrl}", course.PosterUrl);
                }
                if (course.Sections.Any())
                {
                    foreach (var section in course.Sections)
                    {
                        if (section.Lectures.Any())
                        {
                            foreach (var lecture in section.Lectures)
                            {
                                if (!string.IsNullOrEmpty(lecture.ResourceUrl))
                                    await _fileServices.DeleteFileAsync(lecture.ResourceUrl);
                                if (!string.IsNullOrEmpty(lecture.VideoUrl))
                                    await _fileServices.DeleteFileAsync(lecture.VideoUrl);
                                if (!string.IsNullOrEmpty(lecture.FileURL))
                                    await _fileServices.DeleteFileAsync(lecture.FileURL);
                            }
                        }
                    }
                    _logger.LogInformation("Deleted resources for sections of CourseID: {CourseID}", courseID);
                }
                await _unitOfWork.Repository<Course>().DeleteAsync(course);
                await _unitOfWork.CompleteAsync();
            });

            _logger.LogInformation("Course deleted successfully with CourseID: {CourseID}", courseID);
            return true;
        }

        public async Task<PaginatedResponse<CourseResponse>> GetAllAsync
            (Expression<Func<Course, bool>>? expression, PaginationDto? pagination)
        {
            _logger.LogInformation("Retrieving all courses with pagination: {@Pagination}", pagination);
            pagination ??= new PaginationDto();
            var courses = await _unitOfWork.Repository<Course>().GetAllAsync(expression,
                pageSize: pagination.PageSize,
                pageIndex: pagination.PageIndex,
                sortBy: pagination.SortBy,
                sortDirection: pagination.SortDirection,
                includeProperties: "Category");

            if (!courses.Any())
            {
                _logger.LogInformation("No courses found for the given criteria.");
                return new PaginatedResponse<CourseResponse>
                {
                    Items = new List<CourseResponse>(),
                    TotalCount = 0,
                    PageIndex = pagination.PageIndex,
                    PageSize = pagination.PageSize
                };
            }

            var response = _mapper.Map<List<CourseResponse>>(courses);
            await HandleUserInfo(response);
            _logger.LogInformation("Retrieved {Count} courses successfully.", response.Count);
            return new PaginatedResponse<CourseResponse>
            {
                Items = response,
                TotalCount = response.Count,
                PageIndex = pagination.PageIndex,
                PageSize = pagination.PageSize
            };
        }

        public async Task<CourseResponse?> GetByAsync(Expression<Func<Course, bool>> predicate, bool isTracked = false)
        {
            _logger.LogInformation("Retrieving course with predicate.");
            var course = await _unitOfWork.Repository<Course>()
                .GetByAsync(predicate, isTracked, includeProperties: "Category");
            if (course == null)
            {
                _logger.LogWarning("No course found matching the predicate.");
                return null;
            }

            var response = _mapper.Map<CourseResponse>(course);
            await HandleUserInfo(new List<CourseResponse> { response });
            _logger.LogInformation("Course retrieved successfully with CourseID: {CourseID}", course.CourseID);
            return response;
        }

        public async Task<CourseResponse> UpdateAsync(CourseUpdateRequest? courseUpdateRequest)
        {
            _logger.LogInformation("Attempting to update course with request: {@Request}", courseUpdateRequest);
            if (courseUpdateRequest == null)
            {
                _logger.LogWarning("CourseUpdateRequest is null.");
                throw new ArgumentNullException(nameof(courseUpdateRequest));
            }

            var course = await _unitOfWork.Repository<Course>()
                .GetByAsync(x => x.CourseID == courseUpdateRequest.CourseID, includeProperties: "Category");
            if (course == null)
            {
                _logger.LogWarning("Course not found for CourseID: {CourseID}", courseUpdateRequest.CourseID);
                throw new Exception("Course not found");
            }

            var user = await _userMicroserviceClient.GetUserAsync(courseUpdateRequest.UserID);
            if (user == null)
            {
                _logger.LogWarning("User not found for UserID: {UserID}", courseUpdateRequest.UserID);
                throw new Exception("User not found");
            }

            var category = await _unitOfWork.Repository<Category>()
                .GetByAsync(x => x.CategoryID == courseUpdateRequest.CategoryID);
            if (category == null)
            {
                _logger.LogWarning("Category not found for CategoryID: {CategoryID}", courseUpdateRequest.CategoryID);
                throw new Exception("Category not found");
            }

            _mapper.Map(courseUpdateRequest, course);
            course.Category = category;
            course.CategoryID = category.CategoryID;
            course.UpdatedAt = DateTime.UtcNow;
            course.UserID = user.UserID;

            await ExecuteWithTransactionAsync(async () =>
            {
                if (courseUpdateRequest.Poster != null)
                {
                    if (!string.IsNullOrEmpty(course.PosterUrl))
                    {
                        await _fileServices.DeleteFileAsync(course.PosterUrl);
                        _logger.LogInformation("Deleted old poster: {PosterUrl}", course.PosterUrl);
                    }
                    course.PosterUrl = await _fileServices.CreateFileAsync(courseUpdateRequest.Poster);
                    _logger.LogInformation("Uploaded new poster: {PosterUrl}", course.PosterUrl);
                }
                await _unitOfWork.Repository<Course>().UpdateAsync(course);
                await _unitOfWork.CompleteAsync();
            });

            var response = _mapper.Map<CourseResponse>(course);
            response.UserName = user.FullName;
            response.PictuteUrl = user.ProfilePictureUrl;
            _logger.LogInformation("Course updated successfully with CourseID: {CourseID}", course.CourseID);
            return response;
        }

        private async Task HandleUserInfo(IEnumerable<CourseResponse> courses)
        {
            _logger.LogInformation("Fetching user info for {Count} courses.", courses.Count());
            foreach (var course in courses)
            {
                var user = await _userMicroserviceClient.GetUserAsync(course.UserID);
                if (user != null)
                {
                    course.UserID = user.UserID;
                    course.UserName = user.FullName;
                    course.PictuteUrl = user.ProfilePictureUrl;
                }
                else
                {
                    _logger.LogWarning("User not found for UserID: {UserID} in course {CourseID}", course.UserID, course.CourseID);
                }
            }
        }
    }
}