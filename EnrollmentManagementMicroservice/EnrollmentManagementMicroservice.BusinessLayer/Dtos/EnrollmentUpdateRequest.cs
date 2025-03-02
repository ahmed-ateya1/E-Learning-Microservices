namespace EnrollmentManagementMicroservice.BusinessLayer.Dtos
{
    public class EnrollmentUpdateRequest
    {
        public Guid EnrollmentID { get; set; }
        public Guid UserID { get; set; }
        public Guid CourseID { get; set; }
    }
}
