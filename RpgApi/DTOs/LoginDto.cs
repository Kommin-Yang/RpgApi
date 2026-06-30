using System.ComponentModel.DataAnnotations;

namespace RpgApi.DTOs;

public class LoginDto
{
    [Required]
    [EmailAddress(ErrorMessage = "Email not valid.")]
    [MaxLength(150)]
    public string Email { get; set; } = "Email";

    [Required]
    public string Password { get; set; } = "Password";
}