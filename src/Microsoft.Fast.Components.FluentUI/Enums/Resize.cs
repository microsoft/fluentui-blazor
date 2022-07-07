namespace Microsoft.Fast.Components.FluentUI;

public enum Resize
{
    Horizontal,
    Vertical,
    Both
}

public static class ResizeExtensions
{
    private static readonly Dictionary<Resize, string> _resizeValues =
        Enum.GetValues<Resize>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

    public static string? ToAttributeValue(this Resize? value) => value == null ? null : _resizeValues[value.Value];
}
