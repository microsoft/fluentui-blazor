namespace Microsoft.Fast.Components.FluentUI;


public enum TextAreaResize
{
    Horizontal,
    Vertical,
    Both
}


public static class TextAreaResizeExtensions
{
    private static readonly Dictionary<TextAreaResize, string> _resizeValues =
        Enum.GetValues<TextAreaResize>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

    public static string? ToAttributeValue(this TextAreaResize? value) => value == null ? null : _resizeValues[value.Value];
}
