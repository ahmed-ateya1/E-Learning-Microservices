﻿namespace CoursesManagmentMicroservices.Core.Domain.Entities
{
    public class Course
    {
        public Guid CourseID { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Description { get; set; }
        public string PosterUrl { get; set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }
        public string Level { get; set; }
        public decimal Duration { get; set; }
        public string Requirements { get; set; }
        public string WhatYouWillLearn { get; set; }
        public long TotalEnrollments { get; set; }
        public long TotalRating { get; set; }
        public decimal AverageRating { get; set; }
        public string Language { get; set; }
        public bool IsPublic { get; set; } = false;
        public bool IsPublished { get; set; } = false;

        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt { get; set; }
        public Guid UserID { get; set; }

        public Guid CategoryID { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Section> Sections { get; set; }

        public decimal GetPriceAfterDiscount()
        {
            return Discount.HasValue ? Price - (Price*(Discount.Value/100)): Price;
        }
    }
}
