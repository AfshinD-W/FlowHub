using FlowHub.Modules.Identity.Application.DTO.Register;

namespace FlowHub.Modules.Identity.Application.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterRequestDto dto);
        Task LoginAsync();
        Task RefreshTokenAsync();
        Task LogoutAsync();
        Task ForgotPasswordAsync();
    }
}
