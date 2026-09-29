using BuildingBlocks.Exceptions;
using FlowHub.Modules.Identity.Application.DTO.Login;
using FlowHub.Modules.Identity.Application.DTO.Register;
using FlowHub.Modules.Identity.Application.Interfaces;
using FlowHub.Modules.Identity.Infrastructure.Database;
using FlowHub.Modules.Identity.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace FlowHub.Modules.Identity.Infrastructure.Identity.Services
{
    public class AuthService : IAuthService
    {
        private const string InvalidCredentialsMessage = "User name or password is wrong!";

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

            RefreshToken refreshTokenEntity = new()
            {
                Token = refreshToken,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(15),
                UserId = user.Id,
            };

            await _identityDbContext.RefreshTokens.AddAsync(refreshTokenEntity);
            await _identityDbContext.SaveChangesAsync();

            return new LoginResponseDto()
            {
                Token = accessToken,
                RefreshToken = refreshToken,
                TokenExpiery = expires
            };
        }

        public Task RefreshTokenAsync()
        {
            throw new NotImplementedException();
        }

        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }

        public Task ForgotPasswordAsync()
        {
            throw new NotImplementedException();
        }
    }
}
