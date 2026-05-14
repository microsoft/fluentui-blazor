// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial interface IDialogService
{
    /// <summary>
    /// Shows the global overlay.
    /// </summary>
    /// <param name="options">Options used to configure the overlay.</param>
    Task ShowOverlayAsync(Action<OverlayOptions>? options = null);

    /// <summary>
    /// Hides the global overlay.
    /// </summary>
    Task HideOverlayAsync();
}
