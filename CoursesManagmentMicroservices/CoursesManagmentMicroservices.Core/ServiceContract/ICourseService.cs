using CoursesManagmentMicroservices.Core.Domain.Entities;
using CoursesManagmentMicroservices.Core.Dtos;
using CoursesManagmentMicroservices.Core.Dtos.CourseDto;
using System.Linq.Expressions;

namespace CoursesManagmentMicroservices.Core.ServiceContract
{
    public interface ICourseService
    {
        Task<CourseResponse> CreateAsync(CourseAddRequest? courseAddRequest);
        Task<CourseResponse> UpdateAsync(CourseUpdateRequest? courseUpdateRequest);
        Task<bool> DeleteAsync(Guid courseID);
        Task<PaginatedResponse<CourseResponse>> GetAllAsync
            (Expression<Func<Course,bool>>? expression , PaginationDto? pagination);

        Task<CourseResponse?> GetByAsync(Expression<Func<Course, bool>> predicate, bool isTracked = false);
    }
}
