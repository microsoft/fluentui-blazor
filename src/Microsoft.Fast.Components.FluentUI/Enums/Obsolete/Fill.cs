namespace Microsoft.Fast.Components.FluentUI;

[Obsolete("Use a string value instead", true)]
public enum Fill
{
    Highlight,
    Lowlight
}

[Obsolete("Fill enum is obsolete", true)]
internal static class FillExtensions
{
    private static readonly Dictionary<Fill, string> _fillValues =
        Enum.GetValues<Fill>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

    public static string? ToAttributeValue(this Fill? value) => value == null ? null : _fillValues[value.Value];
}