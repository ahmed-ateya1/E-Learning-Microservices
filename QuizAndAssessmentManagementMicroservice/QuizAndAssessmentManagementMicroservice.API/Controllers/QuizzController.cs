using Microsoft.AspNetCore.Mvc;
using QuizManagementMicroservice.Core.Dtos;
using QuizManagementMicroservice.Core.Dtos.QuizzDto;
using QuizManagementMicroservice.Core.ServiceContracts;
using System.Net;

namespace QuizAndAssessmentManagementMicroservice.API.Controllers
{
    /// <summary>
    /// Controller for managing quiz-related operations in the Quiz Management Microservice.
    /// Provides endpoints for creating, updating, deleting, and retrieving quizzes.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class QuizzController : ControllerBase
    {
        private readonly IQuizzService _quizzService;
        private readonly ILogger<QuizzController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuizzController"/> class.
        /// </summary>
        /// <param name="quizzService">The quiz service for handling business logic.</param>
        /// <param name="logger">The logger instance for logging controller operations.</param>
        /// <exception cref="ArgumentNullException">Thrown when quizzService or logger is null.</exception>
        public QuizzController(IQuizzService quizzService, ILogger<QuizzController> logger)
        {
            _quizzService = quizzService ?? throw new ArgumentNullException(nameof(quizzService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Adds a new quiz to the system.
        /// </summary>
        /// <param name="request">The request object containing quiz details to be added.</param>
        /// <returns>
        /// An <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse{T}"/> with the created quiz details.
        /// Returns 200 OK on success, 500 Internal Server Error on failure.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when the request is null.</exception>
        [HttpPost("addQuizz")]
        public async Task<ActionResult<ApiResponse<QuizzResponse>>> AddQuizz(QuizzAddRequest request)
        {
            var response = await _quizzService.CreateAsync(request);
            if (response == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<QuizzResponse>
                {
                    IsSuccess = false,
                    Message = "Failed to add quizz",
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
            return Ok(new ApiResponse<QuizzResponse>
            {
                IsSuccess = true,
                Message = "Quizz added successfully",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Updates an existing quiz in the system.
        /// </summary>
        /// <param name="request">The request object containing updated quiz details.</param>
        /// <returns>
        /// An <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse{T}"/> with the updated quiz details.
        /// Returns 200 OK on success, 500 Internal Server Error on failure.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when the request is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the quiz to update is not found.</exception>
        [HttpPut("updateQuizz")]
        public async Task<ActionResult<ApiResponse<QuizzResponse>>> UpdateQuizz(QuizzUpdateRequest request)
        {
            var response = await _quizzService.UpdateAsync(request);
            if (response == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<QuizzResponse>
                {
                    IsSuccess = false,
                    Message = "Failed to update quizz",
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
            return Ok(new ApiResponse<QuizzResponse>
            {
                IsSuccess = true,
                Message = "Quizz updated successfully",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Deletes a quiz from the system by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the quiz to delete.</param>
        /// <returns>
        /// An <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse{T}"/> with the deletion result.
        /// Returns 200 OK on success, 500 Internal Server Error on failure.
        /// </returns>
        [HttpDelete("deleteQuizz/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteQuizz(Guid id)
        {
            var response = await _quizzService.DeleteAsync(id);
            if (!response)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<bool>
                {
                    IsSuccess = false,
                    Message = "Failed to delete quizz",
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
            return Ok(new ApiResponse<bool>
            {
                IsSuccess = true,
                Message = "Quizz deleted successfully",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Retrieves a specific quiz by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the quiz to retrieve.</param>
        /// <returns>
        /// An <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse{T}"/> with the quiz details.
        /// Returns 200 OK on success, 404 Not Found if the quiz doesn't exist.
        /// </returns>
        [HttpGet("getQuizz/{id}")]
        public async Task<ActionResult<ApiResponse<QuizzResponse>>> GetQuizz(Guid id)
        {
            var response = await _quizzService.GetByAsync(q => q.QuizzID == id);
            if (response == null)
            {
                return NotFound(new ApiResponse<QuizzResponse>()
                {
                    IsSuccess = false,
                    Message = "Quizz not found",
                    StatusCode = HttpStatusCode.NotFound
                });
            }
            return Ok(new ApiResponse<QuizzResponse>
            {
                IsSuccess = true,
                Message = "Quizz retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Retrieves all quizzes with pagination support.
        /// </summary>
        /// <param name="pagination">The pagination parameters (page index and size).</param>
        /// <returns>
        /// An <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse{T}"/> with a paginated list of quizzes.
        /// Returns 200 OK on success, 404 Not Found if no quizzes are found.
        /// </returns>
        [HttpGet("getAllQuizzes")]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<QuizzResponse>>>> GetAllQuizzes([FromQuery] PaginationDto pagination)
        {
            var response = await _quizzService.GetAllAsync(pagination: pagination);
            if (response == null)
            {
                return NotFound(new ApiResponse<PaginatedResponse<QuizzResponse>>
                {
                    IsSuccess = false,
                    Message = "No quizzes found",
                    StatusCode = HttpStatusCode.NotFound
                });
            }
            return Ok(new ApiResponse<PaginatedResponse<QuizzResponse>>
            {
                IsSuccess = true,
                Message = "Quizzes retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Retrieves quizzes associated with a specific lecture.
        /// </summary>
        /// <param name="lectureID">The unique identifier of the lecture.</param>
        /// <param name="pagination">The pagination parameters (page index and size).</param>
        /// <returns>
        /// An <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse{T}"/> with a paginated list of quizzes.
        /// Returns 200 OK on success, 404 Not Found if no quizzes are found.
        /// </returns>
        [HttpGet("getQuizzesByLecture/{lectureID}")]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<QuizzResponse>>>> GetQuizzesByLecture(Guid lectureID, [FromQuery] PaginationDto pagination)
        {
            var response = await _quizzService.GetAllAsync(q => q.LectureID == lectureID, pagination);
            if (response == null)
            {
                return NotFound(new ApiResponse<PaginatedResponse<QuizzResponse>>
                {
                    IsSuccess = false,
                    Message = "No quizzes found",
                    StatusCode = HttpStatusCode.NotFound
                });
            }
            return Ok(new ApiResponse<PaginatedResponse<QuizzResponse>>
            {
                IsSuccess = true,
                Message = "Quizzes retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Retrieves quizzes created by a specific instructor.
        /// </summary>
        /// <param name="instructorID">The unique identifier of the instructor.</param>
        /// <param name="pagination">The pagination parameters (page index and size).</param>
        /// <returns>
        /// An <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse{T}"/> with a paginated list of quizzes.
        /// Returns 200 OK on success, 404 Not Found if no quizzes are found.
        /// </returns>
        [HttpGet("getQuizzesByInstructor/{instructorID}")]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<QuizzResponse>>>> GetQuizzesByInstructor(Guid instructorID, [FromQuery] PaginationDto pagination)
        {
            var response = await _quizzService.GetAllAsync(q => q.InstructorID == instructorID, pagination);
            if (response == null)
            {
                return NotFound(new ApiResponse<PaginatedResponse<QuizzResponse>>
                {
                    IsSuccess = false,
                    Message = "No quizzes found",
                    StatusCode = HttpStatusCode.NotFound
                });
            }
            return Ok(new ApiResponse<PaginatedResponse<QuizzResponse>>
            {
                IsSuccess = true,
                Message = "Quizzes retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Retrieves quizzes by title (case-insensitive partial match).
        /// </summary>
        /// <param name="title">The title or partial title to search for.</param>
        /// <param name="pagination">The pagination parameters (page index and size).</param>
        /// <returns>
        /// An <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse{T}"/> with a paginated list of quizzes.
        /// Returns 200 OK on success, 404 Not Found if no quizzes are found.
        /// </returns>
        /// <remarks>
        /// The search is case-insensitive and matches any quiz whose title contains the specified string.
        /// </remarks>
        [HttpGet("getQuizzesByTitle/{title}")]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<QuizzResponse>>>> GetQuizzesByTitle(string title, [FromQuery] PaginationDto pagination)
        {
            var response = await _quizzService.GetAllAsync(q => q.Title.ToUpper().Contains(title.ToUpper()), pagination);
            if (response == null)
            {
                return NotFound(new ApiResponse<PaginatedResponse<QuizzResponse>>
                {
                    IsSuccess = false,
                    Message = "No quizzes found",
                    StatusCode = HttpStatusCode.NotFound
                });
            }
            return Ok(new ApiResponse<PaginatedResponse<QuizzResponse>>
            {
                IsSuccess = true,
                Message = "Quizzes retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Retrieves quizzes by total marks.
        /// </summary>
        /// <param name="totalMarks">The exact total marks to match.</param>
        /// <param name="pagination">The pagination parameters (page index and size).</param>
        /// <returns>
        /// An <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse{T}"/> with a paginated list of quizzes.
        /// Returns 200 OK on success, 404 Not Found if no quizzes are found.
        /// </returns>
        [HttpGet("getQuizzesByMarks/{totalMarks}")]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<QuizzResponse>>>> GetQuizzesByMarks(int totalMarks, [FromQuery] PaginationDto pagination)
        {
            var response = await _quizzService.GetAllAsync(q => q.TotalMarks == totalMarks, pagination);
            if (response == null)
            {
                return NotFound(new ApiResponse<PaginatedResponse<QuizzResponse>>
                {
                    IsSuccess = false,
                    Message = "No quizzes found",
                    StatusCode = HttpStatusCode.NotFound
                });
            }
            return Ok(new ApiResponse<PaginatedResponse<QuizzResponse>>
            {
                IsSuccess = true,
                Message = "Quizzes retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Retrieves quizzes by passing marks.
        /// </summary>
        /// <param name="passMarks">The exact passing marks to match.</param>
        /// <param name="pagination">The pagination parameters (page index and size).</param>
        /// <returns>
        /// An <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse{T}"/> with a paginated list of quizzes.
        /// Returns 200 OK on success, 404 Not Found if no quizzes are found.
        /// </returns>
        [HttpGet("getQuizzesByPassMarks/{passMarks}")]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<QuizzResponse>>>> GetQuizzesByPassMarks(int passMarks, [FromQuery] PaginationDto pagination)
        {
            var response = await _quizzService.GetAllAsync(q => q.PassMarks == passMarks, pagination);
            if (response == null)
            {
                return NotFound(new ApiResponse<PaginatedResponse<QuizzResponse>>
                {
                    IsSuccess = false,
                    Message = "No quizzes found",
                    StatusCode = HttpStatusCode.NotFound
                });
            }
            return Ok(new ApiResponse<PaginatedResponse<QuizzResponse>>
            {
                IsSuccess = true,
                Message = "Quizzes retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }
    }
}