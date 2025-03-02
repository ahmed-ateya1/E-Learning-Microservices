namespace EnrollmentManagementMicroservice.DataAccessLayer.Entities
{
    public class Enrollment
    {
        public Guid EnrollmentID { get; set; }
        public Guid UserID { get; set; }
        public Guid CourseID { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public decimal Progress { get; set; }
    }
}
