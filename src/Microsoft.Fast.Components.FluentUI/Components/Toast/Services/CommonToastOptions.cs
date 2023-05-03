namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public abstract class CommonToastOptions
{

    public int ShowTransitionDuration { get; set; } = 200;

    public int VisibleStateDuration { get; set; } = 7000;

    public int HideTransitionDuration { get; set; } = 200;

    public bool ShowCloseIcon { get; set; } = true;

    public bool AutoHide { get; set; } = true;

    protected CommonToastOptions() { }

    protected CommonToastOptions(CommonToastOptions options)
    {
        ShowTransitionDuration = options.ShowTransitionDuration;
        VisibleStateDuration = options.VisibleStateDuration;
        HideTransitionDuration = options.HideTransitionDuration;
        ShowCloseIcon = options.ShowCloseIcon;
        AutoHide = options.AutoHide;
    }
}
