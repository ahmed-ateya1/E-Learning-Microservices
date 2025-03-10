using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizManagementMicroservice.Core.Domain.Entities;

namespace QuizManagementMicroservice.Infrastructure.Configuration
{
    public class StudentAnswerConfiguration : IEntityTypeConfiguration<StudentAnswer>
    {
        public void Configure(EntityTypeBuilder<StudentAnswer> builder)
        {
            builder.HasKey(e => e.StudentAnswerID);

            builder.Property(e => e.StudentAnswerID)
                .ValueGeneratedNever();

            builder.Property(e=>e.SubmittedAt)
                .IsRequired();

            builder.Property(e=>e.StudentID)
                .IsRequired();

            builder.HasOne(e => e.Question)
                .WithMany(e=>e.StudentAnswers)
                .HasForeignKey(e => e.QuestionID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.SelectedAnswer)
                .WithMany(e=>e.StudentAnswers)
                .HasForeignKey(e => e.SelectedAnswerID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.ToTable("StudentAnswer");
        }
    }
}
