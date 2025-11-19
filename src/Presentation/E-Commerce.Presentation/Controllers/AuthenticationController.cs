using E_Commerce.ServicesAbstraction;
using E_Commerce.Shared.DTOs.Authentications;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Presentation.Controllers
{
    public class AuthenticationController : ApiBaseController
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authenticationService.LoginAsync(dto);
            return HandleResult<UserDto>(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _authenticationService.RegisterAsync(dto);
            return HandleResult<UserDto>(result);
        }
    }
}
