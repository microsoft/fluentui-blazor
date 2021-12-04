namespace Microsoft.Fast.Components.FluentUI;

public enum Color
{
    Highlight,
    Lowlight
}

internal static class ColorExtensions
{
    private static readonly Dictionary<Color, string> _colorValues =
        Enum.GetValues<Color>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

    public static string? ToAttributeValue(this Color? value) => value == null ? null : _colorValues[value.Value];
}

public enum Fill
{
    Highlight,
    Lowlight
}

internal static class FillExtensions
{
    private static readonly Dictionary<Fill, string> _fillValues =
        Enum.GetValues<Fill>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

    public static string? ToAttributeValue(this Fill? value) => value == null ? null : _fillValues[value.Value];
}