using CoursesManagmentMicroservices.Core.Dtos;
using CoursesManagmentMicroservices.Core.Dtos.CourseDto;
using CoursesManagmentMicroservices.Core.MediatRConfig.Command;
using CoursesManagmentMicroservices.Core.MediatRConfig.Queries.CourseQueries;
using CoursesManagmentMicroservices.Core.ServiceContract;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CoursesManagmentMicroservices.API.Controllers
{
    /// <summary>
    /// Controller responsible for managing course-related operations such as adding, updating, deleting, and retrieving courses.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ILogger<CourseController> _logger;
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CourseController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance used for logging controller activities.</param>
        /// <param name="mediator">The MediatR mediator instance used to dispatch commands and queries.</param>
        public CourseController(ILogger<CourseController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Adds a new course to the system.
        /// </summary>
        /// <param name="courseAddRequest">The request object containing details of the course to be added.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse"/> indicating the result of the operation.</returns>
        /// <response code="200">Returns when the course is successfully added.</response>
        /// <response code="400">Returns when an error occurs during the addition process.</response>
        [HttpPost("addCourse")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> AddCourse([FromForm] CourseAddRequest courseAddRequest)
        {
            var response = await _mediator.Send(new AddCourseCommand(courseAddRequest));
            if (response == null)
            {
                return BadRequest(new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "An error occurred while adding the course.",
                    StatusCode = HttpStatusCode.BadRequest
                });
            }

            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Course added successfully.",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Updates an existing course in the system.
        /// </summary>
        /// <param name="courseUpdateRequest">The request object containing updated course details.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse"/> indicating the result of the operation.</returns>
        /// <response code="200">Returns when the course is successfully updated.</response>
        /// <response code="400">Returns when an error occurs during the update process.</response>
        [HttpPut("updateCourse")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> UpdateCourse([FromForm] CourseUpdateRequest courseUpdateRequest)
        {
            var response = await _mediator.Send(new UpdateCourseCommand(courseUpdateRequest));
            if (response == null)
            {
                return BadRequest(new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "An error occurred while updating the course.",
                    StatusCode = HttpStatusCode.BadRequest
                });
            }
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Course updated successfully.",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Deletes a course from the system based on its unique identifier.
        /// </summary>
        /// <param name="courseID">The unique identifier of the course to delete.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse"/> indicating the result of the operation.</returns>
        /// <response code="200">Returns when the course is successfully deleted.</response>
        /// <response code="400">Returns when an error occurs during the deletion process.</response>
        [HttpDelete("deleteCourse/{courseID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> DeleteCourse(Guid courseID)
        {
            var isDeleted = await _mediator.Send(new DeleteCourseCommand(courseID));
            if (!isDeleted)
            {
                return BadRequest(new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "An error occurred while deleting the course.",
                    StatusCode = HttpStatusCode.BadRequest
                });
            }
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Course deleted successfully.",
                StatusCode = HttpStatusCode.OK
            });
        }

        /// <summary>
        /// Retrieves a paginated list of all courses.
        /// </summary>
        /// <param name="pagination">The pagination parameters to control the result set.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse"/> with the list of courses.</returns>
        /// <response code="200">Returns when the courses are successfully retrieved.</response>
        /// <response code="400">Returns when an error occurs during retrieval.</response>
        [HttpGet("getAllCourses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> GetAllCourses([FromQuery] PaginationDto pagination)
        {
            var response = await _mediator.Send(new GetAllCoursesQuery(pagination));
            if (response == null)
            {
                return BadRequest(new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "An error occurred while getting the courses.",
                    StatusCode = HttpStatusCode.BadRequest
                });
            }
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Courses retrieved successfully.",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Retrieves a specific course by its unique identifier.
        /// </summary>
        /// <param name="courseID">The unique identifier of the course to retrieve.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse"/> with the course details.</returns>
        /// <response code="200">Returns when the course is successfully retrieved.</response>
        /// <response code="400">Returns when an error occurs during retrieval.</response>
        [HttpGet("getCourse/{courseID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> GetCourse(Guid courseID)
        {
            var response = await _mediator.Send(new GetCourseQuery(courseID));
            if (response == null)
            {
                return BadRequest(new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "An error occurred while getting the course.",
                    StatusCode = HttpStatusCode.BadRequest
                });
            }
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Course retrieved successfully.",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Retrieves a paginated list of courses by category.
        /// </summary>
        /// <param name="categoryID">The unique identifier of the category to filter courses by.</param>
        /// <param name="pagination">The pagination parameters to control the result set.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse"/> with the list of courses.</returns>
        /// <response code="200">Returns when the courses are successfully retrieved.</response>
        /// <response code="400">Returns when an error occurs during retrieval.</response>
        [HttpGet("getCoursesByCategory/{categoryID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> GetCourseByCategory(Guid categoryID, [FromQuery] PaginationDto pagination)
        {
            var response = await _mediator.Send(new GetCourseByCategoryQuery(categoryID, pagination));
            if (response == null)
            {
                return BadRequest(new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "An error occurred while getting the course.",
                    StatusCode = HttpStatusCode.BadRequest
                });
            }
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Course retrieved successfully.",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Retrieves a paginated list of courses associated with a specific user.
        /// </summary>
        /// <param name="userID">The unique identifier of the user whose courses are to be retrieved.</param>
        /// <param name="pagination">The pagination parameters to control the result set.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse"/> with the list of courses.</returns>
        /// <response code="200">Returns when the courses are successfully retrieved.</response>
        /// <response code="400">Returns when an error occurs during retrieval.</response>
        [HttpGet("getCoursesByUser/{userID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> GetCourseByUser(Guid userID, [FromQuery] PaginationDto pagination)
        {
            var response = await _mediator.Send(new GetCoursesByUserQuery(userID, pagination));
            if (response == null)
            {
                return BadRequest(new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "An error occurred while getting the course.",
                    StatusCode = HttpStatusCode.BadRequest
                });
            }
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Course retrieved successfully.",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Retrieves a paginated list of courses by title.
        /// </summary>
        /// <param name="title">The title or partial title to filter courses by.</param>
        /// <param name="pagination">The pagination parameters to control the result set.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse"/> with the list of courses.</returns>
        /// <response code="200">Returns when the courses are successfully retrieved.</response>
        /// <response code="400">Returns when an error occurs during retrieval.</response>
        [HttpGet("getCoursesByTitle/{title}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> GetCourseByTitle(string title, [FromQuery] PaginationDto pagination)
        {
            var response = await _mediator.Send(new GetCoursesByTitleQuery(title, pagination));
            if (response == null)
            {
                return BadRequest(new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "An error occurred while getting the course.",
                    StatusCode = HttpStatusCode.BadRequest
                });
            }
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Course retrieved successfully.",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }
    }
}