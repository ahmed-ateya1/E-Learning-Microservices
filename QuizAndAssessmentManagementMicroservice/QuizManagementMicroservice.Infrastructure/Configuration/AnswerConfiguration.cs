using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizManagementMicroservice.Core.Domain.Entities;

namespace QuizManagementMicroservice.Infrastructure.Configuration
{
    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.HasKey(a => a.AnswerID);

            builder.Property(a => a.AnswerID)
                .ValueGeneratedNever();

            builder.Property(a => a.AnswerText)
                .IsRequired();

            builder.Property(a => a.IsCorrect)
                .IsRequired();

            builder.Property(a => a.QuestionID)
                .IsRequired();

            builder.HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Answers");
        }
    }
}
