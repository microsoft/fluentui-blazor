namespace Microsoft.Fast.Components.FluentUI;

public enum GenerateHeaderOption
{
    None,
    Default,
    Sticky
}

public static class GenerateHeaderOptionExtensions
{
    private static readonly Dictionary<GenerateHeaderOption, string> _generateHeaderValues =
        Enum.GetValues<GenerateHeaderOption>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

    public static string? ToAttributeValue(this GenerateHeaderOption? value) => value == null ? null : _generateHeaderValues[value.Value];
}