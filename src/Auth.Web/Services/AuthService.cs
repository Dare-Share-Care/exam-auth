using Auth.Web.Entities;
using Auth.Web.Interfaces.DomainServices;
using Auth.Web.Interfaces.Repositories;
using Auth.Web.Models.Dto;
using MTOGO.Web.Models.Dto.Auth;

namespace Auth.Web.Services;

public class AuthService : IAuthService
{
    private readonly IRepository<User> _userRepository;

    public AuthService(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<string> LoginAsync(LoginDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task RegisterCustomerAsync(RegisterDto dto)
    {
        //Validate that email is not already in use
        var users = await _userRepository.ListAsync();

        if (users.Any(x => x.Email == dto.Email))
            throw new Exception("Email already exists");

        //Map dto to user and hash password
        var user = new User
        {
            Email = dto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            RoleId = 1 //Customer
        };

        //Add user to database
        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();
    }

    public Task ChangePasswordAsync(ChangePasswordDto dto)
    {
        throw new NotImplementedException();
    }
}