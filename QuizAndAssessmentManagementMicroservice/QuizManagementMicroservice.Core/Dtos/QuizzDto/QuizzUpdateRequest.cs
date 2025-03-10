namespace QuizManagementMicroservice.Core.Dtos.QuizzDto
{
    public class QuizzUpdateRequest
    {
        public Guid QuizzID { get; set; }
        public string Title { get; set; }
        public int TotalMarks { get; set; }
        public int PassMarks { get; set; }
        public Guid LectureID { get; set; }
        public Guid InstructorID { get; set; }
    }
}
