using System.ComponentModel.DataAnnotations.Schema;

namespace RpgApi.Models;

public enum EquipmentSlot
{
    Helmet = 1,
    Chest,
    Hands,
    Legs,
    Foots,
    Weapon
}

[Table("CharacterEquipment")]
public class Equipement
{
    public int Id { get; set; }

    public int CharacterId { get; set; }

    public int ItemInstanceId { get; set; }

    public EquipmentSlot Slot { get; set; }
}