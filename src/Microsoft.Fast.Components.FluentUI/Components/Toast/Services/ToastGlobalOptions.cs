namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public class ToastGlobalOptions
{
    public bool NewestOnTop { get; set; } = true;

    public int MaxToasts { get; set; } = 4;

    public ToasterPosition PositionClass { get; set; } = ToasterPosition.TopEnd;

    public int MaximumOpacity { get; set; } = 100;

    public int ShowTransitionDuration { get; set; } = 200;

    public int VisibleStateDuration { get; set; } = 70000;

    public int HideTransitionDuration { get; set; } = 200;

    public bool ShowCloseIcon { get; set; } = true;

    public bool AutoHide { get; set; } = true;

    public bool ClearAfterNavigation { get; set; } = false;
}
