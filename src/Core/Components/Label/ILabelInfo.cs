// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Defines the informational content displayed inside the popover of a
/// <see cref="FluentLabelInfo"/> component.
/// </summary>
public interface ILabelInfo
{
    /// <summary>
    /// Gets or sets the informational text displayed inside the popover when the user
    /// clicks the info icon.
    /// </summary>
    string? InfoText { get; set; }

    /// <summary>
    /// Gets or sets the URL displayed as a "learn more" link inside the popover.
    /// </summary>
    string? InfoActionLink { get; set; }

    /// <summary>
    /// Gets or sets the text displayed as a "learn more" link inside the popover.
    /// </summary>
    string? InfoActionText { get; set; }

    /// <summary>
    /// Gets or sets the target for the "learn more" link inside the popover.
    /// Defaults to <see cref="LinkTarget.Blank"/> to open the link in a new tab.
    /// </summary>
    LinkTarget InfoActionTarget { get; set; }

    /// <summary>
    /// Gets or sets the maximum width of the info popover. Can be any valid CSS width value, such as "200px" or "50%".
    /// If not set, the popover will size to fit its content (`fit-content`).
    /// </summary>
    string? MaxWidth { get; set; }
}
