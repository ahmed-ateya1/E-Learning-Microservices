using WishlistManagementMicroservice.DataAccessLayer.Entities;

namespace WishlistManagementMicroservice.DataAccessLayer.RepoistoryContract
{
    public interface IWishlistRepository
    {
        Task<Wishlist> CreateAsync(Wishlist wishlist);
        Task<bool> DeleteAsync(Guid id);
        Task<Wishlist> GetWishlistByIdAsync(Guid id);
        Task<IEnumerable<Wishlist>> GetWishlistByUserAsync(Guid userId);
        Task<IEnumerable<Wishlist>> GetWishlistByCourseAsync(Guid courseId);
        Task<IEnumerable<Wishlist>> GetAllWishlistsAsync();

    }
}
