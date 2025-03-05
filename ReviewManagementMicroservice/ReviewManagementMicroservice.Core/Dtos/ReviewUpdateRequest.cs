namespace ReviewManagementMicroservice.Core.Dtos
{
    public class ReviewUpdateRequest
    {
        public Guid ReviewID { get; set; }
        public string ReviewText { get; set; }
        public int Rating { get; set; }
        public Guid UserID { get; set; }
        public Guid CourseID { get; set; }
    }
}
