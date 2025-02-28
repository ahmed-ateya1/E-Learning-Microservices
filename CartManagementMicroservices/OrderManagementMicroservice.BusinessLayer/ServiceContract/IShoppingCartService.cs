using CartManagementMicroservice.DataAccessLayer.Entities;
using OrderManagementMicroservice.DataAccessLayer.Entities;

namespace CartManagementMicroservice.BusinessLayer.ServiceContract
{
    public interface IShoppingCartService
    {
        Task<Cart> GetCartAsync(string userId, CancellationToken cancellationToken = default);
        Task AddToCartAsync(string userId, CartItems item, CancellationToken cancellationToken = default);
        Task RemoveFromCartAsync(string userId, Guid courseID, CancellationToken cancellationToken = default);
        Task ClearCartAsync(string userId, CancellationToken cancellationToken = default);
    }
}
