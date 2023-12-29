using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Web.Entities;
using Auth.Web.Interfaces.DomainServices;
using Auth.Web.Interfaces.Repositories;
using Auth.Web.Models.Dto;
using Auth.Web.Specifications;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Web.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IRepository<User> _userRepository;

    public AuthService(IRepository<User> userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<string> LoginAsync(LoginDto dto)
    {
        var user = await _userRepository.FirstOrDefaultAsync(new GetUserByEmailWithRoleSpec(dto.Email));

        // validate
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            throw new Exception("Username or password is incorrect");

        var token = CreateToken(user);

        return token;
    }

    public async Task<UserDto> RegisterCustomerAsync(RegisterDto dto)
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
        
        //Map user to userDto
        var userDto = new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            RoleId = user.RoleId
        };
        
        //Return userDto
        return userDto;
    }

    public async Task<UserDto> ChangePasswordAsync(ChangePasswordDto dto)
    {
        //Get user from database
        var user = await _userRepository.FirstOrDefaultAsync(new GetUserByEmailWithRoleSpec(dto.Email));
        
        if (user == null)
            throw new Exception("User not found");

        //Validate old password
        if (!BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.Password))
            throw new Exception("Old password is incorrect");
        
        //Hash new password
        user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        await _userRepository.SaveChangesAsync();
        
        //Map user to userDto
        var userDto = new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            RoleId = user.RoleId
        };
        
        //Return userDto
        return userDto;
    }
    
    private string CreateToken(User user)
    {
        //Create claims
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role.RoleType.ToString())
        };

        //Get JWT Key from configuration
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!));

        //Create token
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: cred
        );
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }
}