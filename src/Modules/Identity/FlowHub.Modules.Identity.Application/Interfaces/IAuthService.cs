using FlowHub.Modules.Identity.Application.DTO.Login;
using FlowHub.Modules.Identity.Application.DTO.Register;

namespace FlowHub.Modules.Identity.Application.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterRequestDto requestModel);
        Task<LoginResponseDto> LoginAsync(LoginRequestDto requestModel);
        Task RefreshTokenAsync();
        Task LogoutAsync();
        Task ForgotPasswordAsync();
    }
}
