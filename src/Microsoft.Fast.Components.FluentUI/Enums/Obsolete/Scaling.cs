namespace Microsoft.Fast.Components.FluentUI;

[Obsolete("Use AxisScalingMode instead", true)]
public enum Scaling
{
    Content,
    Fill,
    Anchor
}

[Obsolete("Use AxisScalingModeExtensions instead", true)]
internal static class ScalingExtensions
{
    private static readonly Dictionary<Scaling, string> _scalingValues =
        Enum.GetValues<Scaling>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

    public static string? ToAttributeValue(this Scaling? value) => value == null ? null : _scalingValues[value.Value];
}