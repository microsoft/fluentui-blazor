namespace Microsoft.Fast.Components.FluentUI;

public enum GenerateHeaderOptions
{
    None,
    Default,
    Sticky
}

internal static class GenerateHeaderExtensions
{
    private static readonly Dictionary<GenerateHeaderOptions, string> _generateHeaderValues =
        Enum.GetValues<GenerateHeaderOptions>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

    public static string? ToAttributeValue(this GenerateHeaderOptions? value) => value == null ? null : _generateHeaderValues[value.Value];
}