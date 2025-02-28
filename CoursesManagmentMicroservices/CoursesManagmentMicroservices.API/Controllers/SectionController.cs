using CoursesManagmentMicroservices.Core.Dtos;
using CoursesManagmentMicroservices.Core.Dtos.SectionDto;
using CoursesManagmentMicroservices.Core.ServiceContract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CoursesManagmentMicroservices.API.Controllers
{
    /// <summary>
    /// Controller responsible for managing section-related operations such as adding, updating, deleting, and retrieving sections.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : ControllerBase
    {
        private readonly ILogger<SectionController> _logger;
        private readonly ISectionServices _sectionServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance used for logging controller activities.</param>
        /// <param name="sectionServices">The service instance used to handle section-related business logic.</param>
        public SectionController(ILogger<SectionController> logger, ISectionServices sectionServices)
        {
            _logger = logger;
            _sectionServices = sectionServices;
        }

        /// <summary>
        /// Adds a new section to the system.
        /// </summary>
        /// <param name="sectionAddRequest">The request object containing details of the section to be added.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse"/> indicating the result of the operation.</returns>
        /// <response code="200">Returns when the section is successfully added.</response>
        /// <response code="400">Returns when an error occurs during the addition process.</response>
        [HttpPost("addSection")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> AddSection(SectionAddRequest sectionAddRequest)
        {
            var response = await _sectionServices.CreateAsync(sectionAddRequest);
            if (response == null)
            {
                return BadRequest(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Failed to add section",
                    StatusCode = HttpStatusCode.BadRequest,
                });
            }
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Section added successfully",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Updates an existing section in the system.
        /// </summary>
        /// <param name="sectionUpdateRequest">The request object containing updated section details.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse"/> indicating the result of the operation.</returns>
        /// <response code="200">Returns when the section is successfully updated.</response>
        /// <response code="400">Returns when an error occurs during the update process.</response>
        [HttpPut("updateSection")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> UpdateSection(SectionUpdateRequest sectionUpdateRequest)
        {
            var response = await _sectionServices.UpdateAsync(sectionUpdateRequest);
            if (response == null)
            {
                return BadRequest(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Failed to update section",
                    StatusCode = HttpStatusCode.BadRequest,
                });
            }
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Section updated successfully",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Deletes a section from the system based on its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the section to delete.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse"/> indicating the result of the operation.</returns>
        /// <response code="200">Returns when the section is successfully deleted.</response>
        /// <response code="400">Returns when an error occurs during the deletion process.</response>
        [HttpDelete("deleteSection/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> DeleteSection(Guid id)
        {
            var response = await _sectionServices.DeleteAsync(id);
            if (!response)
            {
                return BadRequest(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Failed to delete section",
                    StatusCode = HttpStatusCode.BadRequest,
                });
            }
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Section deleted successfully",
                StatusCode = HttpStatusCode.OK,
            });
        }

        /// <summary>
        /// Retrieves a specific section by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the section to retrieve.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse"/> with the section details.</returns>
        /// <response code="200">Returns when the section is successfully retrieved.</response>
        /// <response code="400">Returns when an error occurs during retrieval or the section is not found.</response>
        [HttpGet("getSection/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> GetSection(Guid id)
        {
            var response = await _sectionServices.GetByAsync(x => x.SectionID == id);
            if (response == null)
            {
                return BadRequest(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Failed to get section",
                    StatusCode = HttpStatusCode.BadRequest,
                });
            }
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Section retrieved successfully",
                Result = response,
                StatusCode = HttpStatusCode.OK
            });
        }

        /// <summary>
        /// Retrieves all sections in the system.
        /// </summary>
        /// <returns>An <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse"/> with the list of sections.</returns>
        /// <response code="200">Returns when the sections are successfully retrieved.</response>
        /// <response code="400">Returns when an error occurs during retrieval.</response>
        [HttpGet("getAllSections")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> GetAllSections()
        {
            var response = await _sectionServices.GetAllAsync();
            if (response == null)
            {
                return BadRequest(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Failed to get sections",
                    StatusCode = HttpStatusCode.BadRequest,
                });
            }
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Sections retrieved successfully",
                Result = response,
                StatusCode = HttpStatusCode.OK
            });
        }

        /// <summary>
        /// Retrieves all sections associated with a specific course.
        /// </summary>
        /// <param name="courseId">The unique identifier of the course to filter sections by.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse"/> with the list of sections.</returns>
        /// <response code="200">Returns when the sections are successfully retrieved.</response>
        /// <response code="400">Returns when an error occurs during retrieval.</response>
        [HttpGet("getSectionsByCourse/{courseId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> GetSectionsByCourse(Guid courseId)
        {
            var response = await _sectionServices.GetAllAsync(x => x.CourseID == courseId);
            if (response == null)
            {
                return BadRequest(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Failed to get sections",
                    StatusCode = HttpStatusCode.BadRequest,
                });
            }
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Sections retrieved successfully",
                Result = response,
                StatusCode = HttpStatusCode.OK,
            });
        }

        /// <summary>
        /// Retrieves all visible sections associated with a specific course.
        /// </summary>
        /// <param name="courseId">The unique identifier of the course to filter sections by.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse"/> with the list of visible sections.</returns>
        /// <response code="200">Returns when the visible sections are successfully retrieved.</response>
        /// <response code="400">Returns when an error occurs during retrieval.</response>
        [HttpGet("getSectionsByCourseAndVisible/{courseId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> GetSectionsByCourseAndVisible(Guid courseId)
        {
            var response = await _sectionServices.GetAllAsync(x => x.CourseID == courseId && x.IsVisible);
            if (response == null)
            {
                return BadRequest(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Failed to get sections",
                    StatusCode = HttpStatusCode.BadRequest,
                });
            }
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Sections retrieved successfully",
                Result = response,
                StatusCode = HttpStatusCode.OK,
            });
        }

        /// <summary>
        /// Retrieves all invisible sections associated with a specific course.
        /// </summary>
        /// <param name="courseId">The unique identifier of the course to filter sections by.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse"/> with the list of invisible sections.</returns>
        /// <response code="200">Returns when the invisible sections are successfully retrieved.</response>
        /// <response code="400">Returns when an error occurs during retrieval.</response>
        [HttpGet("getSectionsByCourseAndInvisible/{courseId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> GetSectionsByCourseAndInvisible(Guid courseId)
        {
            var response = await _sectionServices.GetAllAsync(x => x.CourseID == courseId && !x.IsVisible);
            if (response == null)
            {
                return BadRequest(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Failed to get sections",
                    StatusCode = HttpStatusCode.BadRequest,
                });
            }
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Sections retrieved successfully",
                Result = response,
                StatusCode = HttpStatusCode.OK,
            });
        }

        /// <summary>
        /// Retrieves all visible sections in the system.
        /// </summary>
        /// <returns>An <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse"/> with the list of visible sections.</returns>
        /// <response code="200">Returns when the visible sections are successfully retrieved.</response>
        /// <response code="400">Returns when an error occurs during retrieval.</response>
        [HttpGet("getSectionsByVisible")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> GetSectionsByVisible()
        {
            var response = await _sectionServices.GetAllAsync(x => x.IsVisible);
            if (response == null)
            {
                return BadRequest(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Failed to get sections",
                    StatusCode = HttpStatusCode.BadRequest,
                });
            }
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Sections retrieved successfully",
                Result = response,
                StatusCode = HttpStatusCode.OK,
            });
        }

        /// <summary>
        /// Retrieves all invisible sections in the system.
        /// </summary>
        /// <returns>An <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse"/> with the list of invisible sections.</returns>
        /// <response code="200">Returns when the invisible sections are successfully retrieved.</response>
        /// <response code="400">Returns when an error occurs during retrieval.</response>
        [HttpGet("getSectionsByInvisible")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> GetSectionsByInvisible()
        {
            var response = await _sectionServices.GetAllAsync(x => !x.IsVisible);
            if (response == null)
            {
                return BadRequest(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Failed to get sections",
                    StatusCode = HttpStatusCode.BadRequest,
                });
            }
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Sections retrieved successfully",
                Result = response,
                StatusCode = HttpStatusCode.OK,
            });
        }
    }
}