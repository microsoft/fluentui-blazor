// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The data to show in a splash screen dialog.
/// </summary>
public class SplashScreenContent
{
    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the subtitle.
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

    /// <summary>
    /// Gets or sets the delay to wait before to close the dialog (in milliseconds).
    /// </summary>
    public int DisplayTime { get; set; } = 4000;

    /// <summary>
    /// Updates the labels of the splash screen.
    /// </summary>
    public void UpdateLabels(string? loadingText = null, MarkupString? message = null)
    {
        if (loadingText is not null)
        {
            LoadingText = loadingText;
        }

        if (message is not null)
        {
            Message = message;
        }

        RefreshProperties?.Invoke();
    }

    /// <summary>
    /// Gets or sets the callback used by the host component to refresh the splash screen after updates.
    /// </summary>
    public Action? RefreshProperties { get; set; }
}
