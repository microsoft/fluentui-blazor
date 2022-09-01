namespace Microsoft.Fast.Components.FluentUI;

public enum HorizontalScrollView
{
    Default,
    Mobile
}

public static class HorizontalScrollViewExtensions
{
    private static readonly Dictionary<HorizontalScrollView, string> _hsvValues =
        Enum.GetValues<HorizontalScrollView>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

    public static string? ToAttributeValue(this HorizontalScrollView? value) => value == null ? null : _hsvValues[value.Value];
}