using CoursesManagmentMicroservices.Core.Helper;
using Microsoft.AspNetCore.Http;

namespace CoursesManagmentMicroservices.Core.Dtos.CourseDto
{
    public class CourseAddRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Poster { get; set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }
        public CourseLevel Level { get; set; }
        public decimal Duration { get; set; }
        public string Requirements { get; set; }
        public string WhatYouWillLearn { get; set; }
        public string Language { get; set; }
        public bool IsPublic { get; set; } = true;
        public bool IsPublished { get; set; } = true;
        public Guid CategoryID { get; set; }
        public Guid UserID { get; set; }
    }
}
