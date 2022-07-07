namespace Microsoft.Fast.Components.FluentUI;

public enum AxisScalingMode
{
    Content,
    Fill,
    Anchor
}

public static class AxisScalingModeExtensions
{
    private static readonly Dictionary<AxisScalingMode, string> _scalingValues =
        Enum.GetValues<AxisScalingMode>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

    public static string? ToAttributeValue(this AxisScalingMode? value) => value == null ? null : _scalingValues[value.Value];
}