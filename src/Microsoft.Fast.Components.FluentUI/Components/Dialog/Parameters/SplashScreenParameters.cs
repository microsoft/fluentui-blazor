using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// The set of parameters for a splash screen dialog.
/// </summary>
public class SplashScreenParameters : ISplashScreenParameters
{
    /// <summary>
    /// The title of the splash screen.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// The subtitle of the splash screen.
    /// </summary>
    public string? SubTitle { get; set; }

    /// <summary>
    /// The text to display while loading.
    /// </summary>
    public string? LoadingText { get; set; }

    /// <summary>
    /// Extra message to display. Supports HTML.
    /// </summary>
    public MarkupString? Message { get; set; }

    /// <summary>
    /// Logo to show on the splash screen.
    /// Can be a URL or a base64 encoded string or an SVG.
    /// </summary>
    public string? Logo { get; set; }

    /// <summary>
    /// The width of the splash screen.
    /// Must be a valid CSS width string like "600px" or "3em".
    /// </summary>
    public string? Width { get; set; }

    /// <summary>
    /// The height of the splash screen.
    /// Must be a valid CSS height string like "600px" or "3em".
    /// </summary>
    public string? Height { get; set; }
}
