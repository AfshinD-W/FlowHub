namespace FlowHub.Modules.Identity.Infrastructure.Identity.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }

        public string Token { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime? RevokedAt { get; set; }

        public string DeviceName { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;

        public string? ReplacedByToken { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;
        public User User { get; set; } = null!;
    }
}
