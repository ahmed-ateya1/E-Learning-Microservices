using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizManagementMicroservice.Core.Domain.Entities;

namespace QuizManagementMicroservice.Infrastructure.Configuration
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasKey(q => q.QuestionID);

            builder.Property(q => q.QuestionID)
                .ValueGeneratedNever();

            builder.Property(q => q.QuestionText)
                .IsRequired();

            builder.Property(q => q.QuestionType)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(q => q.Marks)
                .IsRequired();

            builder.Property(q => q.QuizzID)
                .IsRequired();

            builder.HasOne(q => q.Quizz)
                .WithMany(q => q.Questions)
                .HasForeignKey(q => q.QuizzID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Questions");

        }
    }
}
