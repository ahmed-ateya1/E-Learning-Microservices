using Microsoft.AspNetCore.Http;

namespace CoursesManagmentMicroservices.Core.Dtos.LectureDto
{
    public class LectureUpdateRequest
    {
        public Guid LectureID { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public IFormFile? Video { get; set; }
        public string? ResourceUrl { get; set; }
        public IFormFile? File { get; set; }
        public bool IsPreview { get; set; }
        public int DurationInMinutes { get; set; }
        public int Order { get; set; }
        public Guid SectionID { get; set; }
    }
}
