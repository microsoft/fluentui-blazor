namespace Microsoft.Fast.Components.FluentUI;

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