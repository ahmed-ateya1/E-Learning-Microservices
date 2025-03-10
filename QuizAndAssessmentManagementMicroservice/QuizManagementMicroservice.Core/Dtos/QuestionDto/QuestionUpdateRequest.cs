using QuizManagementMicroservice.Core.Helper;

namespace QuizManagementMicroservice.Core.Dtos.QuestionDto
{
    public class QuestionUpdateRequest
    {
        public Guid QuestionID { get; set; }
        public string QuestionText { get; set; }
        public QuestionTypes QuestionType { get; set; }
        public int Marks { get; set; }
    }

}
