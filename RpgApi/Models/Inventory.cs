using System.ComponentModel.DataAnnotations.Schema;

namespace RpgApi.Models;

[Table("CharacterInventory")]
public class Inventory
{
    public int Id { get; set; }

    public int CharacterId { get; set; }

    public List<ItemInstance> ItemInstances { get; set; } = [];
}