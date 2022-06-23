namespace Microsoft.Fast.Components.FluentUI;

[Obsolete("Use FlipperDirection instead", true)]
public enum Direction
{
    Previous,
    Next
}

[Obsolete("Use FlipperDirectionExtensions instead", true)]
internal static class DirectionExtensions
{
    private static readonly Dictionary<Direction, string> _directionValues =
        Enum.GetValues<Direction>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

    public static string? ToAttributeValue(this Direction? value) => value == null ? null : _directionValues[value.Value];
}
