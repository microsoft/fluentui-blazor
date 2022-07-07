namespace Microsoft.Fast.Components.FluentUI;

public enum AxisPositioningMode
{
    Uncontrolled,
    Locktodefault,
    Dynamic
}


public static class AxisPositioningModeExtensions
{
    private static readonly Dictionary<AxisPositioningMode, string> _positioningValues =
        Enum.GetValues<AxisPositioningMode>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

    public static string? ToAttributeValue(this AxisPositioningMode? value) => value == null ? null : _positioningValues[value.Value];
}
