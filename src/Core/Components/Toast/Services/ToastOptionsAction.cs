// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Options for configuring a toast action displayed by the <see cref="INotificationService"/>.
/// </summary>
public class ToastOptionsAction
{
    /// <summary>
    /// Gets or sets the label for the action button.
    /// </summary>
    public string? Label { get; set; }

    /// <summary>
    /// Gets or sets the tooltip for the action button.
    /// </summary>
    public string? Tooltip { get; set; }

    /// <summary>
    /// Gets or sets the callback to invoke when the action button is clicked.
    /// </summary>
    public Func<ToastEventArgs, Task>? CallbackAsync { get; set; }
}