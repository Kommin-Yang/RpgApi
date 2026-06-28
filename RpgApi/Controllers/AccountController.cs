using Microsoft.AspNetCore.Mvc;
using RpgApi.DTOs;
using RpgApi.Services;

namespace RpgApi.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountController : ControllerBase
{
    private readonly AccountService _accountService;
    private readonly JwtService _jwtService;

    public AccountController(AccountService accountService, JwtService jwtService)
    {
        _accountService = accountService;
        _jwtService = jwtService;
    }

    // Endpoint to create an account
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterAccountDto dto)
    {
        var account = await _accountService.RegisterAccount(dto);

        if (account == null)
            return NotFound();

        return Ok(new
        {
            Id = account.Id,
            Username = account.Username,
            Email = account.Email
        });
    }

    // Endpoint to login
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var account = await _accountService.Login(dto);

        if (account == null)
            return Unauthorized();

        var token = _jwtService.GernerateToken(account);

        return Ok(new
        {
            token,
            accountId = account.Id,
            username = account.Username
        });
    }
}