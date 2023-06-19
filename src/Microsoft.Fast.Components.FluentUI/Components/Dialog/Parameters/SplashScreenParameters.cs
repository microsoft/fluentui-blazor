using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public class SplashScreenParameters : DialogParameters, ISplashScreenParameters
{
    [Parameter]
    public string? SubTitle { get; set; }

    [Parameter]
    public string? LoadingText { get; set; }

    [Parameter]
    public MarkupString? Message { get; set; }

    [Parameter]
    public string? Logo { get; set; }

    [Parameter]
    public string? Width { get; set; }

    [Parameter]
    public string? Height { get; set; }
}
