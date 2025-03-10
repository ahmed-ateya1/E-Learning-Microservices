using QuizManagementMicroservice.Core.Dtos.AnswerDto;
using QuizManagementMicroservice.Core.Helper;

namespace QuizManagementMicroservice.Core.Dtos.QuestionDto
{
    public class QuestionResponse
    {
        public Guid QuestionID { get; set; }
        public string QuestionText { get; set; }
        public QuestionTypes QuestionType { get; set; }
        public int Marks { get; set; }
        public List<AnswerResponse>? Answers { get; set; }
    }

}
