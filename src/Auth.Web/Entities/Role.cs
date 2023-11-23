using Auth.Web.Models;

namespace Auth.Web.Entities;

public class Role : BaseEntity
{
    public RoleTypes RoleType { get; set; }    
    public List<User> Users { get; set; } = null!;
}