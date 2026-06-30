using System.ComponentModel.DataAnnotations;

namespace RpgApi.DTOs;

public class RegisterAccountDto
{
    [Required]
    [MaxLength(50, ErrorMessage = "Name too long (50 char max).")]
    public string Username { get; set; } = "Username";

    [Required]
    [EmailAddress(ErrorMessage = "Email not valid.")]
    [MaxLength(150)]
    public string Email { get; set; } = "Email";

    [Required]
    public string Password { get; set; } = "Password";
}