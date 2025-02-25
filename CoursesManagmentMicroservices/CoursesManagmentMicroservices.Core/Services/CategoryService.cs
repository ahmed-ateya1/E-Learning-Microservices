using AutoMapper;
using CoursesManagmentMicroservices.Core.Domain.Entities;
using CoursesManagmentMicroservices.Core.Domain.RepositoryContract;
using CoursesManagmentMicroservices.Core.Dtos.CategoryDto;
using CoursesManagmentMicroservices.Core.ServiceContract;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace CoursesManagmentMicroservices.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ILogger<CategoryService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(ILogger<CategoryService> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        private async Task ExecuteWithTransactionAsync(Func<Task> action)
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    await action();
                    await _unitOfWork.CommitTransactionAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Transaction failed for operation: {Operation}", action.Method.Name);
                    await _unitOfWork.RollbackTransactionAsync();
                    throw;
                }
            }
        }

        public async Task<CategoryResponse> CreateAsync(CategoryAddRequest? categoryAddRequest)
        {
            _logger.LogInformation("Starting category creation for request {@CategoryAddRequest}", categoryAddRequest);
            if (categoryAddRequest == null)
            {
                _logger.LogError("Category creation failed: CategoryAddRequest is null");
                throw new ArgumentNullException(nameof(categoryAddRequest));
            }

            var category = _mapper.Map<Category>(categoryAddRequest);
            _logger.LogDebug("Mapped CategoryAddRequest to Category entity: {@Category}", category);

            await ExecuteWithTransactionAsync(async () =>
            {
                await _unitOfWork.Repository<Category>().CreateAsync(category);
                await _unitOfWork.CompleteAsync();
            });

            _logger.LogInformation("Category created successfully with ID {CategoryID}", category.CategoryID);
            return _mapper.Map<CategoryResponse>(category);
        }

        public async Task<bool> DeleteAsync(Guid categoryID)
        {
            _logger.LogInformation("Attempting to delete category with ID {CategoryID}", categoryID);
            var category = await _unitOfWork.Repository<Category>()
                .GetByAsync(x => x.CategoryID == categoryID, includeProperties: "SubCategories,Courses");

            if (category == null)
            {
                _logger.LogWarning("Category with ID {CategoryID} not found", categoryID);
                return false;
            }
            if (category.SubCategories.Any())
            {
                _logger.LogWarning("Category with ID {CategoryID} has subcategories and cannot be deleted", categoryID);
                return false;
            }
            if (category.Courses.Any())
            {
                _logger.LogWarning("Category with ID {CategoryID} has courses and cannot be deleted", categoryID);
                return false;
            }

            await ExecuteWithTransactionAsync(async () =>
            {
                await _unitOfWork.Repository<Category>().DeleteAsync(category);
                await _unitOfWork.CompleteAsync();
            });

            _logger.LogInformation("Category with ID {CategoryID} deleted successfully", categoryID);
            return true;
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllAsync(
            Expression<Func<Category, bool>>? predicate = null)
        {
            _logger.LogInformation("Retrieving all categories with predicate {@Predicate}", predicate?.ToString());
            var categories = await _unitOfWork.Repository<Category>()
                .GetAllAsync(predicate, "BaseCategory,SubCategories,Courses");

            if (!categories.Any())
            {
                _logger.LogInformation("No categories found matching the predicate");
                return Enumerable.Empty<CategoryResponse>();
            }

            _logger.LogDebug("Retrieved {Count} categories", categories.Count());
            return _mapper.Map<IEnumerable<CategoryResponse>>(categories);
        }

        public async Task<CategoryResponse?> GetByAsync(
            Expression<Func<Category, bool>> predicate,
            bool isTracked = false)
        {
            _logger.LogInformation("Retrieving category with predicate {@Predicate}", predicate.ToString());
            var category = await _unitOfWork.Repository<Category>()
                .GetByAsync(predicate, includeProperties: "BaseCategory,SubCategories,Courses");

            if (category == null)
            {
                _logger.LogWarning("No category found matching the predicate");
                return null;
            }

            _logger.LogDebug("Retrieved category with ID {CategoryID}", category.CategoryID);
            return _mapper.Map<CategoryResponse>(category);
        }

        public async Task<CategoryResponse> UpdateAsync(CategoryUpdateRequest? categoryUpdateRequest)
        {
            _logger.LogInformation("Updating category with request {@CategoryUpdateRequest}", categoryUpdateRequest);
            if (categoryUpdateRequest == null)
            {
                _logger.LogError("Category update failed: CategoryUpdateRequest is null");
                throw new ArgumentNullException(nameof(categoryUpdateRequest));
            }

            var category = await _unitOfWork.Repository<Category>()
                .GetByAsync(x => x.CategoryID == categoryUpdateRequest.CategoryID);
            if (category == null)
            {
                _logger.LogError("Category with ID {CategoryID} not found for update", categoryUpdateRequest.CategoryID);
                throw new ArgumentNullException(nameof(category));
            }

            _mapper.Map(categoryUpdateRequest, category);
            _logger.LogDebug("Mapped CategoryUpdateRequest to existing category: {@Category}", category);

            await ExecuteWithTransactionAsync(async () =>
            {
                await _unitOfWork.Repository<Category>().UpdateAsync(category);
                await _unitOfWork.CompleteAsync();
            });

            _logger.LogInformation("Category with ID {CategoryID} updated successfully", category.CategoryID);
            return _mapper.Map<CategoryResponse>(category);
        }
    }
}