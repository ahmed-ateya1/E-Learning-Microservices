namespace WishlistManagementMicroservice.BusinessLayer.Dtos
{
    public class WishlistResponse
    {
        public Guid WishlistID { get; set; }
        public Guid UserID { get; set; }
        public string FullName { get; set; }
        public string PictureUrl { get; set; }
        public Guid CourseID { get; set; }
        public string CourseName { get; set; }
        public string PosterUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
