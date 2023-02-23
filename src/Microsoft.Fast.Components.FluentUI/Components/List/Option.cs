namespace Microsoft.Fast.Components.FluentUI;

public class Option<TType>
{
    public TType? Value { get; set; }
    public TType? Text { get; set; }

    public (string Name, IconSize? Size, IconVariant? Variant, Color? Color, string? Slot) Icon { get; set; }

    public bool Disabled { get; set; } = false;
    public bool Selected { get; set; } = false;


}

