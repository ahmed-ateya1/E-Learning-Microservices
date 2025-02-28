namespace CoursesManagmentMicroservices.Core.Dtos.SectionDto
{
    public class SectionResponse
    {
        public Guid SectionID { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int DurationInHours { get; set; }
        public bool IsVisible { get; set; }
        public int Order { get; set; }
        public Guid CourseID { get; set; }
        public string CourseTitle { get; set; }
    }
}
