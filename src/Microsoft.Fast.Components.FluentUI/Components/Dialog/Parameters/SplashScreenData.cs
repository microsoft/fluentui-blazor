using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// The data to show in a splash screen dialog.
/// </summary>
public class SplashScreenData
{
    /// <summary>
    /// Typically used to show the name of the product.
    /// </summary>
    public string? Title { get; set; }
    /// <summary>
    /// Typically used to show the name of the suite the product belongs to.
    /// </summary>
    public string? SubTitle { get; set; }

    /// <summary>
    /// Text to indicate something is happening.
    /// </summary>
    public string? LoadingText { get; set; }

    /// <summary>
    /// An extra message. Can contain HTML. 
    /// </summary>
    public MarkupString? Message { get; set; }

    /// <summary>
    /// Logo to show on the splash screen.
    /// Can be a URL or a base64 encoded string or an SVG.
    /// </summary>
    public string? Logo { get; set; }
}
