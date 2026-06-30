using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RpgApi.Data;
using RpgApi.DTOs;
using RpgApi.Models;
using System.Security.Claims;

namespace RpgApi.Services;

public class AccountService
{
    private readonly RpgDbContext _context;
    private readonly JwtService _jwtService;

    public AccountService(RpgDbContext context, JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<Account?> RegisterAccount(RegisterAccountDto dto)
    {
        if (await _context.Accounts.AnyAsync(c => c.Email == dto.Email))
            return null;
        if (await _context.Accounts.AnyAsync(c => c.Username == dto.Username))
            return null;

        var hasher = new PasswordHasher<Account>();

        var account = new Account
        {
            Username = dto.Username,
            Email = dto.Email,
            DateCreation = DateTime.UtcNow,
            LastLoginDate = DateTime.UtcNow
        };

        account.PasswordHash = hasher.HashPassword(account, dto.Password);

        try
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }
        catch (DbUpdateException)
        {
            return null;
        }
    }

    public async Task<string?> Login(LoginDto dto)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == dto.Email);
    
        if (account == null)
        {
            return null;
        }

        var hasher = new PasswordHasher<Account>();

        var result = hasher.VerifyHashedPassword(account, account.PasswordHash, dto.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            return null;
        }

        // Account verification succeed! Now Generate a token!

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, account.Id.ToString())
        };

        var token = _jwtService.GernerateToken(claims);

        return token;
    }
}