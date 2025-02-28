namespace CoursesManagmentMicroservices.Core.Dtos.SectionDto
{
    public class SectionAddRequest
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool IsVisible { get; set; } = true;
        public int Order { get; set; }
        public Guid CourseID { get; set; }
    }
}
