namespace ReviewManagementMicroservice.Core.Dtos
{
    public class ReviewResponse
    {
        public Guid ReviewID { get; set; }
        public string ReviewText { get; set; }
        public DateTime ReviewDate { get; set; }
        public int Rating { get; set; }
        public Guid? BaseReviewID { get; set; }
        public Guid UserID { get; set; }
        public string FullName { get; set; }
        public string ProfileImageUrl { get; set; }
        public Guid CourseID { get; set; }
        public string Title { get; set; }
    }
}
