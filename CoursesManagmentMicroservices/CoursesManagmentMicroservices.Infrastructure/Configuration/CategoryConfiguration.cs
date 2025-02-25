using CoursesManagmentMicroservices.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoursesManagmentMicroservices.Infrastructure.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x=>x.CategoryID);

            builder.Property(x => x.CategoryID)
                .ValueGeneratedNever();

            builder.Property(x => x.CategoryName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.BaseCategoryID)
                .IsRequired(false);

            builder.HasOne(x => x.BaseCategory)
                .WithMany(x => x.SubCategories)
                .HasForeignKey(x => x.BaseCategoryID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.ToTable("Categories");

        }
    }
}
