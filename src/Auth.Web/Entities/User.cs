namespace Auth.Web.Entities;

public class User : BaseEntity
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public long RoleId { get; set; }
    public Role Role { get; set; } = null!;
}