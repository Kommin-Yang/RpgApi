using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RpgApi.DTOs;
using RpgApi.Extensions;
using RpgApi.Services;

namespace RpgApi.Controllers;

[ApiController]
[Route("api/items")]
public class ItemController : ControllerBase
{
    private readonly ItemService _itemService;

    public ItemController(ItemService itemService)
    {
        _itemService = itemService;
    }

    // Endpoint to generate a random item
    [Authorize]
    [HttpPost("generateRandomItem")]
    public async Task<IActionResult> GenerateRandomItem(GetCharacterDto dto)
    {
        var userId = User.GetUserId();
        var characterId = User.GetCharacterId();

        var itemInstance = await _itemService.DropRandomItem(dto, userId, characterId!.Value);

        if (itemInstance == null)
            return NotFound("Not created!");

        return Ok(itemInstance);
    }

    // Endpoint to get item
    [HttpGet("getItem")]
    public async Task<IActionResult> GetItem([FromQuery] GetItemDto dto)
    {
        var item = await _itemService.GetItem(dto);

        if (item == null)
        {
            return NotFound();
        }

        return Ok(item);
    }

    // Endpoint to get itemInstance
    [HttpGet("getItemInstance")]
    public async Task<IActionResult> GetItemInstance([FromQuery] GetItemDto dto)
    {
        var itemInstance = await _itemService.GetItemInstance(dto);

        if (itemInstance == null)
        {
            return NotFound();
        }

        return Ok(itemInstance);
    }

    // Endpoint to get itemInstanceStats
    [HttpGet("getItemStats")]
    public async Task<IActionResult> GetItemInstanceStats([FromQuery] GetItemDto dto)
    {
        var itemInstance = await _itemService.GetItemInstanceStats(dto);

        if (itemInstance == null)
        {
            return NotFound();
        }

        return Ok(itemInstance);
    }
}