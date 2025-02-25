using CoursesManagmentMicroservices.Core.Dtos;
using CoursesManagmentMicroservices.Core.Dtos.CategoryDto;
using CoursesManagmentMicroservices.Core.ServiceContract;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CoursesManagmentMicroservices.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all categories from the system
        /// </summary>
        /// <returns>An ActionResult containing ApiResponse with all categories or error details</returns>
        /// <response code="200">Returns the list of all categories</response>
        /// <response code="400">If the retrieval operation fails</response>
        [HttpGet("getCategories")]
        public async Task<ActionResult<ApiResponse>> GetCategories()
        {
            var response = await _categoryService.GetAllAsync();
            if (response == null)
            {
                return BadRequest(new ApiResponse
                {
                    Message = "Failed to Get All category.",
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest
                });
            }
            return Ok(new ApiResponse
            {
                Message = "Successfully Get All category.",
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Retrieves all base categories (categories without a parent category)
        /// </summary>
        /// <returns>An ActionResult containing ApiResponse with base categories or error details</returns>
        /// <response code="200">Returns the list of base categories</response>
        /// <response code="404">If no base categories are found</response>
        [HttpGet("getBaseCategories")]
        public async Task<ActionResult<ApiResponse>> GetBaseCategory()
        {
            var response = await _categoryService.GetAllAsync(x => x.BaseCategoryID == null);
            if (response == null)
            {
                return NotFound(new ApiResponse
                {
                    Message = "Category Not Found.",
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound
                });
            }
            return Ok(new ApiResponse
            {
                Message = "Successfully Get category.",
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Retrieves all subcategories (categories with a parent category)
        /// </summary>
        /// <returns>An ActionResult containing ApiResponse with subcategories or error details</returns>
        /// <response code="200">Returns the list of subcategories</response>
        /// <response code="404">If no subcategories are found</response>
        [HttpGet("getSubCatgories")]
        public async Task<ActionResult<ApiResponse>> GetSubCategory()
        {
            var response = await _categoryService.GetAllAsync(x => x.BaseCategoryID != null);
            if (response == null)
            {
                return NotFound(new ApiResponse
                {
                    Message = "Category Not Found.",
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound
                });
            }
            return Ok(new ApiResponse
            {
                Message = "Successfully Get category.",
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Retrieves a specific category by its ID
        /// </summary>
        /// <param name="id">The GUID of the category to retrieve</param>
        /// <returns>An ActionResult containing ApiResponse with the category or error details</returns>
        /// <response code="200">Returns the requested category</response>
        /// <response code="404">If the category with specified ID is not found</response>
        [HttpGet("getCategoryById/{id}")]
        public async Task<ActionResult<ApiResponse>> GetCategoryID(Guid id)
        {
            var response = await _categoryService.GetByAsync(x => x.CategoryID == id);
            if (response == null)
            {
                return NotFound(new ApiResponse
                {
                    Message = "Category Not Found.",
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound
                });
            }
            return Ok(new ApiResponse
            {
                Message = "Successfully Get category.",
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Retrieves categories matching a specific name (case-insensitive partial match)
        /// </summary>
        /// <param name="name">The name or partial name to search for</param>
        /// <returns>An ActionResult containing ApiResponse with matching categories or error details</returns>
        /// <response code="200">Returns the list of matching categories</response>
        /// <response code="404">If no categories match the specified name</response>
        [HttpGet("getCategoryByName/{name}")]
        public async Task<ActionResult<ApiResponse>> GetCategoryByName(string name)
        {
            var response = await _categoryService
                .GetAllAsync(x => x.CategoryName.ToLower().Contains(name.ToLower()));
            if (response == null)
            {
                return NotFound(new ApiResponse
                {
                    Message = "Category Not Found.",
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound
                });
            }
            return Ok(new ApiResponse
            {
                Message = "Successfully Get category.",
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Creates a new category
        /// </summary>
        /// <param name="categoryAddRequest">The category details to be added</param>
        /// <returns>An ActionResult containing ApiResponse with the created category or error details</returns>
        /// <response code="200">Returns the newly created category</response>
        /// <response code="400">If the category creation fails</response>
        [HttpPost("addCategory")]
        public async Task<ActionResult<ApiResponse>> AddCategory(CategoryAddRequest categoryAddRequest)
        {
            var response = await _categoryService.CreateAsync(categoryAddRequest);
            if (response == null)
            {
                return BadRequest(new ApiResponse
                {
                    Message = "Failed to Add category.",
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest
                });
            }
            return Ok(new ApiResponse
            {
                Message = "Successfully Add category.",
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Updates an existing category
        /// </summary>
        /// <param name="categoryUpdateRequest">The updated category details</param>
        /// <returns>An ActionResult containing ApiResponse with the updated category or error details</returns>
        /// <response code="200">Returns the updated category</response>
        /// <response code="400">If the category update fails</response>
        [HttpPut("updateCategory")]
        public async Task<ActionResult<ApiResponse>> UpdateCategory(CategoryUpdateRequest categoryUpdateRequest)
        {
            var response = await _categoryService.UpdateAsync(categoryUpdateRequest);
            if (response == null)
            {
                return BadRequest(new ApiResponse
                {
                    Message = "Failed to update category.",
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest
                });
            }
            return Ok(new ApiResponse
            {
                Message = "Successfully update category.",
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Deletes a category by its ID
        /// </summary>
        /// <param name="caetgoryID">The GUID of the category to delete</param>
        /// <returns>An ActionResult containing ApiResponse with success/failure details</returns>
        /// <response code="200">Indicates successful deletion of the category</response>
        /// <response code="400">If the category deletion fails</response>
        [HttpDelete("deleteCategory/{caetgoryID}")]
        public async Task<ActionResult<ApiResponse>> DeleteCategory(Guid caetgoryID)
        {
            var response = await _categoryService.DeleteAsync(caetgoryID);
            if (!response)
            {
                return BadRequest(new ApiResponse
                {
                    Message = "Failed to delete category.",
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest
                });
            }
            return Ok(new ApiResponse
            {
                Message = "Successfully delete category.",
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK
            });
        }
    }
}