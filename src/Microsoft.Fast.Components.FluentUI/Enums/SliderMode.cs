namespace Microsoft.Fast.Components.FluentUI;

public enum SliderMode
{
    SingleValue
}

public static class SliderModeExtensions
{
    private static readonly Dictionary<SliderMode, string> _sliderModeValues =
        Enum.GetValues<SliderMode>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

    public static string? ToAttributeValue(this SliderMode? value)
    {
        if (value is null) return null;
        return value switch
        {
            SliderMode.SingleValue => "single-value",
            _ => _sliderModeValues[value.Value],
        };
    }
}