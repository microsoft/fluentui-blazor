using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public interface ISplashScreenParameters
{
    string? Title { get; set; }
    string? SubTitle { get; set; }
    string? LoadingText { get; set; }
    MarkupString? Message { get; set; }
    string? Logo { get; set; }

    string? Width { get; set; }
    string? Height { get; set; }
}