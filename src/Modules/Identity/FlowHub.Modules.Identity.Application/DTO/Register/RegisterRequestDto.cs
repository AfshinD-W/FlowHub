namespace FlowHub.Modules.Identity.Application.DTO.Register
{
    public class RegisterRequestDto
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
