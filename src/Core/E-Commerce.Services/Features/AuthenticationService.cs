using E_Commerce.Domain.Entities.IdentityModule;
using E_Commerce.ServicesAbstraction;
using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.Authentications;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Services.Features
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthenticationService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<UserDto>> Login(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return Result<UserDto>.Fail(Error.Unauthorized("InvalidCredentials", "Email or password is incorrect."));

            var userDto = new UserDto(user.Email!, user.DisplayName, "Token");
            return Result<UserDto>.Ok(userDto);
        }
        public async Task<Result<UserDto>> Register(RegisterDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.UserName,
                Email = dto.Email,
                DisplayName = dto.DisplayName,
                PhoneNumber = dto.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList();
                return Result<UserDto>.Fail(errors);
            }

            return Result<UserDto>.Ok(new UserDto(user.Email!, user.DisplayName, "Token"));
        }

    }
}
