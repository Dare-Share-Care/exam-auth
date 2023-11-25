namespace Auth.Web.Models.Dto;

public class UserDto
{
    public long Id { get; set; }
    public string Email { get; set; } = null!;
    public long RoleId { get; set; }
}