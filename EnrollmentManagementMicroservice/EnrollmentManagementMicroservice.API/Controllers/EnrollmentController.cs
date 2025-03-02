using EnrollmentManagementMicroservice.BusinessLayer.Dtos;
using EnrollmentManagementMicroservice.BusinessLayer.Dtos.ExternalDto;
using EnrollmentManagementMicroservice.BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EnrollmentManagementMicroservice.API.Controllers
{
    /// <summary>
    /// Controller for managing enrollment operations in the system
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly ILogger<EnrollmentController> _logger;

        /// <summary>
        /// Initializes a new instance of the EnrollmentController
        /// </summary>
        /// <param name="enrollmentService">The enrollment service instance</param>
        /// <param name="logger">The logger instance for logging controller operations</param>
        public EnrollmentController(IEnrollmentService enrollmentService, ILogger<EnrollmentController> logger)
        {
            _enrollmentService = enrollmentService ?? throw new ArgumentNullException(nameof(enrollmentService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Enrolls a student in a course
        /// </summary>
        /// <param name="request">The enrollment request details</param>
        /// <returns>API response containing enrollment result</returns>
        [HttpPost("enroll")]
        public async Task<ActionResult<ApiResponse<EnrollmentResponse>>> EnrollStudent([FromBody] EnrollmentAddRequest request)
        {
            _logger.LogInformation("Starting student enrollment process with request: {@Request}", request);

            var response = await _enrollmentService.AddEnrollmentAsync(request);
            if (response == null)
            {
                _logger.LogWarning("Enrollment failed for request: {@Request}", request);
                return BadRequest(new ApiResponse<EnrollmentResponse>
                {
                    IsSuccess = false,
                    Message = "Enrollment failed",
                    StatusCode = HttpStatusCode.BadRequest,
                    Result = null
                });
            }

            _logger.LogInformation("Student enrolled successfully. Response: {@Response}", response);
            return Ok(new ApiResponse<EnrollmentResponse>
            {
                IsSuccess = true,
                Message = "Enrollment successful",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Updates an existing enrollment
        /// </summary>
        /// <param name="request">The enrollment update request details</param>
        /// <returns>API response containing updated enrollment result</returns>
        [HttpPut("update")]
        public async Task<ActionResult<ApiResponse<EnrollmentResponse>>> UpdateEnrollment([FromBody] EnrollmentUpdateRequest request)
        {
            _logger.LogInformation("Starting enrollment update process with request: {@Request}", request);

            var response = await _enrollmentService.UpdateEnrollmentAsync(request);
            if (response == null)
            {
                _logger.LogWarning("Enrollment update failed for request: {@Request}", request);
                return BadRequest(new ApiResponse<EnrollmentResponse>
                {
                    IsSuccess = false,
                    Message = "Enrollment update failed",
                    StatusCode = HttpStatusCode.BadRequest,
                    Result = null
                });
            }

            _logger.LogInformation("Enrollment updated successfully. Response: {@Response}", response);
            return Ok(new ApiResponse<EnrollmentResponse>
            {
                IsSuccess = true,
                Message = "Enrollment update successful",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Deletes an existing enrollment
        /// </summary>
        /// <param name="enrollmentId">The ID of the enrollment to delete</param>
        /// <returns>API response indicating deletion success</returns>
        [HttpDelete("delete/{enrollmentId}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteEnrollment(Guid enrollmentId)
        {
            _logger.LogInformation("Starting enrollment deletion process for ID: {EnrollmentId}", enrollmentId);

            var response = await _enrollmentService.DeleteEnrollmentAsync(enrollmentId);
            if (!response)
            {
                _logger.LogWarning("Enrollment deletion failed for ID: {EnrollmentId}", enrollmentId);
                return BadRequest(new ApiResponse<bool>
                {
                    IsSuccess = false,
                    Message = "Enrollment deletion failed",
                    StatusCode = HttpStatusCode.BadRequest,
                    Result = false
                });
            }

            _logger.LogInformation("Enrollment deleted successfully for ID: {EnrollmentId}", enrollmentId);
            return Ok(new ApiResponse<bool>
            {
                IsSuccess = true,
                Message = "Enrollment deletion successful",
                StatusCode = HttpStatusCode.OK,
                Result = true
            });
        }

        /// <summary>
        /// Gets an enrollment by its ID
        /// </summary>
        /// <param name="enrollmentId">The ID of the enrollment to retrieve</param>
        /// <returns>API response containing the enrollment details</returns>
        [HttpGet("getEnrollmentById/{enrollmentId}")]
        public async Task<ActionResult<ApiResponse<EnrollmentResponse>>> GetEnrollment(Guid enrollmentId)
        {
            _logger.LogInformation("Retrieving enrollment with ID: {EnrollmentId}", enrollmentId);

            var response = await _enrollmentService.GetByAsync(e => e.EnrollmentID == enrollmentId);
            if (response == null)
            {
                _logger.LogWarning("Enrollment not found for ID: {EnrollmentId}", enrollmentId);
                return BadRequest(new ApiResponse<EnrollmentResponse>
                {
                    IsSuccess = false,
                    Message = "Enrollment not found",
                    StatusCode = HttpStatusCode.NotFound,
                    Result = null
                });
            }

            _logger.LogInformation("Enrollment retrieved successfully for ID: {EnrollmentId}", enrollmentId);
            return Ok(new ApiResponse<EnrollmentResponse>
            {
                IsSuccess = true,
                Message = "Enrollment found",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Gets all enrollments in the system
        /// </summary>
        /// <returns>API response containing all enrollments</returns>
        [HttpGet("getAllEnrollments")]
        public async Task<ActionResult<ApiResponse<IEnumerable<EnrollmentResponse>>>> GetAllEnrollments()
        {
            _logger.LogInformation("Retrieving all enrollments");

            var response = await _enrollmentService.GetAllAsync();
            if (response == null)
            {
                _logger.LogWarning("No enrollments found");
                return BadRequest(new ApiResponse<IEnumerable<EnrollmentResponse>>
                {
                    IsSuccess = false,
                    Message = "No enrollments found",
                    StatusCode = HttpStatusCode.NotFound,
                    Result = null
                });
            }

            _logger.LogInformation("Successfully retrieved {Count} enrollments", response.Count());
            return Ok(new ApiResponse<IEnumerable<EnrollmentResponse>>
            {
                IsSuccess = true,
                Message = "Enrollments found",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Gets all enrollments for a specific course
        /// </summary>
        /// <param name="courseId">The ID of the course</param>
        /// <returns>API response containing enrollments for the course</returns>
        [HttpGet("getAllEnrollmentsByCourse/{courseId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<EnrollmentResponse>>>> GetAllEnrollmentsByCourse(Guid courseId)
        {
            _logger.LogInformation("Retrieving enrollments for course ID: {CourseId}", courseId);

            var response = await _enrollmentService.GetAllAsync(e => e.CourseID == courseId);
            if (response == null)
            {
                _logger.LogWarning("No enrollments found for course ID: {CourseId}", courseId);
                return BadRequest(new ApiResponse<IEnumerable<EnrollmentResponse>>
                {
                    IsSuccess = false,
                    Message = "No enrollments found",
                    StatusCode = HttpStatusCode.NotFound,
                    Result = null
                });
            }

            _logger.LogInformation("Successfully retrieved {Count} enrollments for course ID: {CourseId}", response.Count(), courseId);
            return Ok(new ApiResponse<IEnumerable<EnrollmentResponse>>
            {
                IsSuccess = true,
                Message = "Enrollments found",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Gets all enrollments for a specific user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>API response containing enrollments for the user</returns>
        [HttpGet("getAllEnrollmentsByUser/{userId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<EnrollmentResponse>>>> GetAllEnrollmentsByUser(Guid userId)
        {
            _logger.LogInformation("Retrieving enrollments for user ID: {UserId}", userId);

            var response = await _enrollmentService.GetAllAsync(e => e.UserID == userId);
            if (response == null)
            {
                _logger.LogWarning("No enrollments found for user ID: {UserId}", userId);
                return BadRequest(new ApiResponse<IEnumerable<EnrollmentResponse>>
                {
                    IsSuccess = false,
                    Message = "No enrollments found",
                    StatusCode = HttpStatusCode.NotFound,
                    Result = null
                });
            }

            _logger.LogInformation("Successfully retrieved {Count} enrollments for user ID: {UserId}", response.Count(), userId);
            return Ok(new ApiResponse<IEnumerable<EnrollmentResponse>>
            {
                IsSuccess = true,
                Message = "Enrollments found",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }
    }
}