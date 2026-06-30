using Microsoft.AspNetCore.Mvc;
using RpgApi.DTOs;
using RpgApi.Services;

namespace RpgApi.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountController : ControllerBase
{
    private readonly AccountService _accountService;

    public AccountController(AccountService accountService, JwtService jwtService)
    {
        _accountService = accountService;
    }

    // Endpoint to create an account
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterAccountDto dto)
    {
        var account = await _accountService.RegisterAccount(dto);

        if (account == null)
            return BadRequest("Username already used.");

        return Ok(new
        {
            Id = account.Id,
            Username = account.Username,
            Email = account.Email
        });
    }

    // Endpoint to login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var token = await _accountService.Login(dto);

        if (token == null)
            return Unauthorized("Email or password erroned!");

        return Ok(new { token });
    }
}