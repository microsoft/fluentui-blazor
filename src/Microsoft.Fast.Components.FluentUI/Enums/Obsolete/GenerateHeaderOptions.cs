namespace Microsoft.Fast.Components.FluentUI;

[Obsolete("Use GenerateHeaderOption instead", true)]
public enum GenerateHeaderOptions
{
    None,
    Default,
    Sticky
}

[Obsolete("Use GenerateHeaderOptionExtensions instead", true)]
internal static class GenerateHeaderExtensions
{
    private static readonly Dictionary<GenerateHeaderOptions, string> _generateHeaderValues =
        Enum.GetValues<GenerateHeaderOptions>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

    public static string? ToAttributeValue(this GenerateHeaderOptions? value) => value == null ? null : _generateHeaderValues[value.Value];
}