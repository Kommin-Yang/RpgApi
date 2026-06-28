namespace RpgApi.Models;

public class Account
{
    public int Id { get; set; }

    public string Username { get; set; } = "Username";

    public string Email { get; set; } = "Email";

    public string PasswordHash { get; set; } = "Password";

    public DateTime DateCreation { get; set; } = DateTime.UtcNow;

    public DateTime LastLoginDate { get; set; } = DateTime.UtcNow;

    public List<Character> Characters { get; set; } = [];
}