using CoursesManagmentMicroservices.Core.Domain.Entities;
using CoursesManagmentMicroservices.Core.Dtos.CategoryDto;
using System.Linq.Expressions;

namespace CoursesManagmentMicroservices.Core.ServiceContract
{
    public interface ICategoryService
    {
        Task<CategoryResponse> CreateAsync(CategoryAddRequest? categoryAddRequest);
        Task<CategoryResponse> UpdateAsync(CategoryUpdateRequest? categoryUpdateRequest);
        Task<bool> DeleteAsync(Guid categoryID);
        Task<CategoryResponse?> GetByAsync(Expression<Func<Category, bool>> predicate , bool isTracked=false);
        Task<IEnumerable<CategoryResponse>> GetAllAsync(Expression<Func<Category,bool>>? predicate= null);

    }
}
