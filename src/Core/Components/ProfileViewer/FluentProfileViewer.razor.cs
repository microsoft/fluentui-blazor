// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentProfileViewer : FluentComponentBase
{
    public FluentProfileViewer()
    {
        Id = Identifier.NewId();
    }

    private bool PopoverVisible { get; set; } = false;

    [Parameter]
    public RenderFragment? HeaderTemplate { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

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

    [Parameter]
    public string? Initials { get; set; }

    [Parameter]
    public string? FullName { get; set; }

    [Parameter]
    public string? CompanyName { get; set; }

    [Parameter]
    public string? EMail { get; set; }

    [Parameter]
    public string? Image { get; set; }

    [Parameter]
    public string? ImageSize { get; set; } = "64px";

    [Parameter]
    public string? ButtonSize { get; set; } = "32px";

    [Parameter]
    public string? SignOutLabel { get; set; } = "Sign out";

    [Parameter]
    public EventCallback OnSignOut { get; set; }

    [Parameter]
    public string? ViewAccountLabel { get; set; } = "View Account";

    [Parameter]
    public EventCallback OnViewAccount { get; set; }
}
