using Auth.Web.Models.Dto;
using Auth.Web.Models.Dto;

namespace Auth.Web.Interfaces.DomainServices;

public interface IAuthService
{
    Task<string> LoginAsync(LoginDto dto);
    Task<UserDto> RegisterCustomerAsync(RegisterDto dto);
    Task<UserDto> ChangePasswordAsync(ChangePasswordDto dto);
}