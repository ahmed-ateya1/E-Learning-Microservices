namespace QuizManagementMicroservice.Core.Dtos.AnswerDto
{
    public class AnswerUpdateRequest
    {
        public Guid AnswerID { get; set; }
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
    }
}
