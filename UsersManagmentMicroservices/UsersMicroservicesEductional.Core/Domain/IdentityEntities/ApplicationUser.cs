using Microsoft.AspNetCore.Identity;
using UsersMicroservicesEductional.Core.Dtos.AuthenticationDto;

namespace UsersMicroservicesEductional.Core.Domain.IdentityEntities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? OTPCode { get; set; }
        public DateTime? OTPExpiration { get; set; }
        public string FullName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public ICollection<RefreshToken>? RefreshTokens { get; set; }

    }
}
