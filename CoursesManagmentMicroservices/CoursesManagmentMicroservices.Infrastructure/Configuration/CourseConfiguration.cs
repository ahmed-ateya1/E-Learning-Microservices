using CoursesManagmentMicroservices.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoursesManagmentMicroservices.Infrastructure.Configuration
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(e => e.CourseID);

            builder.Property(x=>x.CourseID)
                .ValueGeneratedNever();

            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(e => e.PosterUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(e => e.Price)
                .HasColumnType("decimal(18,2)");

            builder.Property(e => e.Discount)
                .HasColumnType("decimal(18,2)");

            builder.Property(e => e.Level)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.Duration)
                .IsRequired();

            builder.Property(e => e.Requirements)
                .IsRequired()
                .HasMaxLength(500);
            builder.Property(e => e.WhatYouWillLearn)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(e => e.TotalEnrollments)
                .IsRequired();

            builder.Property(e => e.TotalRating)
                .IsRequired();
            builder.Property(e => e.AverageRating)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(e => e.Language)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.IsPublic)
                .IsRequired();

            builder.Property(e => e.IsPublished)
                .IsRequired();

            builder.Property(e => e.CreatedAt)
                .IsRequired();

            builder.Property(e => e.UpdatedAt)
                .IsRequired();

            builder.Property(e => e.UserID)
                .IsRequired();
            
            builder.HasOne(e => e.Category)
                .WithMany(x=>x.Courses)
                .HasForeignKey(e => e.CategoryID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.ToTable("Courses");
        }
    }
}
