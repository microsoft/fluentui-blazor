// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Default implementation of <see cref="ILabelInfo"/>, describing the
/// informational content displayed inside the popover of a
/// <see cref="FluentLabelInfo"/> component.
/// </summary>
public partial class LabelInfo : ILabelInfo
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LabelInfo"/> class with default values.
    /// </summary>
    public LabelInfo() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="LabelInfo"/> class with the specified informational content.
    /// </summary>
    /// <param name="text">The informational text displayed inside the popover.</param>
    /// <param name="actionLink">The URL displayed as a "learn more" link inside the popover.</param>
    /// <param name="actionText">The text displayed as a "learn more" link inside the popover.</param>
    /// <param name="actionTarget">The target for the "learn more" link inside the popover.</param>
    /// <param name="maxWidth">The maximum width of the info popover. Can be any valid CSS width value, such as "200px" or "50%".</param>
    public LabelInfo(string text, string? actionLink = null, string? actionText = null, LinkTarget actionTarget = LinkTarget.Blank, string? maxWidth = null)
    {
        InfoText = text;
        InfoActionLink = actionLink;
        InfoActionText = actionText;
        InfoActionTarget = actionTarget;
        MaxWidth = maxWidth;
    }

    /// <inheritdoc />
    public string? InfoText { get; set; }

    /// <inheritdoc />
    public string? InfoActionLink { get; set; }

    /// <inheritdoc />
    public string? InfoActionText { get; set; }

    /// <inheritdoc />
    public LinkTarget InfoActionTarget { get; set; } = LinkTarget.Blank;

    /// <inheritdoc />
    public string? MaxWidth { get; set; }
}
