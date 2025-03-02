using EnrollmentManagementMicroservice.BusinessLayer.Dtos.ExternalDto;

namespace EnrollmentManagementMicroservice.BusinessLayer.HttpClients
{
    public interface ICourseMicroserviceClient 
    {
        Task<CourseDto> GetCourseInfo(Guid courseId);
    }
}
