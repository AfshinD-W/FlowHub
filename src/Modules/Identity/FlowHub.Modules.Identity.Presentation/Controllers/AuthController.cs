using BuildingBlocks.Responses;
using FlowHub.Modules.Identity.Application.DTO.Login;
using FlowHub.Modules.Identity.Application.DTO.Register;
using FlowHub.Modules.Identity.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlowHub.Modules.Identity.Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private const string SuccessRegister = "User registered success fully.";
        private const string SuccessLogin = "User login was success fully.";

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequestDto requestModel)
        {
            await _authService.RegisterAsync(requestModel);

            return Ok(new ApiResponse<object>()
            {
                Success = true,
                Message = SuccessRegister
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginRequestDto requestModel)
        {
            LoginResponseDto response = await _authService.LoginAsync(requestModel);

            return Ok(new ApiResponse<object>()
            {
                Success = true,
                Message = SuccessLogin,
                Data = response
            });
        }
    }
}
