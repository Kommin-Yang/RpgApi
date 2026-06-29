namespace RpgApi.Models;

public enum ItemRarity
{
    Common = 1,
    Uncommon,
    Rare,
    Epic,
    Legendary,
    Mythic
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
    Agility,
    Vitality,
    Intelligence,
    Spirit,

    AttackPower,
    MagicPower,

    Defense,
    MagicResistancce,

    Health,
    Mana,

    CriticalRate,
    CriticalDamage,

    AttackSpeed,
    LifeSteal
}