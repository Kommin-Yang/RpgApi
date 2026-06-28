using System.Security.Claims;

namespace RpgApi.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        return int.Parse(
            user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }

    public static int? GetCharacterId(this ClaimsPrincipal user)
    {
        var claim = user.FindFirst("CharacterId");
        if (claim == null)
            return null;
        return int.Parse(claim!.Value);
    }
}