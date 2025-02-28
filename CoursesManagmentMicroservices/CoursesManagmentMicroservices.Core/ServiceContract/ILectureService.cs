using CoursesManagmentMicroservices.Core.Domain.Entities;
using CoursesManagmentMicroservices.Core.Dtos.LectureDto;
using System.Linq.Expressions;

namespace CoursesManagmentMicroservices.Core.ServiceContract
{
    public interface ILectureService
    {
        Task<LectureResponse> CreateAsync(LectureAddRequest? request);
        Task<LectureResponse> UpdateAsync(LectureUpdateRequest? request);
        Task<bool> DeleteAsync(Guid id);
        Task<LectureResponse?> GetByAsync(Expression<Func<Lecture, bool>> predicate , bool isTracked=false);
        Task<IEnumerable<LectureResponse>> GetAllAsync(Expression<Func<Lecture, bool>>? predicate = null);
    }
}
