using UsersMicroservicesEductional.Core.Dtos.AuthenticationDto;

namespace UsersMicroservicesEductional.Core.ServiceContract
{
    public interface IAuthenticationServices
    {
        Task<AuthenticationResponse> RegisterClientAsync(RegisterDTO clientRegisterDTO);
        Task<AuthenticationResponse> LoginAsync(LoginDTO loginDTO);
        Task<AuthenticationResponse> RefreshTokenAsync(string token);
        Task<bool> ForgotPassword(ForgotPasswordDTO forgotPasswordDTO);
        Task<bool> RevokeTokenAsync(string token);
        Task<string> AddRoleToUserAsync(AddRoleDTO model);
    }
}
