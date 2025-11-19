using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Shared.DTOs.Authentications
{
    public record RegisterDto(string DisplayName, string UserName, [EmailAddress] string Email, string Password, [Phone] string PhoneNumber);
}
