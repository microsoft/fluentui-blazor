// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentProfileMenu : FluentComponentBase
{
    public FluentProfileMenu()
    {
        Id = Identifier.NewId();
    }

    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-profile-menu")
        .Build();

    protected string? StyleValue => new StyleBuilder(Style)
        .Build();

    private bool PopoverVisible { get; set; } = false;

    /// <summary>
    /// Gets or sets whether popover should be forced to top right or top left (RTL).
    /// </summary>
    [Parameter]
    public bool TopCorner { get; set; } = false;

    /// <summary>
    /// Gets or sets the content to be displayed in the header section of the popover.
    /// Using this property will override the <see cref="HeaderLabel" /> and <see cref="HeaderButton"/> properties.
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
    /// Using this property will override the <see cref="FooterLink" /> property.
    /// </summary>
    [Parameter]
    public RenderFragment? FooterTemplate { get; set; }

    /// <summary>
    /// Gets or sets the content to be displayed in the start (left) section of the Profile button.
    /// </summary>
    [Parameter]
    public RenderFragment? StartTemplate { get; set; }

    /// <summary>
    /// Gets or sets the content to be displayed in the end (right) section of the Profile button.
    /// </summary>
    [Parameter]
    public RenderFragment? EndTemplate { get; set; }

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
    /// Gets or sets the Class to apply to the Profile Popup.
    /// </summary>
    [Parameter]
    public virtual string? PopoverClass { get; set; } = null;

    /// <summary>
    /// Gets or sets the Style to apply to the Profile Popup.
    /// </summary>
    [Parameter]
    public virtual string? PopoverStyle { get; set; } = null;

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
    /// Gets or sets the email to display.
    /// </summary>
    [Parameter]
    public string? EMail { get; set; }

    /// <summary>
    /// Gets or sets the header label (e.g Company Name) to display on the top-left.
    /// </summary>
    [Parameter]
    public string? HeaderLabel { get; set; }

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
    /// Gets or sets the Header Button label (e.g. Sign out) on the top-right.
    /// </summary>
    [Parameter]
    public string? HeaderButton { get; set; } = "Sign out";

    /// <summary>
    /// Event raised when the user clicks on the Header Button (e.g. Sign out).
    /// </summary>
    [Parameter]
    public EventCallback OnHeaderButtonClick { get; set; }

    /// <summary>
    /// Gets or sets the footer label to display on the bottom-left.
    /// </summary>
    [Parameter]
    public string? FooterLabel { get; set; }

    /// <summary>
    /// Gets or sets the Footer hyperlink label (e.g. View account) on the bottom-right.
    /// </summary>
    [Parameter]
    public string? FooterLink { get; set; } = "View account";

    /// <summary>
    /// Event raised when the user clicks on the Footer hyperlink (e.g. View account).
    /// </summary>
    [Parameter]
    public EventCallback OnFooterLinkClick { get; set; }

    /// <summary />
    private string PersonaId => $"{Id}-persona";
}
