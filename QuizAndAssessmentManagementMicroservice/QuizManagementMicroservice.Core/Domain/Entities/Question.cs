using QuizManagementMicroservice.Core.Helper;

namespace QuizManagementMicroservice.Core.Domain.Entities
{
    public class Question
    {
        public Guid QuestionID { get; set; }
        public string QuestionText { get; set; }
        public QuestionTypes QuestionType { get; set; }
        public int Marks { get; set; }
        public Guid QuizzID { get; set; }
        public virtual Quizz Quizz { get; set; }
        public virtual ICollection<Answer> Answers { get; set; } = [];
        public virtual ICollection<StudentAnswer> StudentAnswers { get; set; } = [];
    }
}
