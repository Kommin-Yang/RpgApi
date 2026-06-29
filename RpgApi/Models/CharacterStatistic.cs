namespace RpgApi.Models;

public class CharacterStatistic
{
    public int Id { get; set; }

    public int CharacterId { get; set; }

    public StatType Type { get; set; }

    public double Value { get; set; }
}