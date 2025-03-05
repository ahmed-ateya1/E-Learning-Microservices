using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReviewManagementMicroservice.Core.Dtos;
using ReviewManagementMicroservice.Core.Dtos.ExternalDto;
using ReviewManagementMicroservice.Core.ServiceContract;
using System.Net;

namespace ReviewManagementMicroservice.API.Controllers
{
    /// <summary>
    /// Provides API endpoints for managing reviews in the review management microservice.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly ILogger<ReviewController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReviewController"/> class.
        /// </summary>
        /// <param name="reviewService">The service responsible for review operations.</param>
        /// <param name="logger">The logger instance for logging controller activities.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="reviewService"/> or <paramref name="logger"/> is null.</exception>
        public ReviewController(IReviewService reviewService, ILogger<ReviewController> logger)
        {
            _reviewService = reviewService ?? throw new ArgumentNullException(nameof(reviewService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogInformation("ReviewController initialized.");
        }

        /// <summary>
        /// Adds a new review for a user and course.
        /// </summary>
        /// <param name="request">The request containing review details, including user ID and course ID.</param>
        /// <returns>
        /// Returns an <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse{T}"/> with the created review if successful,
        /// or an error response if the request is invalid or the operation fails.
        /// </returns>
        /// <response code="201">Returns the newly created review.</response>
        /// <response code="400">Returns if the request is invalid (e.g., null request).</response>
        /// <response code="500">Returns if an internal server error occurs during review creation.</response>
        [HttpPost("addReview")]
        public async Task<ActionResult<ApiResponse<ReviewResponse>>> AddReview(ReviewAddRequest request)
        {
            if (request == null)
            {
                _logger.LogError("Received null request for AddReview endpoint.");
                return BadRequest(new ApiResponse<ReviewResponse>
                {
                    Message = "Invalid request: ReviewAddRequest is null.",
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                });
            }

            _logger.LogInformation("Processing AddReview request for User {UserID} and Course {CourseID}", request.UserID, request.CourseID);

            var response = await _reviewService.CreateAsync(request);

            if (response == null)
            {
                _logger.LogWarning("Failed to add review for User {UserID} and Course {CourseID}", request.UserID, request.CourseID);
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse<ReviewResponse>
                {
                    Message = "Failed to add review due to an internal server error.",
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                });
            }

            _logger.LogInformation("Successfully added review with ID {ReviewID} for User {UserID} and Course {CourseID}", response.ReviewID, request.UserID, request.CourseID);
            return Ok(new ApiResponse<ReviewResponse>
            {
                Message = "Review added successfully.",
                StatusCode = HttpStatusCode.Created,
                IsSuccess = true,
                Result = response
            });
        }

        /// <summary>
        /// Updates an existing review for a user and course.
        /// </summary>
        /// <param name="request">The request containing updated review details, including review ID, user ID, and course ID.</param>
        /// <returns>
        /// Returns an <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse{T}"/> with the updated review if successful,
        /// or an error response if the request is invalid or the operation fails.
        /// </returns>
        /// <response code="200">Returns the updated review.</response>
        /// <response code="400">Returns if the request is invalid (e.g., null request).</response>
        /// <response code="500">Returns if an internal server error occurs during review update.</response>
        [HttpPut("updateReview")]
        public async Task<ActionResult<ApiResponse<ReviewResponse>>> UpdateReview(ReviewUpdateRequest request)
        {
            if (request == null)
            {
                _logger.LogError("Received null request for UpdateReview endpoint.");
                return BadRequest(new ApiResponse<ReviewResponse>
                {
                    Message = "Invalid request: ReviewUpdateRequest is null.",
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                });
            }

            _logger.LogInformation("Processing UpdateReview request for ReviewID {ReviewID}", request.ReviewID);

            var response = await _reviewService.UpdateAsync(request);

            if (response == null)
            {
                _logger.LogWarning("Failed to update review with ID {ReviewID}", request.ReviewID);
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse<ReviewResponse>
                {
                    Message = "Failed to update review due to an internal server error.",
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                });
            }

            _logger.LogInformation("Successfully updated review with ID {ReviewID} for User {UserID} and Course {CourseID}", response.ReviewID, request.UserID, request.CourseID);
            return Ok(new ApiResponse<ReviewResponse>
            {
                Message = "Review updated successfully.",
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = response
            });
        }

        /// <summary>
        /// Deletes a review by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the review to delete.</param>
        /// <returns>
        /// Returns an <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse{T}"/> indicating success if the review is deleted,
        /// or an error response if the review is not found or cannot be deleted.
        /// </returns>
        /// <response code="204">Returns if the review is successfully deleted.</response>
        /// <response code="404">Returns if the review is not found.</response>
        [HttpDelete("deleteReview/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteReview(Guid id)
        {
            _logger.LogInformation("Processing DeleteReview request for ReviewID {ReviewID}", id);

            var response = await _reviewService.DeleteAsync(id);

            if (!response)
            {
                _logger.LogWarning("Failed to delete review with ID {ReviewID}", id);
                return NotFound(new ApiResponse<bool>
                {
                    Message = "Review not found or failed to delete.",
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false,
                    Result = false
                });
            }

            _logger.LogInformation("Successfully deleted review with ID {ReviewID}", id);
            return Ok(new ApiResponse<bool>
            {
                Message = "Review deleted successfully.",
                StatusCode = HttpStatusCode.NoContent,
                IsSuccess = true,
                Result = true
            });
        }

        /// <summary>
        /// Retrieves a specific review by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the review to retrieve.</param>
        /// <returns>
        /// Returns an <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse{T}"/> with the review if found,
        /// or an error response if the review is not found.
        /// </returns>
        /// <response code="200">Returns the requested review.</response>
        /// <response code="404">Returns if the review is not found.</response>
        [HttpGet("getReview/{id}")]
        public async Task<ActionResult<ApiResponse<ReviewResponse>>> GetReview(Guid id)
        {
            _logger.LogInformation("Processing GetReview request for ReviewID {ReviewID}", id);

            var response = await _reviewService.GetByAsync(x => x.ReviewID == id);

            if (response == null)
            {
                _logger.LogWarning("Failed to get review with ID {ReviewID}", id);
                return NotFound(new ApiResponse<ReviewResponse>
                {
                    Message = "Review not found.",
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false,
                });
            }

            _logger.LogInformation("Successfully retrieved review with ID {ReviewID}", response.ReviewID);
            return Ok(new ApiResponse<ReviewResponse>
            {
                Message = "Review retrieved successfully.",
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = response
            });
        }

        /// <summary>
        /// Retrieves all reviews.
        /// </summary>
        /// <returns>
        /// Returns an <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse{T}"/> with all reviews if found,
        /// or an error response if no reviews exist.
        /// </returns>
        /// <response code="200">Returns the list of all reviews.</response>
        /// <response code="404">Returns if no reviews are found.</response>
        [HttpGet("getAllReviews")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ReviewResponse>>>> GetAllReviews()
        {
            _logger.LogInformation("Processing GetAllReviews request");

            var response = await _reviewService.GetAllAsync();

            if (response == null || !response.Any())
            {
                _logger.LogWarning("No reviews found for GetAllReviews request");
                return NotFound(new ApiResponse<IEnumerable<ReviewResponse>>
                {
                    Message = "No reviews found.",
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false,
                });
            }

            _logger.LogInformation("Successfully retrieved {Count} reviews", response.Count());
            return Ok(new ApiResponse<IEnumerable<ReviewResponse>>
            {
                Message = "Reviews retrieved successfully.",
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = response
            });
        }

        /// <summary>
        /// Retrieves all reviews for a specific course.
        /// </summary>
        /// <param name="courseId">The unique identifier of the course.</param>
        /// <returns>
        /// Returns an <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse{T}"/> with reviews for the specified course if found,
        /// or an error response if no reviews exist for the course.
        /// </returns>
        /// <response code="200">Returns the list of reviews for the course.</response>
        /// <response code="404">Returns if no reviews are found for the specified course.</response>
        [HttpGet("getReviewsByCourse/{courseId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ReviewResponse>>>> GetReviewsByCourse(Guid courseId)
        {
            _logger.LogInformation("Processing GetReviewsByCourse request for CourseID {CourseID}", courseId);

            var response = await _reviewService.GetAllAsync(x => x.CourseID == courseId);

            if (response == null || !response.Any())
            {
                _logger.LogWarning("No reviews found for CourseID {CourseID}", courseId);
                return NotFound(new ApiResponse<IEnumerable<ReviewResponse>>
                {
                    Message = "No reviews found for the specified course.",
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false,
                });
            }

            _logger.LogInformation("Successfully retrieved {Count} reviews for CourseID {CourseID}", response.Count(), courseId);
            return Ok(new ApiResponse<IEnumerable<ReviewResponse>>
            {
                Message = "Reviews retrieved successfully.",
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = response
            });
        }

        /// <summary>
        /// Retrieves all reviews for a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>
        /// Returns an <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse{T}"/> with reviews for the specified user if found,
        /// or an error response if no reviews exist for the user.
        /// </returns>
        /// <response code="200">Returns the list of reviews for the user.</response>
        /// <response code="404">Returns if no reviews are found for the specified user.</response>
        [HttpGet("getReviewsByUser/{userId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ReviewResponse>>>> GetReviewsByUser(Guid userId)
        {
            _logger.LogInformation("Processing GetReviewsByUser request for UserID {UserID}", userId);

            var response = await _reviewService.GetAllAsync(x => x.UserID == userId);

            if (response == null || !response.Any())
            {
                _logger.LogWarning("No reviews found for UserID {UserID}", userId);
                return NotFound(new ApiResponse<IEnumerable<ReviewResponse>>
                {
                    Message = "No reviews found for the specified user.",
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false,
                });
            }

            _logger.LogInformation("Successfully retrieved {Count} reviews for UserID {UserID}", response.Count(), userId);
            return Ok(new ApiResponse<IEnumerable<ReviewResponse>>
            {
                Message = "Reviews retrieved successfully.",
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = response
            });
        }

        /// <summary>
        /// Retrieves all reviews with a specific rating.
        /// </summary>
        /// <param name="rating">The rating value to filter reviews.</param>
        /// <returns>
        /// Returns an <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse{T}"/> with reviews matching the specified rating if found,
        /// or an error response if no reviews exist with that rating.
        /// </returns>
        /// <response code="200">Returns the list of reviews with the specified rating.</response>
        /// <response code="404">Returns if no reviews are found for the specified rating.</response>
        [HttpGet("getReviewsByRating/{rating}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ReviewResponse>>>> GetReviewsByRating(int rating)
        {
            _logger.LogInformation("Processing GetReviewsByRating request for Rating {Rating}", rating);

            var response = await _reviewService.GetAllAsync(x => x.Rating == rating);

            if (response == null || !response.Any())
            {
                _logger.LogWarning("No reviews found for Rating {Rating}", rating);
                return NotFound(new ApiResponse<IEnumerable<ReviewResponse>>
                {
                    Message = "No reviews found for the specified rating.",
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false,
                });
            }

            _logger.LogInformation("Successfully retrieved {Count} reviews for Rating {Rating}", response.Count(), rating);
            return Ok(new ApiResponse<IEnumerable<ReviewResponse>>
            {
                Message = "Reviews retrieved successfully.",
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = response
            });
        }

        /// <summary>
        /// Retrieves all reviews associated with a specific base review.
        /// </summary>
        /// <param name="baseReviewId">The unique identifier of the base review.</param>
        /// <returns>
        /// Returns an <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse{T}"/> with reviews for the specified base review if found,
        /// or an error response if no reviews exist for the base review.
        /// </returns>
        /// <response code="200">Returns the list of reviews for the base review.</response>
        /// <response code="404">Returns if no reviews are found for the specified base review.</response>
        [HttpGet("getReviewsByBaseReview/{baseReviewId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ReviewResponse>>>> GetReviewsByBaseReview(Guid baseReviewId)
        {
            _logger.LogInformation("Processing GetReviewsByBaseReview request for BaseReviewID {BaseReviewID}", baseReviewId);

            var response = await _reviewService.GetAllAsync(x => x.BaseReviewID == baseReviewId);

            if (response == null || !response.Any())
            {
                _logger.LogWarning("No reviews found for BaseReviewID {BaseReviewID}", baseReviewId);
                return NotFound(new ApiResponse<IEnumerable<ReviewResponse>>
                {
                    Message = "No reviews found for the specified base review.",
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false,
                });
            }

            _logger.LogInformation("Successfully retrieved {Count} reviews for BaseReviewID {BaseReviewID}", response.Count(), baseReviewId);
            return Ok(new ApiResponse<IEnumerable<ReviewResponse>>
            {
                Message = "Reviews retrieved successfully.",
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = response
            });
        }

        /// <summary>
        /// Retrieves all reviews for a specific date.
        /// </summary>
        /// <param name="date">The date to filter reviews.</param>
        /// <returns>
        /// Returns an <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse{T}"/> with reviews for the specified date if found,
        /// or an error response if no reviews exist for that date.
        /// </returns>
        /// <response code="200">Returns the list of reviews for the date.</response>
        /// <response code="404">Returns if no reviews are found for the specified date.</response>
        [HttpGet("getReviewsByDate/{date}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ReviewResponse>>>> GetReviewsByDate(DateTime date)
        {
            _logger.LogInformation("Processing GetReviewsByDate request for Date {Date}", date.ToString("yyyy-MM-dd"));

            var response = await _reviewService.GetAllAsync(x => x.ReviewDate == date);

            if (response == null || !response.Any())
            {
                _logger.LogWarning("No reviews found for Date {Date}", date.ToString("yyyy-MM-dd"));
                return NotFound(new ApiResponse<IEnumerable<ReviewResponse>>
                {
                    Message = "No reviews found for the specified date.",
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false,
                });
            }

            _logger.LogInformation("Successfully retrieved {Count} reviews for Date {Date}", response.Count(), date.ToString("yyyy-MM-dd"));
            return Ok(new ApiResponse<IEnumerable<ReviewResponse>>
            {
                Message = "Reviews retrieved successfully.",
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = response
            });
        }
    }
}