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

    public List<CharacterStatistic> Stats { get; set; } = [];

    public CharacterInventory? Inventory { get; set; }

    public List<CharacterEquipment> Equipements { get; set; } = [];
}