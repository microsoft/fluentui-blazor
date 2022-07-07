namespace Microsoft.Fast.Components.FluentUI;

[Obsolete("Use SelectPosition instead", true)]
public enum Position
{
    Above,
    Below
}

[Obsolete("Use SelectPositionExtensions instead", true)]
internal static class PositionExtensions
{
    private static readonly Dictionary<Position, string> _positionValues =
        Enum.GetValues<Position>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

    public static string? ToAttributeValue(this Position? value) => value == null ? null : _positionValues[value.Value];
}