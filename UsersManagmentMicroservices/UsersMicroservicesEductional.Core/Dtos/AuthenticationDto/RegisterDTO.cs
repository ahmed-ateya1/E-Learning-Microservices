using Microsoft.AspNetCore.Http;
using UsersMicroservicesEductional.Core.Helper;

namespace UsersMicroservicesEductional.Core.Dtos.AuthenticationDto
{
    public class RegisterDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public IFormFile? ProfilePicture { get; set; }
        public RolesOption Role { get; set; } = RolesOption.USER;
    }
}
