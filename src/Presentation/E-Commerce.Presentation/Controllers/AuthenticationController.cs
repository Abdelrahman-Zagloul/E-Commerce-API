using E_Commerce.ServicesAbstraction;
using E_Commerce.Shared.DTOs.Authentications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [Authorize]
        [HttpGet("current-user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await _authenticationService.GetCurrentUserAsync(email!);
            return HandleResult<UserDto>(result);
        }
        [HttpGet("email-exist")]
        public async Task<IActionResult> IsEmailExist(string email)
        {
            var result = await _authenticationService.IsEmailExist(email!);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("user-address")]
        public async Task<IActionResult> GetCurrentUserAddress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await _authenticationService.GetUserAddressAsync(email!);
            return HandleResult<AddressDto>(result);
        }

        [HttpPut("update-user-address")]
        [Authorize]
        public async Task<IActionResult> UpdateCurrentUserAddress(AddressDto dto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await _authenticationService.UpdateUserAddressAsync(email!, dto);
            return HandleResult(result);
        }


    }
}
