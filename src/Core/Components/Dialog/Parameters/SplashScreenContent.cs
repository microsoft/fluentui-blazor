using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The data to show in a splash screen dialog.
/// </summary>
public class SplashScreenContent
{
    /// <summary>
    /// Gets or sets the title.
    /// Typically used to show the name of the product.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the subtitle.
    /// Typically used to show the name of the suite the product belongs to.
    /// </summary>
    public string? SubTitle { get; set; }

    /// <summary>
    /// Gets or sets the text to indicate something is happening.
    /// </summary>
    public string? LoadingText { get; set; }

    /// <summary>
    /// Gets or sets an extra message. Can contain HTML. 
    /// </summary>
    public MarkupString? Message { get; set; }

    /// <summary>
    /// Gets or sets the logo to show on the splash screen.
    /// Can be a URL or a base64 encoded string or an SVG.
    /// </summary>
    public string? Logo { get; set; }
}
