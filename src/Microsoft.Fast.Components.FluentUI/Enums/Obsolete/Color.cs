namespace Microsoft.Fast.Components.FluentUI;


[Obsolete("Use a string value instead", true)]
public enum Color
{
    Highlight,
    Lowlight
}

[Obsolete("Color enum is obsolete", true)]
internal static class ColorExtensions
{
    private static readonly Dictionary<Color, string> _colorValues =
        Enum.GetValues<Color>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

    public static string? ToAttributeValue(this Color? value) => value == null ? null : _colorValues[value.Value];
}

