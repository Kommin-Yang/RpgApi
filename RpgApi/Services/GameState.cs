using RpgApi.Models;
using System.Collections.Concurrent;

namespace RpgApi.Services;

public class GameState
{
    // Dictionary to save the actives players in RAM (Key: CharacterId, Value: Object)
    private readonly ConcurrentDictionary<int, ActivePlayer> _activePlayer = new();

    // Add the Character in RAM when connecting
    public void AddPlayer(Character character)
    {
        _activePlayer[character.Id] = new ActivePlayer(character);
    }

    // Get the ActiveCharacter in the RAM
    public ActivePlayer? GetPlayer(int characterId)
    {
        _activePlayer.TryGetValue(characterId, out var player);
        return player;
    }

    // Delete the ActiveCharacter from RAM after logging out
    public void RemovePlayer(int characterId)
    {
        _activePlayer.TryRemove(characterId, out _);
    }

    // Get all the characters for the periodic save
    public IEnumerable<ActivePlayer> GetAllActivePlayers() => _activePlayer.Values;
}