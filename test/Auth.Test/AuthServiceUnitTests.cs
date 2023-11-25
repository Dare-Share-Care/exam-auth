using Auth.Web.Entities;
using Auth.Web.Interfaces.DomainServices;
using Auth.Web.Interfaces.Repositories;
using Auth.Web.Services;
using Moq;
using MTOGO.Web.Models.Dto.Auth;

namespace Auth.Test;

public class AuthServiceUnitTests
{
    private readonly IAuthService _authService;
    private readonly Mock<IRepository<User>> _userRepositoryMock = new();

    public AuthServiceUnitTests()
    {
        _authService = new AuthService(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task RegisterCustomerAsync_ShouldRegisterUserWithCustomerRole()
    {
        //Arrange
        var dto = new RegisterDto
        {
            Email = "test@example.com",
            Password = "password"
        };
        
        //Setup the mock
        var user = new User
        {
            Id = 1,
            Email = dto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            RoleId = 1 //Customer
        };
        
        //Assure that the repository returns an empty list rather than null
        _userRepositoryMock.Setup(x => x.ListAsync(new CancellationToken()))
            .ReturnsAsync(new List<User>());
        
        //Act
        var result = await _authService.RegisterCustomerAsync(dto);
        
        //Assert
        Assert.Equal(1, result.RoleId); //Is customer role
        Assert.Equal(dto.Email, result.Email); //Email is correct
    }
}