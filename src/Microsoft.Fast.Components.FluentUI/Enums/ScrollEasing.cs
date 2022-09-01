namespace Microsoft.Fast.Components.FluentUI;

public enum ScrollEasing
{
    Linear,
    EaseIn,
    EaseOut,
    EaseInOut
}

public static class ScrollEasingExtensions
{
    public static string? ToAttributeValue(this ScrollEasing? value)
    {
        if (value is null) return null;
        return value switch
        {
            ScrollEasing.Linear => "linear",
            ScrollEasing.EaseIn => "ease-in",
            ScrollEasing.EaseOut => "ease-out",
            ScrollEasing.EaseInOut => "ease-in-out",
            _ => throw new NotImplementedException(),
        };
    }

}