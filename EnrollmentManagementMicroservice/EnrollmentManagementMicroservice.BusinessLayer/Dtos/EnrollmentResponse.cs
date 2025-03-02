namespace EnrollmentManagementMicroservice.BusinessLayer.Dtos
{
    public class EnrollmentResponse
    {
        public Guid EnrollmentID { get; set; }
        public Guid UserID { get; set; }
        public decimal Progress { get; set; }
        public string UserName { get; set; }
        public Guid CourseID { get; set; }
        public string CourseName { get; set; }
        public string PosterUrl { get; set; }
        public Guid CategoryID { get; set; }
        public string CategoryName { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }
}
