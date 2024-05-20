namespace Microsoft.FluentUI.AspNetCore.Components;

public class Option<TType> : IOptionIcon
{
    public TType? Value { get; set; }

    public string? Text { get; set; }

    public (Icon Value, Color? Color, string? Slot)? Icon { get; set; }

    public bool Disabled { get; set; } = false;

    public bool Selected { get; set; } = false;
}
