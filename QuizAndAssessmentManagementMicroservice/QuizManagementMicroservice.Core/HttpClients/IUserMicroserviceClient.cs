using QuizManagementMicroservice.Core.Dtos.ExternalDto;

namespace QuizManagementMicroservice.Core.HttpClients
{
    public interface IUserMicroserviceClient
    {
        Task<UserDto?> GetUserAsync(Guid userId);
    }
}
