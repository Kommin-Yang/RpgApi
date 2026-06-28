namespace RpgApi.Models;

public enum StatType
{
    Strength = 1,
    Agility = 2,
    Vitality = 3,
    Intelligence = 4,

    Defense = 5,

    Health = 6,
    Mana = 7,

    CriticalRate = 8,
    CriticalDamage = 9,

    AttackSpeed = 10,
    LifeSteal = 11
}

public class Character
{
    public int Id { get; set; }

    public required string Name { get; set; }

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