namespace Microsoft.Fast.Components.FluentUI;

[Obsolete("Use AxisPositioningMode instead", true)]
public enum Positioning
{
    Uncontrolled,
    Locktodefault,
    Dynamic
}

[Obsolete("Use AxisPositioningModeExtensions instead", true)]
internal static class PositioningExtensions
{
    private static readonly Dictionary<Positioning, string> _positioningValues =
        Enum.GetValues<Positioning>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

    public static string? ToAttributeValue(this Positioning? value) => value == null ? null : _positioningValues[value.Value];
}
