namespace WishlistManagementMicroservice.BusinessLayer.Dtos
{
    public class WishlistAddRequest
    {
        public Guid UserID { get; set; }
        public Guid CourseID { get; set; }
    }
}
