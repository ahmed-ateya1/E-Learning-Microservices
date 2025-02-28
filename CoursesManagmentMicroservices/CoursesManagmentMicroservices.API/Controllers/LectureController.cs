using CoursesManagmentMicroservices.Core.Dtos;
using CoursesManagmentMicroservices.Core.Dtos.LectureDto;
using CoursesManagmentMicroservices.Core.ServiceContract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CoursesManagmentMicroservices.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LectureController : ControllerBase
    {
        private readonly ILectureService _lectureService;
        private readonly ILogger<LectureController> _logger;

        public LectureController(ILectureService lectureService, ILogger<LectureController> logger)
        {
            _lectureService = lectureService;
            _logger = logger;
        }
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
