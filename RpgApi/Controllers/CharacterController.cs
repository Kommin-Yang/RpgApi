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

    // Endpoint to select character
    [Authorize]
    [HttpPost("selectCharacter")]
    public async Task<IActionResult> SelectCharacter([FromBody] SelectCharacterDto dto)
    {
        var userId = User.GetUserId();

        var token = await _characterService.SelectCharacter(dto, userId);

        if (token == null)
            return NotFound("Character doesnt exist!");

        return Ok(new { token });
    }

    // Endpoint to attack
    [Authorize]
    [HttpPost("attack")]
    public async Task<IActionResult> Attack(AttackDto dto)
    {
        var userId = User.GetUserId();
        int? characterId = User.GetCharacterId();

        var hasAttacked = await _characterService.Attack(dto, userId, characterId!.Value);

        if (characterId == null)
            return BadRequest("You are not controlling a character!");

        return Ok(hasAttacked);
    }

    // Endpoint to get character
    [HttpGet("getCharacter")]
    public async Task<IActionResult> GetCharacter([FromQuery] GetCharacterDto dto)
    {
        var character = await _characterService.GetCharacter(dto);

        if (character == null)
            return NotFound();

        return Ok(character.Name);
    }

    [Authorize]
    [HttpPost("addXP")]
    public async Task<IActionResult> AddXP(AddXPDto dto)
    {
        var userId = User.GetUserId();
        var characterId = User.GetCharacterId();

        var character = await _characterService.AddXP(dto, userId, characterId!.Value);

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