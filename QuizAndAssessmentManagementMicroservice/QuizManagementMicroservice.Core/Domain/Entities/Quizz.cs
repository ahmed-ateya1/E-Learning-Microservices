namespace QuizManagementMicroservice.Core.Domain.Entities
{
    public class Quizz
    {
        public Guid QuizzID { get; set; }
        public string Title { get; set; }
        public int TotalMarks { get; set; }
        public int PassMarks { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid LectureID { get; set; }
        public Guid InstructorID { get; set; }
        public virtual ICollection<Question> Questions { get; set; } = [];
        public virtual ICollection<StudentAnswer> StudentAnswers { get; set; } = [];
    }
}
