namespace FlowHub.Modules.Identity.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(
            string userId,
            string userName,
            IEnumerable<string> roles
            );

        string GenerateRefreshToken();
    }
}
