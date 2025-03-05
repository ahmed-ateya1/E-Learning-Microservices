using ReviewManagementMicroservice.Core.Dtos.ExternalDto;

namespace ReviewManagementMicroservice.Core.HttpClients
{
    public interface ICourseMicroserviceClient
    {
        Task<CourseDto> GetCourseInfo(Guid courseId);
    }
}
