// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a popup component for the profile menu.
/// </summary>
public partial class FluentProfilePopover : FluentComponentBase
{
    /// <summary />
    public FluentProfilePopover(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-profile-popover")
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("width", Width)
        .AddStyle("height", Height)
        .Build();

    /// <summary />
    protected string? RegionRole =>
        HasAccessibleName
            ? "region"
            : null;

    /// <summary>
    /// Gets or sets the width of the popover.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the popover.
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Gets or sets the image displayed by the profile menu avatars.
    /// </summary>
    [Parameter]
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Gets or sets the email address displayed in the profile menu.
    /// </summary>
    [Parameter]
    public string? EMail { get; set; }
    
    /// <summary>
    /// Gets or sets the full name displayed in the profile menu.
    /// </summary>
    [Parameter]
    public string? FullName { get; set; }

    /// <summary>
    /// Gets or sets the initials displayed when no image is available.
    /// </summary>
    [Parameter]
    public string? ImageInitials { get; set; }

    /// <summary>
    /// Gets or sets the size of the avatar displayed in the popover.
    /// </summary>
    [Parameter]
    public AvatarSize? ImageSize { get; set; }

    /// <summary>
    /// Gets or sets custom content for the header section of the popover.
    /// </summary>
    [Parameter]
    public RenderFragment? HeaderTemplate { get; set; }

    /// <summary>
    /// Gets or sets the label displayed in the header section of the popover.
    /// </summary>
    [Parameter]
    public string? HeaderLabel { get; set; }

    /// <summary>
    /// Gets or sets the label of the link displayed in the header section of the popover.
    /// </summary>
    [Parameter]
    public string? HeaderLinkLabel { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when the header link is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnHeaderLinkClick { get; set; }

    /// <summary>
    /// Gets or sets custom content for the footer section of the popover.
    /// </summary>
    [Parameter]
    public RenderFragment? FooterTemplate { get; set; }

    /// <summary>
    /// Gets or sets the label displayed in the footer section of the popover.
    /// </summary>
    [Parameter]
    public string? FooterLabel { get; set; }

    /// <summary>
    /// Gets or sets the label of the link displayed in the footer section of the popover.
    /// </summary>
    [Parameter]
    public string? FooterLinkLabel { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when the footer link is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnFooterLinkClick { get; set; }

    /// <summary>
    /// Gets or sets the content displayed in the main section of the popover.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private bool HasAccessibleName =>
        !string.IsNullOrWhiteSpace(FullName) ||
        HasNonEmptyAttribute("aria-label") ||
        HasNonEmptyAttribute("aria-labelledby");

    private bool HasNonEmptyAttribute(string name)
    {
        return AdditionalAttributes?.TryGetValue(name, out var value) is true &&
               value is string text &&
               !string.IsNullOrWhiteSpace(text);
    }
}