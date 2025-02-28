using CartManagementMicroservice.DataAccessLayer.Entities;

namespace OrderManagementMicroservice.DataAccessLayer.Entities
{
    public class Cart
    {
        public List<CartItems> CartItems { get; set; } = [];
    }
}
