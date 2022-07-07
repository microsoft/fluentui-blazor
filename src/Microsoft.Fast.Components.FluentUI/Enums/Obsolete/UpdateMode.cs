namespace Microsoft.Fast.Components.FluentUI;

[Obsolete("Use AutoUpdateMode instead", true)]
public enum UpdateMode
{
    Anchor,
    Auto
}

[Obsolete("Use AutoUpdateModeExtensions instead", true)]
internal static class UpdateModeExtensions
{
    private static readonly Dictionary<UpdateMode, string> _updateModeValues =
        Enum.GetValues<UpdateMode>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

    public static string? ToAttributeValue(this UpdateMode? value) => value == null ? null : _updateModeValues[value.Value];
}
