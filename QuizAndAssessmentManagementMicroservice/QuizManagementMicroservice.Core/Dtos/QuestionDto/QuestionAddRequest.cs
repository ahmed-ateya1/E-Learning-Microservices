using QuizManagementMicroservice.Core.Dtos.AnswerDto;
using QuizManagementMicroservice.Core.Helper;

namespace QuizManagementMicroservice.Core.Dtos.QuestionDto
{
    public class QuestionAddRequest
    {
        public Guid QuizzID { get; set; }
        public string QuestionText { get; set; }
        public QuestionTypes QuestionType { get; set; }
        public int Marks { get; set; }
        public List<AnswerAddRequest>? Answers { get; set; } // for MCQ Questions
    }

}
