using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RpgApi.DTOs;
using RpgApi.Extensions;
using RpgApi.Services;

namespace RpgApi.Controllers;

[ApiController]
[Route("api/characters")]
public class CharacterController : ControllerBase
{
    private readonly CharacterService _characterService;

    public CharacterController(CharacterService characterService)
    {
        _characterService = characterService;
    }

    [Authorize]
    [HttpGet("profile")]
    public IActionResult Profile()
    {
        return Ok("You are connected!");
    }

    [Authorize]
    [HttpPost("createCharacter")]
    public async Task<IActionResult> CreateCharacter(CreateCharacterDto dto)
    {
        var userId = User.GetUserId();

        var character = await _characterService.CreateCharacter(dto, userId);

        if (character == null)
            return NotFound();

        return Ok(character);
    }

    // Endpoint to get character
    [HttpGet("getCharacter")]
    public async Task<IActionResult> GetCharacter([FromQuery] GetCharacterDto dto)
    {
        var character = await _characterService.GetCharacter(dto);

        if (character == null)
            return NotFound();

        return Ok(character);
    }

    [Authorize]
    [HttpPost("addXP")]
    public async Task<IActionResult> AddXP(AddXPDto dto)
    {
        var userId = User.GetUserId();

        var character = await _characterService.AddXP(dto, userId);

        if (character == null)
            return NotFound();

        return Ok(character);
    }

    // Endpoint to get character stats
    [HttpGet("getStats")]
    public async Task<IActionResult> GetStats([FromQuery] GetCharacterDto dto)
    {
        var stats = await _characterService.GetStats(dto);

        if (stats == null)
            return NotFound();

        return Ok(stats);
    }
}