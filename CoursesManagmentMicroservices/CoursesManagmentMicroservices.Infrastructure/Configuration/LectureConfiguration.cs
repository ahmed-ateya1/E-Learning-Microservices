using CoursesManagmentMicroservices.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoursesManagmentMicroservices.Infrastructure.Configuration
{
    public class LectureConfiguration : IEntityTypeConfiguration<Lecture>
    {
        public void Configure(EntityTypeBuilder<Lecture> builder)
        {
            builder.HasKey(e => e.LectureID);

            builder.Property(e => e.LectureID)
                .ValueGeneratedNever();
            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Description)
                .HasMaxLength(500);

            builder.Property(e => e.VideoUrl)
                .HasMaxLength(500);

            builder.Property(e => e.ResourceUrl)
                .HasMaxLength(500);

            builder.Property(e => e.FileURL)
                .HasMaxLength(500);

            builder.Property(e => e.IsPreview)
                .IsRequired();

            builder.Property(e => e.DurationInMinutes)
                .IsRequired();

            builder.Property(e => e.Order)
                .IsRequired();

            builder.Property(e => e.CreatedAt)
                .IsRequired();

            builder.Property(e => e.UpdatedAt)
                .IsRequired();

            builder.Property(e => e.SectionID)
                .IsRequired();

            builder.HasOne(d => d.Section)
                .WithMany(x => x.Lectures)
                .OnDelete(DeleteBehavior.NoAction);

            builder.ToTable("Lectures");
        }
    }
}
