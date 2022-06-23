namespace Microsoft.Fast.Components.FluentUI;

public enum SelectPosition
{
    Above,
    Below
}

public static class SelectPositionExtensions
{
    private static readonly Dictionary<SelectPosition, string> _positionValues =
        Enum.GetValues<SelectPosition>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

    public static string? ToAttributeValue(this SelectPosition? value) => value == null ? null : _positionValues[value.Value];
}