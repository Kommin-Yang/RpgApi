using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RpgApi.Data;
using RpgApi.DTOs;
using RpgApi.Models;

namespace RpgApi.Services;

public class AccountService
{
    private readonly RpgDbContext _context;

    public AccountService(RpgDbContext context)
    {
        _context = context;
    }

    public async Task<Account> RegisterAccount(RegisterAccountDto dto)
    {
        var hasher = new PasswordHasher<Account>();

        var account = new Account
        {
            Username = dto.Username,
            Email = dto.Email,
            DateCreation = DateTime.UtcNow,
            LastLoginDate = DateTime.UtcNow
        };

        account.PasswordHash = hasher.HashPassword(account, dto.Password);

        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();

        return account;
    }

    public async Task<Account?> Login(LoginDto dto)
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

        return account;
    }
}