namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// List events that can be set as ignored by <see cref="IFocusManager"/>
/// </summary>
public sealed class FocusManagerIgnoredKeydownEvents
{
    public bool Tab { get; set; }
    public bool Escape { get; set; }
    public bool Enter { get; set; }
    public bool ArrowUp { get; set; }
    public bool ArrowDown { get; set; }
    public bool ArrowLeft { get; set; }
    public bool ArrowRight { get; set; }
    public bool PageUp { get; set; }
    public bool PageDown { get; set; }
    public bool Home { get; set; }
    public bool End { get; set; }
}

