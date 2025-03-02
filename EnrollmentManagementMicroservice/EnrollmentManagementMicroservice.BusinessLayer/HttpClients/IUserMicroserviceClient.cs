using EnrollmentManagementMicroservice.BusinessLayer.Dtos.ExternalDto;

namespace EnrollmentManagementMicroservice.BusinessLayer.HttpClients
{
    public interface IUserMicroserviceClient
    {
        Task<UserDto> GetUserInfo(Guid userId);
    }
}
