namespace CoursesManagmentMicroservices.Core.Domain.Entities
{
    public class Section
    {
        public Guid SectionID { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int DurationInHours { get; set; }
        public bool IsVisible { get; set; } = true;
        public int Order { get; set; }
        public Guid CourseID { get; set; }
        public virtual Course Course { get; set; }
        public virtual ICollection<Lecture> Lectures { get; set; }
    }
}
