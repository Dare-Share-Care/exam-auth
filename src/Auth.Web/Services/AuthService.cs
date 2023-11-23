using Auth.Web.Interfaces.DomainServices;
using Auth.Web.Models.Dto;

namespace Auth.Web.Services;

public class AuthService : IAuthService
{
    public Task<string> LoginAsync(LoginDto dto)
    {
        throw new NotImplementedException();
    }

    public Task RegisterCustomerAsync(RegisterDto dto)
    {
        throw new NotImplementedException();
    }

    public Task ChangePasswordAsync(ChangePasswordDto dto)
    {
        throw new NotImplementedException();
    }
}