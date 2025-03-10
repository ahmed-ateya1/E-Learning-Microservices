namespace QuizManagementMicroservice.Core.Dtos.ExternalDto
{
    public class UserDto
    {
        public Guid UserID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
}
