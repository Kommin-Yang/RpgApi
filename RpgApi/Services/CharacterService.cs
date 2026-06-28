using Microsoft.EntityFrameworkCore;
using RpgApi.Data;
using RpgApi.DTOs;
using RpgApi.Models;

namespace RpgApi.Services;

public class CharacterService
{
    private readonly RpgDbContext _context;

    public CharacterService(RpgDbContext context)
    {
        _context = context;
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
        /*
        character.Stats = CreateDefaultStats(character.Id);
        character.Inventory = CreateDefaultInventory(character.Id);
        character.Equipements = CreateDefaultEquipements(character.Id);

        await _context.SaveChangesAsync();
        */
        return character;
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

    public async Task<Character?> AddXP(AddXPDto dto, int userId)
    {
        var character = await _context.Characters
            .FirstOrDefaultAsync(c =>
            c.Id == dto.Id &&
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
                //CharacterId = characterId,
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
            //CharacterId = characterId,
            ItemInstances = []
        };
    }

    private List<Equipement> CreateDefaultEquipements()
    {
        return new List<Equipement>
        {
            new Equipement
            {
                //CharacterId = characterId,
                ItemInstanceId = 1,
                Slot = EquipmentSlot.Weapon
            }
        };
    }
}