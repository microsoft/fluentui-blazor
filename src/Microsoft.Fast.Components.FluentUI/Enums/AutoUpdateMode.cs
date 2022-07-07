namespace Microsoft.Fast.Components.FluentUI;

public enum AutoUpdateMode
{
    Anchor,
    Auto
}

public static class AutoUpdateModeExtensions
{
    private static readonly Dictionary<AutoUpdateMode, string> _updateModeValues =
        Enum.GetValues<AutoUpdateMode>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

    public static string? ToAttributeValue(this AutoUpdateMode? value) => value == null ? null : _updateModeValues[value.Value];
}
