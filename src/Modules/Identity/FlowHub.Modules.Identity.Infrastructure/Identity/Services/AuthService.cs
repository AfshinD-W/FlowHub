using BuildingBlocks.Exceptions;
using FlowHub.Modules.Identity.Application.DTO.Login;
using FlowHub.Modules.Identity.Application.DTO.Register;
using FlowHub.Modules.Identity.Application.Interfaces;
using FlowHub.Modules.Identity.Infrastructure.Database;
using FlowHub.Modules.Identity.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FlowHub.Modules.Identity.Infrastructure.Identity.Services
{
    public class AuthService : IAuthService
    {
        private const string InvalidCredentialsMessage = "User name or password is wrong!";
        private const string InvalidRefreshTokenMessage = "Invalid refresh token!";
        private const string UserNameNotFound = "UserName is null.";

        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IdentityDbContext _identityDbContext;

        public AuthService(UserManager<User> userManager, ITokenService tokenService, IdentityDbContext identityDbContext)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _identityDbContext = identityDbContext;
        }

        public async Task RegisterAsync(RegisterRequestDto requestModel)
        {
            User user = new()
            {
                UserName = requestModel.UserName,
                Email = requestModel.Email,
                PhoneNumber = requestModel.PhoneNumber,
            };

            IdentityResult result = await _userManager.CreateAsync(user, requestModel.Password);

            if (!result.Succeeded)
                throw new ValidationException(result.Errors.Select(x => x.Description));
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto requestModel)
        {
            User? user = await _userManager.FindByEmailAsync(requestModel.UserEmail)
                ?? throw new UnauthorizedException(InvalidCredentialsMessage);

            bool checkPass = await _userManager.CheckPasswordAsync(user, requestModel.Password);

            if (!checkPass)
                throw new UnauthorizedException(InvalidCredentialsMessage);

            IList<string> roles = await _userManager.GetRolesAsync(user);

            (string accessToken, DateTime expires) = _tokenService.GenerateAccessToken(user.Id, user.UserName!, roles);

            string refreshToken = _tokenService.GenerateRefreshToken();

            RefreshToken refreshTokenEntity = CreateRefreshToken(user.Id);

            await _identityDbContext.RefreshTokens.AddAsync(refreshTokenEntity);
            await _identityDbContext.SaveChangesAsync();

            return new LoginResponseDto()
            {
                Token = accessToken,
                RefreshToken = refreshToken,
                TokenExpiery = expires
            };
        }

        public async Task<LoginResponseDto> RefreshTokenAsync(string refreshToken)
        {
            RefreshToken storedToken = await _identityDbContext.RefreshTokens.Include(r => r.User).SingleOrDefaultAsync(r => r.Token == refreshToken)
                ?? throw new UnauthorizedException(InvalidRefreshTokenMessage);


            if (storedToken.RevokedAt is not null)
            {
                IEnumerable<RefreshToken> refreshTokens = await _identityDbContext.RefreshTokens.Where(r => r.UserId == storedToken.UserId).ToListAsync();

                DateTime now = DateTime.UtcNow;

                foreach (RefreshToken token in refreshTokens)
                {
                    token.RevokedAt = now;
                }

                await _identityDbContext.SaveChangesAsync();

                throw new UnauthorizedException(InvalidRefreshTokenMessage);
            }


            if (storedToken.ExpiresAt <= DateTime.UtcNow)
                throw new UnauthorizedException(InvalidRefreshTokenMessage);

            if (storedToken.User.UserName is null)
                throw new NotFoundException(UserNameNotFound);

            IEnumerable<string> userRoles = await _userManager.GetRolesAsync(storedToken.User);

            var (accessToken, expiery) = _tokenService.GenerateAccessToken(storedToken.User.Id, storedToken.User.UserName, userRoles);

            RefreshToken newRefreshToken = CreateRefreshToken(storedToken.User.Id);

            storedToken.RevokedAt = DateTime.UtcNow;
            storedToken.ReplacedByToken = newRefreshToken.Token;
            _identityDbContext.RefreshTokens.Add(newRefreshToken);

            await _identityDbContext.SaveChangesAsync();

            return new LoginResponseDto()
            {
                Token = accessToken,
                TokenExpiery = expiery,
                RefreshToken = newRefreshToken.Token,
            };
        }

        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }

        public Task ForgotPasswordAsync()
        {
            throw new NotImplementedException();
        }

        private RefreshToken CreateRefreshToken(string userId)
        {
            DateTime now = DateTime.UtcNow;

            return new()
            {
                Token = _tokenService.GenerateRefreshToken(),
                CreatedAt = now,
                ExpiresAt = now.AddDays(15),
                UserId = userId,
            };
        }
    }
}
