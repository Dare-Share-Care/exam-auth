using Auth.Web.Interfaces.DomainServices;
using Auth.Web.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterCustomerAsync(RegisterDto dto)
    {
        await _authService.RegisterCustomerAsync(dto);
        return Ok();
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginDto dto)
    {
        var token = await _authService.LoginAsync(dto);
        return Ok(token);
    }
    
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePasswordAsync(ChangePasswordDto dto)
    {
        await _authService.ChangePasswordAsync(dto);
        return Ok();
    }
}