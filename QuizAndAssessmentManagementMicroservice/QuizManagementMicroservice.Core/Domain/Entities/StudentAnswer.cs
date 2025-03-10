namespace QuizManagementMicroservice.Core.Domain.Entities
{
    public class StudentAnswer
    {
        public Guid StudentAnswerID { get; set; }  
      
        public string? AnswerText { get; set; }  

        public bool? IsCorrect { get; set; }  

        public DateTime SubmittedAt { get; set; }

        public Guid QuestionID { get; set; }
        public virtual Question Question { get; set; }

        public Guid? SelectedAnswerID { get; set; }
        public virtual Answer? SelectedAnswer { get; set; }

        public Guid StudentID { get; set; }

    }
}
