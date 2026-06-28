using Microsoft.EntityFrameworkCore;
using RpgApi.Data;
using RpgApi.DTOs;
using RpgApi.Models;

namespace RpgApi.Services;

public class ItemService
{
    private readonly RpgDbContext _context;

    public ItemService(RpgDbContext context)
    {
        _context = context;
    }

    // A BIG RANDOM GENERATE ITEMINSTANCE \\
    public async Task<ItemInstance?> DropRandomItem(GetCharacterDto dto, int userId)
    {
        var character = await _context.Characters
            .FirstOrDefaultAsync(c =>
            c.Id == dto.Id &&
            c.AccountId == userId);

        if (character == null)
            return null;

        Inventory? inventory = character.Inventory;

        if (inventory == null)
            return null;

        int randType = Random.Shared.Next(100);
        ItemType? itemType = randType < 10 ? ItemType.Weapon : 
                             randType < 20 ? ItemType.Armor : 
                             randType < 30 ? ItemType.Consumable :
                             randType < 60 ? ItemType.Miscellaneous : null;

        if (itemType == null)
            return null;

        var items = await _context.Items
                            .Where(i => i.Type == itemType &&
                            i.RequiredLevel <= character.Level)
                            .ToListAsync();

        if (items.Count == 0)
            return null;

        Item item = items[Random.Shared.Next(items.Count)];

        if (item == null)
            return null;


        ItemInstance itemInstance = new()
        {
            ItemId = item.Id,
            CharacterId = inventory != null ? dto.Id : null,
            InventoryId = inventory != null ? inventory?.Id : null,
            CreatedAt = DateTime.UtcNow
        };

        if(itemType != ItemType.Consumable || itemType != ItemType.Miscellaneous
            || itemType != ItemType.QuestItem)
        {
            GenerateRarityAndStats(itemInstance);
        }

        inventory?.ItemInstances.Add(itemInstance);

        _context.ItemInstances.Add(itemInstance);
        await _context.SaveChangesAsync();

        return itemInstance;
    }

    private void GenerateRarityAndStats(ItemInstance itemInstance)
    {
        int randomRarity = Random.Shared.Next(100);
        ItemRarity? itemRarity = randomRarity < 50 ? ItemRarity.Common :
                                 randomRarity < 75 ? ItemRarity.Uncommon :
                                 randomRarity < 90 ? ItemRarity.Rare :
                                 randomRarity < 97 ? ItemRarity.Epic :
                                 randomRarity < 99 ? ItemRarity.Legendary :
                                 ItemRarity.Mythic;

        itemInstance.Rarity = itemRarity.Value;

        int statsNumber = itemRarity switch
        {
            ItemRarity.Common => 1,
            ItemRarity.Uncommon => 2,
            ItemRarity.Rare => 3,
            ItemRarity.Epic => 4,
            ItemRarity.Legendary => 5,
            ItemRarity.Mythic => 6,
            _ => 1
        };

        int randomStatType = 0;
        int randomStatValue = 0;
        StatType? statType = null;
        for (int i = 0; i < statsNumber; i++)
        {
            randomStatType = Random.Shared.Next(100);

            if (randomStatType < 50)
            {
                statType = (StatType)Random.Shared.Next(1, 6);
                randomStatValue = Random.Shared.Next(1 + (int)itemRarity / 2, 5 + (int)itemRarity);
            }
            else if (randomStatType < 60)
            {
                statType = StatType.Health;
                randomStatValue = Random.Shared.Next(9 + (int)itemRarity, 25 + (int)itemRarity);
            }
            else if (randomStatType < 70)
            {
                statType = StatType.Mana;
                randomStatValue = Random.Shared.Next(14 + (int)itemRarity, 30 + (int)itemRarity);
            }
            else if (randomStatType < 80)
            {
                statType = StatType.CriticalRate;
                randomStatValue = Random.Shared.Next(4 + (int)itemRarity, 10 + (int)itemRarity);
            }
            else if (randomStatType < 90)
            {
                statType = StatType.CriticalDamage;
                randomStatValue = Random.Shared.Next(9 + (int)itemRarity, 15 + (int)itemRarity);
            }
            else if (randomStatType < 95)
            {
                statType = StatType.AttackSpeed;
                randomStatValue = Random.Shared.Next(1 + (int)itemRarity / 2, 5 + (int)itemRarity);
            }
            else
            {
                statType = StatType.LifeSteal;
                randomStatValue = Random.Shared.Next(1 + (int)itemRarity / 2, 5 + (int)itemRarity);
            }

            ItemStats? uniqueStat = itemInstance.Stats.FirstOrDefault(c => c.StatType == statType);

            if (uniqueStat != null)
            {
                uniqueStat.Value += randomStatValue;
            }
            else
            {
                uniqueStat = new()
                {
                    ItemInstanceId = itemInstance.Id,
                    StatType = statType.Value,
                    Value = randomStatValue
                };
                itemInstance.Stats.Add(uniqueStat);
            }
        }
    }

    public async Task<Item?> GetItem(GetItemDto dto)
    {
        var item = await _context.Items.FindAsync(dto.ItemId);

        if (item == null)
        {
            return null;
        }

        return item;
    }

    public async Task<ItemInstance?> GetItemInstance(GetItemDto dto)
    {
        var instance = await _context.ItemInstances.FindAsync(dto.ItemInstanceId);

        if(instance == null)
        {
            return null;
        }

        return instance;
    }

    public async Task<List<ItemStats>?> GetItemInstanceStats(GetItemDto dto)
    {
        var instance = await _context.ItemInstances.FindAsync(dto.ItemInstanceId);

        if(instance == null || instance.Stats == null)
        {
            return null;
        }

        return instance.Stats;
    }
}