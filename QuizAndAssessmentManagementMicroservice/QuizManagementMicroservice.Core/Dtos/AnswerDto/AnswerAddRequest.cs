namespace QuizManagementMicroservice.Core.Dtos.AnswerDto
{
    public class AnswerAddRequest
    {
        public Guid QuestionID { get; set; }
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
    }
}
