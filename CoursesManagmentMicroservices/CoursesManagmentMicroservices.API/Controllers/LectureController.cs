using CoursesManagmentMicroservices.Core.Dtos;
using CoursesManagmentMicroservices.Core.Dtos.LectureDto;
using CoursesManagmentMicroservices.Core.ServiceContract;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CoursesManagmentMicroservices.API.Controllers
{
    /// <summary>
    /// Provides API endpoints for managing lectures within the course management microservice.
    /// This controller handles CRUD operations and retrieval of lectures based on various criteria such as section, course, and instructor.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LectureController : ControllerBase
    {
        private readonly ILectureService _lectureService;
        private readonly ILogger<LectureController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LectureController"/> class.
        /// </summary>
        /// <param name="lectureService">The service responsible for lecture-related operations.</param>
        /// <param name="logger">The logger instance for logging controller activities.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="lectureService"/> or <paramref name="logger"/> is null.</exception>
        public LectureController(ILectureService lectureService, ILogger<LectureController> logger)
        {
            _lectureService = lectureService ?? throw new ArgumentNullException(nameof(lectureService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Adds a new lecture for a specific section, including associated file content.
        /// </summary>
        /// <param name="lectureAdd">The request containing lecture details and file content, submitted as a form data object.</param>
        /// <returns>
        /// Returns an <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse"/> with the created lecture if successful,
        /// or an error response if the request is invalid or the operation fails.
        /// </returns>
        /// <response code="200">Returns the newly created lecture.</response>
        /// <response code="400">Returns if the request is invalid (e.g., malformed or missing data).</response>
        /// <response code="404">Returns if the lecture creation fails due to a not found resource (e.g., invalid section ID).</response>
        [HttpPost("addLecture")]
        public async Task<ActionResult<ApiResponse>> AddLecture([FromForm] LectureAddRequest lectureAdd)
        {
            var result = await _lectureService.CreateAsync(lectureAdd);
            if (result == null)
            {
                return BadRequest(new ApiResponse
                {
                    Message = "Lecture Bad Request",
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false
                });
            }
            return Ok(new ApiResponse
            {
                Message = "Lecture added successfully",
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = result
            });
        }

        /// <summary>
        /// Updates an existing lecture for a specific section, including associated file content.
        /// </summary>
        /// <param name="lectureUpdate">The request containing updated lecture details and file content, submitted as a form data object.</param>
        /// <returns>
        /// Returns an <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse"/> with the updated lecture if successful,
        /// or an error response if the request is invalid or the operation fails.
        /// </returns>
        /// <response code="200">Returns the updated lecture.</response>
        /// <response code="400">Returns if the request is invalid (e.g., malformed or missing data).</response>
        /// <response code="404">Returns if the lecture update fails due to a not found resource (e.g., invalid lecture ID).</response>
        [HttpPut("updateLecture")]
        public async Task<ActionResult<ApiResponse>> UpdateLecture([FromForm] LectureUpdateRequest lectureUpdate)
        {
            var result = await _lectureService.UpdateAsync(lectureUpdate);
            if (result == null)
            {
                return BadRequest(new ApiResponse
                {
                    Message = "Lecture Bad Request",
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false
                });
            }
            return Ok(new ApiResponse
            {
                Message = "Lecture updated successfully",
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = result
            });
        }

        /// <summary>
        /// Deletes a lecture by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the lecture to delete.</param>
        /// <returns>
        /// Returns an <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse"/> indicating success if the lecture is deleted,
        /// or an error response if the lecture is not found or cannot be deleted.
        /// </returns>
        /// <response code="200">Returns success confirmation for deletion.</response>
        /// <response code="404">Returns if the lecture is not found or cannot be deleted.</response>
        [HttpDelete("deleteLecture/{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteLecture(Guid id)
        {
            var result = await _lectureService.DeleteAsync(id);
            if (!result)
            {
                return BadRequest(new ApiResponse
                {
                    Message = "Lecture Bad Request",
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false
                });
            }
            return Ok(new ApiResponse
            {
                Message = "Lecture deleted successfully",
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true
            });
        }

        /// <summary>
        /// Retrieves a specific lecture by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the lecture to retrieve.</param>
        /// <returns>
        /// Returns an <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse"/> with the lecture if found,
        /// or an error response if the lecture is not found.
        /// </returns>
        /// <response code="200">Returns the requested lecture.</response>
        /// <response code="404">Returns if the lecture is not found.</response>
        [HttpGet("getLecture/{id}")]
        public async Task<ActionResult<ApiResponse>> GetLecture(Guid id)
        {
            var result = await _lectureService.GetByAsync(x => x.LectureID == id);
            if (result == null)
            {
                return BadRequest(new ApiResponse
                {
                    Message = "Lecture Bad Request",
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false
                });
            }
            return Ok(new ApiResponse
            {
                Message = "Lecture retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = result
            });
        }

        /// <summary>
        /// Retrieves all lectures available in the system.
        /// </summary>
        /// <returns>
        /// Returns an <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse"/> with all lectures if found,
        /// or an error response if no lectures exist.
        /// </returns>
        /// <response code="200">Returns the list of all lectures.</response>
        /// <response code="404">Returns if no lectures are found.</response>
        [HttpGet("getAllLectures")]
        public async Task<ActionResult<ApiResponse>> GetAllLectures()
        {
            var result = await _lectureService.GetAllAsync();
            if (result == null)
            {
                return BadRequest(new ApiResponse
                {
                    Message = "Lecture Bad Request",
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false
                });
            }
            return Ok(new ApiResponse
            {
                Message = "Lectures retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = result
            });
        }

        /// <summary>
        /// Retrieves all lectures for a specific section.
        /// </summary>
        /// <param name="sectionID">The unique identifier of the section.</param>
        /// <returns>
        /// Returns an <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse"/> with lectures for the specified section if found,
        /// or an error response if no lectures exist for the section.
        /// </returns>
        /// <response code="200">Returns the list of lectures for the section.</response>
        /// <response code="404">Returns if no lectures are found for the specified section.</response>
        [HttpGet("getLecturesBySection/{sectionID}")]
        public async Task<ActionResult<ApiResponse>> GetLecturesBySection(Guid sectionID)
        {
            var result = await _lectureService.GetAllAsync(x => x.SectionID == sectionID);
            if (result == null)
            {
                return BadRequest(new ApiResponse
                {
                    Message = "Lecture Bad Request",
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false
                });
            }
            return Ok(new ApiResponse
            {
                Message = "Lectures retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = result
            });
        }

        /// <summary>
        /// Retrieves all lectures for a specific course.
        /// </summary>
        /// <param name="courseID">The unique identifier of the course.</param>
        /// <returns>
        /// Returns an <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse"/> with lectures for the specified course if found,
        /// or an error response if no lectures exist for the course.
        /// </returns>
        /// <response code="200">Returns the list of lectures for the course.</response>
        /// <response code="404">Returns if no lectures are found for the specified course.</response>
        [HttpGet("getLecturesByCourse/{courseID}")]
        public async Task<ActionResult<ApiResponse>> GetLecturesByCourse(Guid courseID)
        {
            var result = await _lectureService.GetAllAsync(x => x.Section.CourseID == courseID);
            if (result == null)
            {
                return BadRequest(new ApiResponse
                {
                    Message = "Lecture Bad Request",
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false
                });
            }
            return Ok(new ApiResponse
            {
                Message = "Lectures retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = result
            });
        }

        /// <summary>
        /// Retrieves all lectures created by a specific instructor.
        /// </summary>
        /// <param name="userID">The unique identifier of the instructor (user).</param>
        /// <returns>
        /// Returns an <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse"/> with lectures for the specified instructor if found,
        /// or an error response if no lectures exist for the instructor.
        /// </returns>
        /// <response code="200">Returns the list of lectures for the instructor.</response>
        /// <response code="404">Returns if no lectures are found for the specified instructor.</response>
        [HttpGet("getLecturesByInstructor/{userID}")]
        public async Task<ActionResult<ApiResponse>> GetLecturesByInstructor(Guid userID)
        {
            var result = await _lectureService.GetAllAsync(x => x.Section.Course.UserID == userID);
            if (result == null)
            {
                return BadRequest(new ApiResponse
                {
                    Message = "Lecture Bad Request",
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false
                });
            }
            return Ok(new ApiResponse
            {
                Message = "Lectures retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = result
            });
        }
    }
}