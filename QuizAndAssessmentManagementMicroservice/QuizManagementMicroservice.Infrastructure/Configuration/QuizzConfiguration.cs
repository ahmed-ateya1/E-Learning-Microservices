using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizManagementMicroservice.Core.Domain.Entities;

namespace QuizManagementMicroservice.Infrastructure.Configuration
{
    public class QuizzConfiguration : IEntityTypeConfiguration<Quizz>
    {
        public void Configure(EntityTypeBuilder<Quizz> builder)
        {
            builder.HasKey(q => q.QuizzID);

            builder.Property(q => q.QuizzID)
                .ValueGeneratedNever();
            
            builder.Property(q=>q.TotalMarks)
                .IsRequired();

            builder.Property(q=>q.PassMarks)
                .IsRequired();

            builder.Property(q => q.CreatedAt)
                .IsRequired();

            builder.Property(q => q.LectureID)
                .IsRequired();

            builder.ToTable("Quizzes");
        }
    }
}
