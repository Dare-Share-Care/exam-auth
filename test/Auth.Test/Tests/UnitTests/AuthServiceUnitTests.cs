using Auth.Web.Entities;
using Auth.Web.Interfaces.DomainServices;
using Auth.Web.Interfaces.Repositories;
using Auth.Web.Models.Dto;
using Auth.Web.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Auth.Test.Tests.UnitTests;

public class AuthServiceUnitTests
{
    private readonly IAuthService _authService;
    private readonly Mock<IRepository<User>> _userRepositoryMock = new();
    private readonly Mock<IConfiguration> _configurationMock = new();

    public AuthServiceUnitTests()
    {
        _authService = new AuthService(_userRepositoryMock.Object, _configurationMock.Object);
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
    
    [Fact]
    public async Task RegisterCustomerAsync_ShouldThrowExceptionIfEmailAlreadyExists()
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
            
        //Assure that the repository returns a list with one user
        _userRepositoryMock.Setup(x => x.ListAsync(new CancellationToken()))
            .ReturnsAsync(new List<User> {user});

        //Act + Assert
        await Assert.ThrowsAsync<Exception>(() => _authService.RegisterCustomerAsync(dto));
    }
}
