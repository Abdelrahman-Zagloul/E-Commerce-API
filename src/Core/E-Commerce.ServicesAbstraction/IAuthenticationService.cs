using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.Authentications;

namespace E_Commerce.ServicesAbstraction
{
    public interface IAuthenticationService
    {
        Task<Result<UserDto>> Register(RegisterDto registerDto);
        Task<Result<UserDto>> Login(LoginDto loginDto);
    }
}
