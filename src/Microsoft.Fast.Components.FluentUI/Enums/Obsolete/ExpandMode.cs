namespace Microsoft.Fast.Components.FluentUI;

[Obsolete("Use AccordionExpandMode instead", true)]
public enum ExpandMode
{
    Single
}

[Obsolete("Use AccordionExpandModeExtensions instead", true)]
internal static class ExpandModeExtensions
{
    private static readonly Dictionary<ExpandMode, string> _expandModeValues =
        Enum.GetValues<ExpandMode>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

    public static string? ToAttributeValue(this ExpandMode? value) => value == null ? null : _expandModeValues[value.Value];
}