namespace Microsoft.Fast.Components.FluentUI;

internal static class IconsExtensions
{
    /// <summary>
    /// Returns true if the icon is a system icon.
    /// </summary>
    /// <param name="icon"></param>
    /// <returns></returns>
    internal static bool IsSystemIcon(this string? icon)
    {
        return !string.IsNullOrEmpty(icon) && icon.StartsWith("<icon ", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Extract the size and variant from the System Icon string.
    /// </summary>
    /// <param name="icon"></param>
    /// <returns></returns>
    internal static (int Size, string Variant) ExtractSystemIconDetails(this string? icon)
    {
        if (!IsSystemIcon(icon))
        {
            return (0, string.Empty);
        }

        int sizeStart = icon.IndexOf("size=\"", StringComparison.OrdinalIgnoreCase);
        int sizeEnd = icon.IndexOf("\"", sizeStart + 6, StringComparison.OrdinalIgnoreCase);
        int variantStart = icon.IndexOf("variant=\"", StringComparison.OrdinalIgnoreCase);
        int variantEnd = icon.IndexOf("\"", variantStart + 9, StringComparison.OrdinalIgnoreCase);

        if (sizeEnd > sizeStart && variantEnd > variantStart)
        {
            int size = int.Parse(icon.Substring(sizeStart + 6, sizeEnd - sizeStart - 6));
            string variant = icon.Substring(variantStart + 9, variantEnd - variantStart - 9);
            return (size, variant);
        }
        else
        {
            return (0, string.Empty);
        }
    }
}
