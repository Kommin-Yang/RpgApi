namespace RpgApi.Models;

public class CharacterInventory
{
    public int Id { get; set; }

    public int CharacterId { get; set; }

    public int GoldAmount { get; set; }

    public List<ItemInstance> ItemInstances { get; set; } = [];
}