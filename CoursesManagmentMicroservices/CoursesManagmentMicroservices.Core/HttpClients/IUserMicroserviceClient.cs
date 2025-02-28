using CoursesManagmentMicroservices.Core.Dtos.ExternalDto;

namespace CoursesManagmentMicroservices.Core.HttpClients
{
    public interface IUserMicroserviceClient
    {
        Task<UserDto?> GetUserAsync(Guid userId);
    }
}
