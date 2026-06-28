namespace RpgApi.Models;

public enum ItemType
{
    Weapon = 1,
    Armor,
    Consumable,
    Miscellaneous,
    QuestItem
}

public class Item
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public ItemType Type { get; set; } = ItemType.QuestItem;

    public EquipmentSlot EquipmentSlot { get; set; } = EquipmentSlot.Weapon;

    public int RequiredLevel { get; set; } = 1;

    public int BaseEffectValue { get; set; } = 0;

    public int BaseValue { get; set; } = 0;

    public int Quantity { get; set; } = 1;

    public int MaxQuantity { get; set; } = 1;
}