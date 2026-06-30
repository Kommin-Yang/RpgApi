using Microsoft.EntityFrameworkCore;
using RpgApi.Data;
using RpgApi.DTOs;
using RpgApi.Models;
using System.Security.Claims;

namespace RpgApi.Services;

public class CharacterService
{
    private readonly RpgDbContext _context;
    private readonly GameState _gameState;
    private readonly JwtService _jwtService;

    public CharacterService(RpgDbContext context,
        GameState gameState, JwtService jwtService)
    {
        _context = context;
        _gameState = gameState;
        _jwtService = jwtService;
    }

    public async Task<Character?> CreateCharacter(CreateCharacterDto dto, int userId)
    {
        if (await _context.Characters.AnyAsync(c => c.Name == dto.Name))
            return null;

        try
        {
            var character = new Character
            {
                Name = dto.Name,
                Level = 1,
                XP = 0,
                XPToNextLevel = 100,
                AccountId = userId,

                Stats = CreateDefaultStats(),
                Inventory = CreateDefaultInventory(),
                Equipements = CreateDefaultEquipements()
            };

            _context.Characters.Add(character);
            await _context.SaveChangesAsync();

            return character;
        }
        catch (DbUpdateException)
        {
            return null;
        }
    }

    public async Task<string?> SelectCharacter(SelectCharacterDto dto, int userId)
    {
        var character = await _context.Characters
            .Include(c => c.Stats)
            .Include(c => c.Inventory)
            .Include(c => c.Equipements)
            .FirstOrDefaultAsync(
            c => c.Id == dto.CharacterId && 
            c.AccountId == userId);

        if (character == null)
            return null;

        _gameState.AddPlayer(character); // Add the character selected and connected in the RAM

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim("CharacterId", character.Id.ToString())
        };

        var token = _jwtService.GernerateToken(claims);

        return token;
    }
    
    public async Task<Character?> GetCharacter(GetCharacterDto dto)
    {
        Character? character;

        if (dto.Id != 0)
        {
            character = await _context.Characters.FindAsync(dto.Id);
        }
        else
        {
            character = await _context.Characters.Include(c => c.Stats).
                Include(c => c.Inventory).
                Include(c => c.Equipements).
                FirstOrDefaultAsync(c => c.Id == dto.Id);
        }

        if (character == null)
        {
            return null;
        }

        return character;
    }

    public string AddXP(AddXPDto dto, int userId, int characterId)
    {
        // Get the one who attacks in RAM
        var character = _gameState.GetPlayer(characterId);
        if (character == null || character.Character.AccountId != userId)
            return "Character not found or not connected";

        character.Character.XP += dto.XPAmount;

        while (character.Character.XP >= character.Character.XPToNextLevel)
        {
            character.Character.XP -= character.Character.XPToNextLevel;
            character.Character.Level++;
            character.Character.XPToNextLevel = (int)(character.Character.XPToNextLevel * 1.2);
            character.Character.PointsToAllocate += 5;
        }

        return $"You won {dto.XPAmount} XP";
    }

    public string Attack(AttackDto dto, int userId, int characterId)
    {
        // Get the one who attacks in RAM
        var character = _gameState.GetPlayer(characterId);
        if (character == null || character.Character.AccountId != userId)
            return "Character not found or not connected";

        // Get the one who is attacked in RAM
        var enemy = _gameState.GetPlayer(dto.IdAttacked);
        if (enemy == null)
            return "Enemy not found or dead";

        var attacker = _gameState.GetPlayer(characterId);
        var target = _gameState.GetPlayer(dto.IdAttacked);

        var characterStats = attacker!.CachedStats;
        var enemyStats = target!.CachedStats;

        double randCrit = Random.Shared.NextDouble();
        bool isCrit = randCrit * 100.0 <= characterStats[StatType.CriticalRate].Value;

        double rawDamage = characterStats[StatType.Strength].Value * 2 +
            characterStats[StatType.Agility].Value;

        double totalDamage = rawDamage + (isCrit ? (rawDamage *
            (int)Math.Round(characterStats[StatType.CriticalDamage].Value / 100.0)) : 0) -
            enemyStats[StatType.Defense].Value;

        enemyStats[StatType.Health].Value -= totalDamage;

        return $"You attacked and dealt {totalDamage} damage.";
    }

    public async Task<List<CharacterStatistic>?> GetStats(GetCharacterDto dto)
    {
        Character? character = await _context.Characters.Include(c => c.Stats)
            .FirstOrDefaultAsync(c => c.Id == dto.Id);

        if (character == null)
            return null;

        var stats = character.Stats;

        if (stats == null)
            return null;

        return stats;
    }

    private List<CharacterStatistic> CreateDefaultStats()
    {
        var stats = new List<CharacterStatistic>();

        foreach (StatType statType in Enum.GetValues<StatType>())
        {
            var stat = new CharacterStatistic
            {
                Type = statType,
                Value = 0
            };
            stat.Value = stat.Type switch
            {
                StatType.Strength => 5,
                StatType.Agility => 5,
                StatType.Vitality => 5,
                StatType.Intelligence => 5,
                _ => 0
            };
            stats.Add(stat);
        }

        UpdateDerivedStats(stats);

        return stats;
    }

    private void UpdateDerivedStats(List<CharacterStatistic> statistics)
    {
        var stats = statistics.ToDictionary(s => s.Type);

        stats[StatType.Defense].Value =
            stats[StatType.Strength].Value * 2 + 
            (int)Math.Round(stats[StatType.Agility].Value / 2.0);

        stats[StatType.Health].Value =
            stats[StatType.Vitality].Value * 10;

        stats[StatType.Mana].Value =
            stats[StatType.Intelligence].Value * 15;

        stats[StatType.CriticalRate].Value =
            5 + (int)Math.Round(stats[StatType.Agility].Value / 2.0);

        stats[StatType.CriticalDamage].Value =
            15 + stats[StatType.Agility].Value +
            (int)Math.Round(stats[StatType.Strength].Value / 2.0);
    }

    private CharacterInventory CreateDefaultInventory()
    {
        return new CharacterInventory
        {
            ItemInstances = []
        };
    }

    private List<CharacterEquipment> CreateDefaultEquipements()
    {
        return new List<CharacterEquipment>
        {
            new CharacterEquipment
            {
                ItemInstanceId = 1,
                Slot = EquipmentSlot.Weapon
            }
        };
    }
}