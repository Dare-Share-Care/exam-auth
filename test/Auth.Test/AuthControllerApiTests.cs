using System.Net;
using System.Text;
using System.Text.Json;
using Auth.Test.CustomFactories;
using Auth.Web.Data;
using Auth.Web.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Test;

public class AuthControllerApiTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public AuthControllerApiTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
   
    [Fact]
    public async Task RegisterEndpoint_ReturnsSuccessStatusCode()
    {
        // Arrange
        var client = _factory.CreateClient();
        var user = new RegisterDto { Email = "test@example.com", Password = "testpassword" };
        var json = JsonSerializer.Serialize(user);
        var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync("/api/Auth/register", stringContent);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        // Check the in-memory database
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<UserContext>();
        var registeredUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");
        Assert.NotNull(registeredUser);
        Assert.Equal(user.Email, registeredUser.Email);
    }
}