// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0048:File name must match type name", Justification = "To write builder methods in a separate file and keep the main class file clean.")]
public partial class LabelInfo : ILabelInfo
{
    /// <summary>
    /// Creates a new <see cref="LabelInfo"/> instance with the specified informational text.
    /// </summary>
    /// <param name="text">The informational text displayed inside the popover.</param>
    /// <returns>A new <see cref="LabelInfo"/> instance.</returns>
    public static LabelInfo WithText(string text) => new() { InfoText = text };

    /// <summary>
    /// Sets the URL displayed as a "learn more" link inside the popover.
    /// </summary>
    /// <param name="actionLink">The URL displayed as a "learn more" link inside the popover.</param>
    /// <returns>The current <see cref="LabelInfo"/> instance.</returns>
    public LabelInfo WithActionLink(string? actionLink)
    {
        InfoActionLink = actionLink;
        return this;
    }

    /// <summary>
    /// Sets the text displayed as a "learn more" link inside the popover.
    /// </summary>
    /// <param name="actionText">The text displayed as a "learn more" link inside the popover.</param>
    /// <returns>The current <see cref="LabelInfo"/> instance.</returns>
    public LabelInfo WithActionText(string? actionText)
    {
        InfoActionText = actionText;
        return this;
    }

    /// <summary>
    /// Sets the target for the "learn more" link inside the popover.
    /// </summary>
    /// <param name="actionTarget">The target for the "learn more" link inside the popover.</param>
    /// <returns>The current <see cref="LabelInfo"/> instance.</returns>
    public LabelInfo WithActionTarget(LinkTarget actionTarget)
    {
        InfoActionTarget = actionTarget;
        return this;
    }
}
