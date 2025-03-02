using WishlistManagementMicroservice.BusinessLayer.Dtos.ExternalDto;

namespace WishlistManagementMicroservice.BusinessLayer.HttpClients
{
    public interface IUserMicroserviceHttpClient
    {
        Task<UserDto?> GetUserInfoAsync(Guid userID);
    }
}
