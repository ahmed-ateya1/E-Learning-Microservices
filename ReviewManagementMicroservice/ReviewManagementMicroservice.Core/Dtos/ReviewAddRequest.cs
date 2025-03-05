using ReviewManagementMicroservice.Core.Domian.Entities;

namespace ReviewManagementMicroservice.Core.Dtos
{
    public class ReviewAddRequest
    {
        public string ReviewText { get; set; }
        public int Rating { get; set; }
        public Guid? BaseReviewID { get; set; }
        public Guid UserID { get; set; }
        public Guid CourseID { get; set; }
    }
}
