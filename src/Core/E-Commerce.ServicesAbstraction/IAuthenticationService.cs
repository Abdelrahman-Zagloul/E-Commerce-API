using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.Authentications;

namespace E_Commerce.ServicesAbstraction
{
    public interface IAuthenticationService
    {
        Task<Result<UserDto>> RegisterAsync(RegisterDto dto);
        Task<Result<UserDto>> LoginAsync(LoginDto dto);
        Task<Result<UserDto>> GetCurrentUserAsync(string email);
        Task<bool> IsEmailExist(string email);
        Task<Result<AddressDto>> GetUserAddressAsync(string email);
        Task<Result> UpdateUserAddressAsync(string email, AddressDto addressDto);
    }
}




