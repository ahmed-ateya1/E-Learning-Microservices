using ReviewManagementMicroservice.Core.Dtos.ExternalDto;

namespace ReviewManagementMicroservice.Core.HttpClients
{
    public interface IUserMicroserviceClient
    {
        Task<UserDto> GetUserInfo(Guid userId);
    }
}
