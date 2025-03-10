namespace QuizManagementMicroservice.Core.Domain.Entities
{
    public class Answer
    {
        public Guid AnswerID { get; set; }
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
        public Guid QuestionID { get; set; }
        public virtual Question Question { get; set; }

        public virtual ICollection<StudentAnswer> StudentAnswers { get; set; } = [];
    }
}
