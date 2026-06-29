namespace RpgApi.Models;

public class CharacterEquipment
{
    public int Id { get; set; }

    public int CharacterId { get; set; }

    public int ItemInstanceId { get; set; }

    public EquipmentSlot Slot { get; set; }
}