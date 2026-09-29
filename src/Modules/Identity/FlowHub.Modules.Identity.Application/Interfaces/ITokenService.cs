namespace FlowHub.Modules.Identity.Application.Interfaces
{
    public interface ITokenService
    {
        (string Token, DateTime ExpiresAt) GenerateAccessToken(
            string userId,
            string userName,
            IEnumerable<string> roles
            );

        string GenerateRefreshToken();
    }
}
