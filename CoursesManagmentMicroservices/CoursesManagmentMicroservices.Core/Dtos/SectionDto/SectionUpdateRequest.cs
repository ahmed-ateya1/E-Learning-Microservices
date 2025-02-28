namespace CoursesManagmentMicroservices.Core.Dtos.SectionDto
{
    public class SectionUpdateRequest
    {
        public Guid SectionID { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool IsVisible { get; set; }
        public int Order { get; set; }
        public Guid CourseID { get; set; }
    }
}
