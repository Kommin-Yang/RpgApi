namespace RpgApi.Models;

public enum ItemRarity
{
    Common = 1,
    Uncommon = 2,
    Rare = 3,
    Epic = 4,
    Legendary = 5,
    Mythic = 6
}

public enum ItemType
{
    Weapon = 1,
    Armor,
    Consumable,
    Miscellaneous,
    QuestItem
}

public enum EquipmentSlot
{
    Helmet = 1,
    Chest,
    Hands,
    Legs,
    Foots,
    Weapon
}

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
