using Microsoft.EntityFrameworkCore;
using RpgApi.Data;
using RpgApi.DTOs;
using RpgApi.Models;
using System.Security.Claims;

namespace RpgApi.Services;

public class CharacterService
{
    private readonly RpgDbContext _context;
    private readonly JwtService _jwtService;

    public CharacterService(RpgDbContext context, JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<Character> CreateCharacter(CreateCharacterDto dto, int userId)
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

    public async Task<string?> SelectCharacter(SelectCharacterDto dto, int userId)
    {
        var character = await _context.Characters.FirstOrDefaultAsync(
            c => c.Id == dto.CharacterId && 
            c.AccountId == userId);

        if (character == null)
            return null;

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

    public async Task<Character?> AddXP(AddXPDto dto, int userId, int characterId)
    {
        var character = await _context.Characters
            .FirstOrDefaultAsync(c =>
            c.Id == characterId &&
            c.AccountId == userId);

        if (character == null)
            return null;

        character.XP += dto.XPAmount;

        while (character.XP >= character.XPToNextLevel)
        {
            character.XP -= character.XPToNextLevel;
            character.Level++;
            character.XPToNextLevel = (int)(character.XPToNextLevel * 1.2);
            character.PointsToAllocate += 5;
        }

        await _context.SaveChangesAsync();

        return character;
    }

    public async Task<string> Attack(AttackDto dto, int userId, int characterId)
    {
        var character = await _context.Characters
            .Include(c => c.Stats)
            .Include(c => c.Equipements)
            .Include(c => c.Inventory)
            .FirstOrDefaultAsync(c =>
            c.Id == characterId &&
            c.AccountId == userId);

        if (character == null)
            return "Character not found";

        var enemy = await _context.Characters
            .Include(c => c.Stats)
            .Include(c => c.Equipements)
            .Include(c => c.Inventory)
            .FirstOrDefaultAsync(c =>
            c.Id == dto.IdAttacked);

        if (enemy == null)
            return "Enemy not found";

        var characterStats = character.Stats.ToDictionary(s => s.Type);
        var enemyStats = enemy.Stats.ToDictionary(s => s.Type);

        int randCrit = Random.Shared.Next(100);
        bool isCrit = false;

        if(randCrit <= characterStats[StatType.CriticalRate].Value)
            isCrit = true;

        int rawDamage = characterStats[StatType.Strength].Value * 2 +
            characterStats[StatType.Agility].Value;

        enemyStats[StatType.Health].Value -= rawDamage + (isCrit ? (rawDamage *
            (int)Math.Round(characterStats[StatType.CriticalDamage].Value / 100.0)) : 0) - 
            enemyStats[StatType.Defense].Value;

        await _context.SaveChangesAsync();

        return "The character attacked";
    }

    public async Task<List<Statistics>?> GetStats(GetCharacterDto dto)
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

    private List<Statistics> CreateDefaultStats()
    {
        var stats = new List<Statistics>();

        foreach (StatType statType in Enum.GetValues<StatType>())
        {
            var stat = new Statistics
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

    private void UpdateDerivedStats(List<Statistics> statistics)
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

    private Inventory CreateDefaultInventory()
    {
        return new Inventory
        {
            ItemInstances = []
        };
    }

    private List<Equipement> CreateDefaultEquipements()
    {
        return new List<Equipement>
        {
            new Equipement
            {
                ItemInstanceId = 1,
                Slot = EquipmentSlot.Weapon
            }
        };
    }
}