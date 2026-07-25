using BuildingBlocks.Exceptions;
using FlowHub.Modules.Identity.Application.DTO.Register;
using FlowHub.Modules.Identity.Application.Interfaces;
using FlowHub.Modules.Identity.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace FlowHub.Modules.Identity.Infrastructure.Identity.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;

        public AuthService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task RegisterAsync(RegisterRequestDto dto)
        {
            User user = new()
            {
                UserName = dto.UserName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
            };

            IdentityResult result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                throw new ValidationException(result.Errors.Select(x => x.Description));
        }

        public Task LoginAsync()
        {
            throw new NotImplementedException();
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
