using System.ComponentModel.DataAnnotations;

namespace Auth.Web.Models.Dto;

public class ChangePasswordDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Old password is required")]
    public string OldPassword { get; set; } = null!;

    [Required(ErrorMessage = "New password is required")]
    public string NewPassword { get; set; } = null!;
}