using WishlistManagementMicroservice.BusinessLayer.Dtos;

namespace WishlistManagementMicroservice.BusinessLayer.ServiceContract
{
    public interface IWishlistService
    {
        Task<WishlistResponse> CreateAsync(WishlistAddRequest? request);
        Task<bool> DeleteAsync(Guid id);
        Task<WishlistResponse> GetWishlistByIdAsync(Guid wishlistID);
        Task<IEnumerable<WishlistResponse>> GetWishlistByUserIdAsync(Guid userID);
        Task<IEnumerable<WishlistResponse>> GetWishlistByCourseIdAsync(Guid courseID);
        Task<IEnumerable<WishlistResponse>> GetAllAsync();
    }
}
