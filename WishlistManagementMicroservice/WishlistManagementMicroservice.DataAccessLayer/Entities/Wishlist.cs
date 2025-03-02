namespace WishlistManagementMicroservice.DataAccessLayer.Entities
{
    public class Wishlist
    {
        public Guid WishlistID { get; set; }
        public Guid UserID { get; set; }
        public Guid CourseID { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
