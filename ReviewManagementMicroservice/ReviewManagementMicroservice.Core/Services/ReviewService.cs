using AutoMapper;
using Microsoft.Extensions.Logging;
using ReviewManagementMicroservice.Core.Domian.Entities;
using ReviewManagementMicroservice.Core.Domian.RepositoryContract;
using ReviewManagementMicroservice.Core.Dtos;
using ReviewManagementMicroservice.Core.HttpClients;
using ReviewManagementMicroservice.Core.ServiceContract;
using System.Linq.Expressions;

namespace ReviewManagementMicroservice.Core.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly ILogger<ReviewService> _logger;
        private readonly IMapper _mapper;
        private readonly IUserMicroserviceClient _userMicroserviceClient;
        private readonly ICourseMicroserviceClient _courseMicroserviceClient;

        public ReviewService(
            IReviewRepository reviewRepository,
            ILogger<ReviewService> logger,
            IMapper mapper,
            IUserMicroserviceClient userMicroserviceClient,
            ICourseMicroserviceClient courseMicroserviceClient)
        {
            _reviewRepository = reviewRepository;
            _logger = logger;
            _mapper = mapper;
            _userMicroserviceClient = userMicroserviceClient;
            _courseMicroserviceClient = courseMicroserviceClient;
        }

        public async Task<ReviewResponse> CreateAsync(ReviewAddRequest request)
        {
            var review = _mapper.Map<Review>(request);
            review.ReviewDate = DateTime.UtcNow;
            review.ReviewID = Guid.NewGuid();

            await _reviewRepository.CreateAsync(review);

            var userTask = _userMicroserviceClient.GetUserInfo(request.UserID);
            var courseTask = _courseMicroserviceClient.GetCourseInfo(request.CourseID);
            await Task.WhenAll(userTask, courseTask);

            var user = await userTask;
            var course = await courseTask;
            if (user == null) throw new InvalidOperationException("User not found.");
            if (course == null) throw new InvalidOperationException("Course not found.");
            _logger.LogInformation("User and course found for review {ReviewID}.", review.ReviewID);
            var response = _mapper.Map<ReviewResponse>(review);
            response.FullName = user.FullName;
            response.ProfileImageUrl = user.ProfilePictureUrl;
            response.Title = course.Title;

            return response;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var review = await _reviewRepository.GetByAsync(x => x.ReviewID == id);
            if (review == null)
            {
                _logger.LogWarning("Review with ID {ReviewId} not found for deletion.", id);
                return false;
            }
            return await _reviewRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ReviewResponse>> GetAllAsync(Expression<Func<Review, bool>>? expression = null)
        {
            var reviews = await _reviewRepository.GetAllAsync(expression);
            if (!reviews.Any()) return Enumerable.Empty<ReviewResponse>();

            var responses = _mapper.Map<IEnumerable<ReviewResponse>>(reviews);

            var tasks = responses.Select(async item =>
            {
                var userTask = _userMicroserviceClient.GetUserInfo(item.UserID);
                var courseTask = _courseMicroserviceClient.GetCourseInfo(item.CourseID);
                await Task.WhenAll(userTask, courseTask);

                var user = await userTask;
                var course = await courseTask;

                if (user != null)
                {
                    item.FullName = user.FullName;
                    item.ProfileImageUrl = user.ProfilePictureUrl;
                }

                if (course != null)
                {
                    item.Title = course.Title;
                }
            });

            await Task.WhenAll(tasks);
            _logger.LogInformation("All reviews found.");
            return responses;
        }

        public async Task<ReviewResponse> GetByAsync(Expression<Func<Review, bool>> predicate)
        {
            var review = await _reviewRepository.GetByAsync(predicate);
            if (review == null) return null;

            var response = _mapper.Map<ReviewResponse>(review);

            var userTask = _userMicroserviceClient.GetUserInfo(response.UserID);
            var courseTask = _courseMicroserviceClient.GetCourseInfo(response.CourseID);
            await Task.WhenAll(userTask, courseTask);

            var user = await userTask;
            var course = await courseTask;

            if (user != null)
            {
                _logger.LogInformation("User found for review {ReviewID}.", review.ReviewID);
                response.FullName = user.FullName;
                response.ProfileImageUrl = user.ProfilePictureUrl;
            }

            if (course != null)
            {
                _logger.LogInformation("Course found for review {ReviewID}.", review.ReviewID);
                response.Title = course.Title;
            }

            return response;
        }

        public async Task<ReviewResponse> UpdateAsync(ReviewUpdateRequest request)
        {
            var review = await _reviewRepository.GetByAsync(x => x.ReviewID == request.ReviewID);
            if (review == null) throw new InvalidOperationException("Review not found.");

            _mapper.Map(request, review);
            await _reviewRepository.UpdateAsync(review);

            var userTask = _userMicroserviceClient.GetUserInfo(request.UserID);
            var courseTask = _courseMicroserviceClient.GetCourseInfo(request.CourseID);
            await Task.WhenAll(userTask, courseTask);

            var user = await userTask;
            var course = await courseTask;

            if (user == null) throw new InvalidOperationException("User not found.");
            if (course == null) throw new InvalidOperationException("Course not found.");

            var response = _mapper.Map<ReviewResponse>(review);
            response.FullName = user.FullName;
            response.ProfileImageUrl = user.ProfilePictureUrl;
            response.Title = course.Title;

            return response;
        }
    }
}
