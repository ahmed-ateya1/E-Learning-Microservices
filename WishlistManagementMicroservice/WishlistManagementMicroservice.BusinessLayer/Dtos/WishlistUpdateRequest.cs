namespace WishlistManagementMicroservice.BusinessLayer.Dtos
{
    public class WishlistUpdateRequest
    {
        public Guid WishlistID { get; set; }
        public Guid UserID { get; set; }
        public Guid CourseID { get; set; }
    }
}
