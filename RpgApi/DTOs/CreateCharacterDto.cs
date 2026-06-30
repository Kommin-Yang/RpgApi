using System.ComponentModel.DataAnnotations;

namespace RpgApi.DTOs;

public class CreateCharacterDto
{
    [Required]
    [MaxLength(24, ErrorMessage = "Name too long (24 char max).")]
    public string Name { get; set; } = string.Empty;
}
