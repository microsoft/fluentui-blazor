using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public class SplashScreenParameters : DialogParameters, ISplashScreenParameters
{
    public string? SubTitle { get; set; }
    public string? LoadingText { get; set; }
    public MarkupString? Message { get; set; }
    public string? Logo { get; set; }
    public string? Width { get; set; }
    public string? Height { get; set; }
}
