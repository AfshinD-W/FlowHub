namespace FlowHub.Modules.Identity.Application.DTO.Login
{
    public class LoginRequestDto
    {
        public string UserEmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
