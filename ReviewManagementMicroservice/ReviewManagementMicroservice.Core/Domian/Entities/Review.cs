namespace ReviewManagementMicroservice.Core.Domian.Entities
{
    public class Review
    {
        public Guid ReviewID { get; set; }
        public string ReviewText { get; set; }
        public DateTime ReviewDate { get; set; }
        public int Rating { get; set; }
        public Guid? BaseReviewID { get; set; }
        public virtual Review BaseReview { get; set; }
        public virtual ICollection<Review> Replies { get; set; }
        public Guid UserID { get; set; }
        public Guid CourseID { get; set; }
    }
}
