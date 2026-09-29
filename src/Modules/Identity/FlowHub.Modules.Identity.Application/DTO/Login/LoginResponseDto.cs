namespace FlowHub.Modules.Identity.Application.DTO.Login
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenExpiery { get; set; }
    }
}
