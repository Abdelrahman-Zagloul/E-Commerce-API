using E_Commerce.Domain.Entities.IdentityModule;
using E_Commerce.ServicesAbstraction;
using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.Authentications;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_Commerce.Services.Features
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthenticationService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<Result<UserDto>> GetCurrentUserAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Result<UserDto>.Fail(Error.NotFound("User Not Found", "User not found."));

            var userDto = new UserDto(user.Email!, user.DisplayName, await GenerateJwtToken(user));
            return Result<UserDto>.Ok(userDto);
        }

        public async Task<Result<AddressDto>> GetUserAddressAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Result<AddressDto>.Fail(Error.NotFound("User Not Found", "User not found."));

            var userAddressDto = await _userManager.Users.Select(
                u => new AddressDto
                {
                    Id = u.Address.Id,
                    Street = u.Address.Street,
                    City = u.Address.City,
                    Country = u.Address.Country,
                    FirstName = u.Address.FirstName,
                    LastName = u.Address.LastName,
                    UserId = u.Id
                }).FirstOrDefaultAsync(x => x.UserId == user.Id);

            return Result<AddressDto>.Ok(userAddressDto!);
        }

        public async Task<bool> IsEmailExist(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return false;
            return true;
        }

        public async Task<Result<UserDto>> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return Result<UserDto>.Fail(Error.Unauthorized("InvalidCredentials", "Email or password is incorrect."));

            var userDto = new UserDto(user.Email!, user.DisplayName, await GenerateJwtToken(user));
            return Result<UserDto>.Ok(userDto);
        }
        public async Task<Result<UserDto>> RegisterAsync(RegisterDto dto)
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

            return Result<UserDto>.Ok(new UserDto(user.Email!, user.DisplayName, await GenerateJwtToken(user)));
        }

        public async Task<Result> UpdateUserAddressAsync(string email, AddressDto addressDto)
        {
            var user = await _userManager.Users.Include(x => x.Address).FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
                return Result.Fail(Error.NotFound("User Not Found", "User not found."));

            user.Address.Street = addressDto.Street;
            user.Address.City = addressDto.City;
            user.Address.Country = addressDto.Country;
            user.Address.FirstName = addressDto.FirstName;
            user.Address.LastName = addressDto.LastName;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList();
                return Result.Fail(errors);
            }

            return Result.Ok();
        }

        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var UserRole = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName!)
            };
            claims.AddRange(UserRole.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
