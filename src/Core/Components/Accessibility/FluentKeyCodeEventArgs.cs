namespace Microsoft.FluentUI.AspNetCore.Components;

public class FluentKeyCodeEventArgs
{
    public KeyLocation Location { get; init; }
    public KeyCode Key { get; init; }
    public int KeyCode { get; init; }
    public string Value { get; init; } = string.Empty;
    public bool CtrlKey { get; init; }
    public bool ShiftKey { get; init; }
    public bool AltKey { get; init; }
    public bool MetaKey { get; init; }
    public string TargetId { get; set; } = string.Empty;
}
