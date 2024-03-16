// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentProfileMenu : FluentComponentBase
{
    public FluentProfileMenu()
    {
        Id = Identifier.NewId();
    }

    private bool PopoverVisible { get; set; } = false;

    /// <summary>
    /// Gets or sets the content to be displayed in the header section of the popover.
    /// Using this property will override the <see cref="CompanyName" /> and <see cref="SignOutLabel"/> properties.
    /// </summary>
    [Parameter]
    public RenderFragment? HeaderTemplate { get; set; }

    /// <summary>
    /// Gets or sets the content to be displayed in the main section (body) of the popover.
    /// Using this property will override the <see cref="EMail"/>, <see cref="FullName"/>, <see cref="Image"/> properties.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the content to be displayed in the footer section of the popover.
    /// Using this property will override the <see cref="ViewAccountLabel" /> property.
    /// </summary>
    [Parameter]
    public RenderFragment? FooterTemplate { get; set; }

    /// <summary>
    /// Gets or sets the status to show. See <see cref="PresenceStatus"/> for options.
    /// </summary>
    [Parameter]
    public PresenceStatus? Status { get; set; }

    /// <summary>
    /// Gets or sets the title to show on hover. If not provided, the status will be used.
    /// </summary>
    [Parameter]
    public string? StatusTitle { get; set; }

    /// <summary>
    /// Gets or sets the initials to display if no image is provided.
    /// By default, the first letters of the <see cref="FullName"/> is used.
    /// </summary>
    [Parameter]
    public string? Initials { get; set; }

    /// <summary>
    /// Gets or sets the name to display.
    /// </summary>
    [Parameter]
    public string? FullName { get; set; }

    /// <summary>
    /// Gets or sets the company name to display.
    /// </summary>
    [Parameter]
    public string? CompanyName { get; set; }

    /// <summary>
    /// Gets or sets the email to display.
    /// </summary>
    [Parameter]
    public string? EMail { get; set; }

    /// <summary>
    /// Gets or sets the image to display, in replacement of the initials.
    /// </summary>
    [Parameter]
    public string? Image { get; set; }

    /// <summary>
    /// Gets or sets the size of the image, in the popover.
    /// </summary>
    [Parameter]
    public string? ImageSize { get; set; } = "64px";

    /// <summary>
    /// Gets or sets the size of the main button image (button clickable to display the popover).
    /// </summary>
    [Parameter]
    public string? ButtonSize { get; set; } = "32px";

    /// <summary>
    /// Gets or sets the "Sign out" button label.
    /// </summary>
    [Parameter]
    public string? SignOutLabel { get; set; } = "Sign out";

    /// <summary>
    /// Event raised when the user clicks on the "Sign out" button.
    /// </summary>
    [Parameter]
    public EventCallback OnSignOut { get; set; }

    /// <summary>
    /// Gets or sets the "View account" hyperlink label.
    /// </summary>
    [Parameter]
    public string? ViewAccountLabel { get; set; } = "View account";

    /// <summary>
    /// Event raised when the user clicks on the "View account" link.
    /// </summary>
    [Parameter]
    public EventCallback OnViewAccount { get; set; }
}
