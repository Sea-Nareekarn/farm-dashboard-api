using System.Security.Claims;

namespace FarmApi.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal user)
    {
        // Supabase picks "sub" as the claim type for user ID, while ASP.NET Core Identity uses ClaimTypes.NameIdentifier
        return user.FindFirst(ClaimTypes.NameIdentifier)?.Value 
               ?? user.FindFirst("sub")?.Value 
               ?? "system";
    }
}