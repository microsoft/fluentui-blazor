// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A settings card, similar to the tiles in the Windows 11 settings app.
/// It shows an icon, a header, a description and an action area (e.g. a dropdown or slider).
/// When <see cref="ChildContent"/> is provided, the card becomes expandable and
/// the content is displayed below the header (typically as nested <see cref="FluentSettingsCard"/> rows).
/// </summary>
public partial class FluentSettingsCard : FluentComponentBase
{
    /// <summary />
    public FluentSettingsCard(LibraryConfiguration configuration) : base(configuration) { }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-settings-card")
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .Build();

    /// <summary />
    private bool IsExpandable => ChildContent is not null;

    /// <summary>
    /// Gets or sets the icon displayed at the start of the card.
    /// </summary>
    [Parameter]
    public Icon? Icon { get; set; }

    /// <summary>
    /// Gets or sets the header text.
    /// </summary>
    [Parameter]
    public string? Header { get; set; }

    /// <summary>
    /// Gets or sets a URL. When set, the whole card becomes a link and an "open" icon is displayed at the end.
    /// </summary>
    [Parameter]
    public string? Href { get; set; }

    /// <summary>
    /// Gets or sets the link target (e.g. <c>_blank</c>) used with <see cref="Href"/>.
    /// </summary>
    [Parameter]
    public string? Target { get; set; }

    /// <summary>
    /// Gets or sets custom content (e.g. a checkbox) displayed instead of <see cref="Header"/>.
    /// </summary>
    [Parameter]
    public RenderFragment? HeaderTemplate { get; set; }

    /// <summary>
    /// Gets or sets the description displayed below the header.
    /// </summary>
    [Parameter]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the content displayed at the end of the header (e.g. a dropdown, switch or slider).
    /// </summary>
    [Parameter]
    public RenderFragment? Action { get; set; }

    /// <summary>
    /// Gets or sets the expandable content. When set, a chevron is displayed to toggle it.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets whether the expandable content is visible.
    /// </summary>
    [Parameter]
    public bool Expanded { get; set; }

    /// <summary>
    /// Raised when the <see cref="Expanded"/> state changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> ExpandedChanged { get; set; }

    /// <summary />
    private async Task ToggleAsync()
    {
        Expanded = !Expanded;
        await ExpandedChanged.InvokeAsync(Expanded);
    }
}
