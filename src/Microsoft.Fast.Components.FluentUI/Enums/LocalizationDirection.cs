namespace Microsoft.Fast.Components.FluentUI;

public enum LocalizationDirection

{
    ltr,
    rtl
}

public static class LocalizationDirectionExtensions
{
    private static readonly Dictionary<LocalizationDirection, string> _directionValues =
        Enum.GetValues<LocalizationDirection>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

    public static string? ToAttributeValue(this LocalizationDirection? value) => value == null ? null : _directionValues[value.Value];
}