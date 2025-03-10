namespace QuizManagementMicroservice.Core.Dtos.QuizzDto
{
    public class QuizzAddRequest
    {
        public string Title { get; set; }
        public int TotalMarks { get; set; }
        public int PassMarks { get; set; }
        public Guid LectureID { get; set; }
        public Guid InstructorID { get; set; }
    }
}
