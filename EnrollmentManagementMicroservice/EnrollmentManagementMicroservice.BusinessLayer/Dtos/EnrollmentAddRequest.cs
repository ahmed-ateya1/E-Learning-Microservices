namespace EnrollmentManagementMicroservice.BusinessLayer.Dtos
{
    public class EnrollmentAddRequest
    {
        public Guid UserID { get; set; }
        public Guid CourseID { get; set; }
    }
}
