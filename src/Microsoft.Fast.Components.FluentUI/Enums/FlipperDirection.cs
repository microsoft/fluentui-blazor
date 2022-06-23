namespace Microsoft.Fast.Components.FluentUI;

public enum FlipperDirection
{
    Previous,
    Next
}

public static class FlipperDirectionExtensions
{
    private static readonly Dictionary<FlipperDirection, string> _directionValues =
        Enum.GetValues<FlipperDirection>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

    public static string? ToAttributeValue(this FlipperDirection? value) => value == null ? null : _directionValues[value.Value];
}
