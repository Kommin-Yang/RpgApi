namespace RpgApi.Models;

public class ActivePlayer
{
    public Character Character { get; set; }

    public Dictionary<StatType, CharacterStatistic> CachedStats { get; set; }

    public ActivePlayer(Character character)
    {
        Character = character;
        CachedStats = character.Stats.ToDictionary(s => s.Type);
    }
}