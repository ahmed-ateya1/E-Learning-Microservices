namespace CoursesManagmentMicroservices.Core.Dtos.LectureDto
{
    public class LectureResponse
    {
        public Guid LectureID { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? VideoUrl { get; set; }
        public string? ResourceUrl { get; set; }
        public string? FileURL { get; set; }
        public bool IsPreview { get; set; }
        public int DurationInMinutes { get; set; }
        public int Order { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid SectionID { get; set; }
        public string SectionTitle { get; set; }

    }
}
