namespace Microsoft.Fast.Components.FluentUI;

public enum AccordionExpandMode
{
    Single,
    Multi
}

public static class AccordionExpandModeExtensions
{
    private static readonly Dictionary<AccordionExpandMode, string> _expandModeValues =
        Enum.GetValues<AccordionExpandMode>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

    public static string? ToAttributeValue(this AccordionExpandMode? value) => value == null ? null : _expandModeValues[value.Value];
}