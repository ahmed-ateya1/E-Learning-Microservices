using CoursesManagmentMicroservices.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoursesManagmentMicroservices.Infrastructure.Configuration
{
    public class SectionConfiguration : IEntityTypeConfiguration<Section>
    {
        public void Configure(EntityTypeBuilder<Section> builder)
        {
            builder.HasKey(e => e.SectionID);
            builder.Property(e => e.SectionID)
                .ValueGeneratedNever();

            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Description)
                .HasMaxLength(500);

            builder.Property(e => e.Order)
                .IsRequired();

            builder.Property(e => e.CourseID)
                .IsRequired();
            
            builder.HasOne(d => d.Course)
                .WithMany(x => x.Sections)
                .OnDelete(DeleteBehavior.NoAction);

            builder.ToTable("Sections");
        }
    }
}
