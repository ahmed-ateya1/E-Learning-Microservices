namespace ReviewManagementMicroservice.Core.Dtos.ExternalDto
{
    public class CourseDto
    {
        public Guid CourseID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PosterUrl { get; set; }

        public decimal PriceBeforeDiscount { get; set; }
        public decimal PriceAfterDiscount { get; set; }
        public decimal? Discount { get; set; }

        public string Level { get; set; }
        public decimal Duration { get; set; }
        public string Requirements { get; set; }
        public string WhatYouWillLearn { get; set; }

        public string Language { get; set; }
        public bool IsPublic { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public long TotalEnrollments { get; set; }
        public bool IsEnrolled { get; set; }

        public long TotalRating { get; set; }
        public decimal AverageRating { get; set; }

        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string PictuteUrl { get; set; }

        public Guid CategoryID { get; set; }
        public string CategoryName { get; set; }
    }
}
