// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Options for configuring a splash screen.
/// </summary>
public class SplashScreenOptions : DialogOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SplashScreenOptions"/> class.
    /// </summary>
    public SplashScreenOptions()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SplashScreenOptions"/> class
    /// using the specified implementation factory.
    /// </summary>
    /// <param name="implementationFactory">Action used to configure the splash screen options.</param>
    public SplashScreenOptions(Action<SplashScreenOptions> implementationFactory)
    {
        implementationFactory.Invoke(this);
    }

    /// <summary>
    /// Gets or sets the text to indicate something is happening.
    /// </summary>
    public string? LoadingText { get; set; }

    /// <summary>
    /// Gets or sets the logo to show on the splash screen.
    /// Can be a URL or a base64 encoded string or an SVG.
    /// </summary>
    public string? Logo { get; set; }

    /// <summary>
    /// Gets or sets an extra message. Can contain HTML.
    /// </summary>
    public MarkupString? Message { get; set; }

    /// <summary>
    /// Gets or sets the subtitle.
    /// Typically used to show the name of the suite the product belongs to.
    /// </summary>
    public string? SubTitle { get; set; }

    /// <summary>
    /// Gets or sets the title.
    /// Typically used to show the name of the product.
    /// </summary>
    public string? Title { get; set; }
}
