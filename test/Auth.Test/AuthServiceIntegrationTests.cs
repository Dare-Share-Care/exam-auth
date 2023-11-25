using Auth.Web.Data;
using Auth.Web.Entities;
using Auth.Web.Services;
using Auth.Web.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Test;

public class AuthServiceIntegrationTests
{
    [Fact]
    public async Task RegisterCustomerAsync_ShouldSaveUserToDatabase()
    {
        // Arrange
        var serviceProvider = new ServiceCollection()
            .AddDbContext<UserContext>(options => options.UseInMemoryDatabase(databaseName: "TestDB"))
            .BuildServiceProvider();

        var dbContext = serviceProvider.GetRequiredService<UserContext>();
        await dbContext.Database.EnsureCreatedAsync();

        var userRepository = new EfRepository<User>(dbContext);
        var authService = new AuthService(userRepository);

        var dto = new RegisterDto
        {
            Email = "test@example.com",
            Password = "password"
        };

        // Act
        var result = await authService.RegisterCustomerAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Email, result.Email);

        // Check if the user is saved in the in-memory database
        var savedUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        Assert.NotNull(savedUser);
        Assert.Equal(dto.Email, savedUser.Email);
    }
}