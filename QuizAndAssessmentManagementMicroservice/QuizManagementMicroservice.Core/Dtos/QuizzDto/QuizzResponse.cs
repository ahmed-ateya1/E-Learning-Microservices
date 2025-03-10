using QuizManagementMicroservice.Core.Dtos.QuestionDto;

namespace QuizManagementMicroservice.Core.Dtos.QuizzDto
{
    public class QuizzResponse
    {
        public Guid QuizzID { get; set; }
        public string Title { get; set; }
        public int TotalMarks { get; set; }
        public int PassMarks { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid LectureID { get; set; }
        public Guid InstructorID { get; set; }
        public List<QuestionResponse> Questions { get; set; } = new();
    }
}
