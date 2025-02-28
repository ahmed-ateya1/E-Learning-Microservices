using CoursesManagmentMicroservices.Core.Domain.Entities;
using CoursesManagmentMicroservices.Core.Dtos.SectionDto;
using System.Linq.Expressions;

namespace CoursesManagmentMicroservices.Core.ServiceContract
{
    public interface ISectionServices
    {
        Task<SectionResponse> CreateAsync(SectionAddRequest? request);
        Task<SectionResponse> UpdateAsync(SectionUpdateRequest? request);
        Task<bool> DeleteAsync(Guid id);
        Task<SectionResponse?> GetByAsync(Expression<Func<Section, bool>> predicate , bool isTracked=false);
        Task<IEnumerable<SectionResponse>> GetAllAsync(Expression<Func<Section, bool>>? predicate = null);
    }
}
