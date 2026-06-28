namespace RpgApi.Models;

public class Character
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Level { get; set; } = 1;

    public int XP { get; set; } = 0;

    public int XPToNextLevel { get; set; } = 100;

    public int PointsToAllocate { get; set; } = 0;

    // Foreign key to get Account
    public int AccountId { get; set; }

    public List<Statistics> Stats { get; set; } = [];

    public Inventory? Inventory { get; set; }

    public List<Equipement> Equipements { get; set; } = [];
}