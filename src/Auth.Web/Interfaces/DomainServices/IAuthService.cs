using Auth.Web.Models.Dto;

namespace Auth.Web.Interfaces.DomainServices;

public interface IAuthService
{
    Task<string> LoginAsync(LoginDto dto);
    Task RegisterCustomerAsync(RegisterDto dto);
    Task ChangePasswordAsync(ChangePasswordDto dto);
}