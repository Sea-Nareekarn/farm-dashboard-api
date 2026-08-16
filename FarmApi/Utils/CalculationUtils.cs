namespace FarmApi.Utils;

public static class CalculationUtils
{
    public static decimal PercentageOf(decimal part, decimal total)
    {
        return total > 0 ? Math.Round(part / total * 100, 2) : 0;
    }
}
