using System.ComponentModel.DataAnnotations.Schema;

namespace RpgApi.Models;

[Table("CharacterEquipment")]
public class Equipement
{
    public int Id { get; set; }

    public int CharacterId { get; set; }

    public int ItemInstanceId { get; set; }

    public EquipmentSlot Slot { get; set; }
}