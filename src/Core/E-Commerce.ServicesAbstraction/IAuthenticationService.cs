using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.Authentications;

namespace E_Commerce.ServicesAbstraction
{
    public interface IAuthenticationService
    {
        Task<Result<UserDto>> RegisterAsync(RegisterDto dto);
        Task<Result<UserDto>> LoginAsync(LoginDto dto);
    }
}
