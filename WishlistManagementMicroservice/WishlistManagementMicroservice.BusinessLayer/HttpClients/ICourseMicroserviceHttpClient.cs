using WishlistManagementMicroservice.BusinessLayer.Dtos.ExternalDto;

namespace WishlistManagementMicroservice.BusinessLayer.HttpClients
{
    public interface ICourseMicroserviceHttpClient
    {
        Task<CourseDto> GetCourseInfoAsync(Guid courseID);
    }
}
