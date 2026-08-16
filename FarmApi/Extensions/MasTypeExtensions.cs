using FarmApi.Models;

namespace FarmApi.Extensions;

public static class MasTypeExtensions
{
    public static string GetNameLoc(this IEnumerable<MasTypeDb> masTypes, string? code)
    {
        return masTypes.FirstOrDefault(t => t.Code == code)?.NameLoc ?? string.Empty;
    }
}
