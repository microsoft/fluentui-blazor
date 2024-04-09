namespace Microsoft.FluentUI.AspNetCore.Components;

internal static class StringExtensions
{
    /// <summary />
    public static string ToPercentage(this decimal value)
    {
        return value.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);
    }
}
