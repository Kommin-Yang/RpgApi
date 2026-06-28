namespace RpgApi.Models;

public class ItemInstance
{
    public int Id { get; set; }

    public int ItemId { get; set; }

    public int? CharacterId { get; set; }

    public int? InventoryId { get; set; }

    public ItemRarity Rarity { get; set; }

    public List<ItemStats> Stats { get; set; } = [];

    public DateTime CreatedAt { get; set; }

/*    public int SellValue
    {
        get
        {
            int value = Item.BaseValue;

            value *= GetRarityMultiplier();

            foreach (var stat in Stats)
            {
                value += stat.Value * GetStatWeight(stat.StatType);
            }

            return value;
        }
    }*/
}