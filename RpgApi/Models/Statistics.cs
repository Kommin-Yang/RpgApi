using System.ComponentModel.DataAnnotations.Schema;

namespace RpgApi.Models;

[Table("CharacterStats")]
public class Statistics
{
    public int Id { get; set; }

    public int CharacterId { get; set; }

    public StatType Type { get; set; }

    public int Value { get; set; }
}