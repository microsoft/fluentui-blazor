// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Presents a setting, similar to the tiles in the Windows 11 settings app, by separating its informational content,
/// main action and optional expandable panel.
/// When <see cref="ChildContent"/> is provided, an expand/collapse button is displayed.
/// The panel can contain nested <see cref="FluentSettingsExpander"/> components.
/// </summary>
public partial class FluentSettingsExpander : FluentComponentBase
{
    private readonly string _panelId = Identifier.NewId();

    /// <summary />
    public FluentSettingsExpander(LibraryConfiguration configuration) : base(configuration) { }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-settings-expander")
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .Build();

    /// <summary />
    private bool IsExpandable => ChildContent is not null;

    /// <summary />
    private bool HasLink => !Disabled && !string.IsNullOrEmpty(Href);

    /// <summary />
    private bool HasTitle => !string.IsNullOrEmpty(Title);

    /// <summary />
    private bool HasDescription => DescriptionTemplate is not null || !string.IsNullOrEmpty(Description);

    /// <summary />
    private string TitleId => $"{_panelId}-title";

    /// <summary />
    private string DescriptionId => $"{_panelId}-description";

    /// <summary />
    private string PanelId => $"{_panelId}-panel";

    /// <summary />
    private string? AccessibleName => string.IsNullOrWhiteSpace(ExpandButtonLabel) ? Title : ExpandButtonLabel;

    /// <summary>
    /// Gets or sets the optional leading icon (on the left in LTR, on the right in RTL).
    /// </summary>
    [Parameter]
    public Icon? Icon { get; set; }

    /// <summary>
    /// Gets or sets the setting title.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the optional description displayed below the title.
    /// </summary>
    [Parameter]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the optional description content displayed below the title.
    /// It takes precedence over <see cref="Description"/>.
    /// </summary>
    [Parameter]
    public RenderFragment? DescriptionTemplate { get; set; }

    /// <summary>
    /// Gets or sets the trailing action (e.g. a switch, button or dropdown).
    /// </summary>
    [Parameter]
    public RenderFragment? ActionTemplate { get; set; }

    /// <summary>
    /// Gets or sets the expandable panel content, including nested settings expanders.
    /// When set, the expand/collapse button is displayed.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets whether the panel is expanded. Defaults to false.
    /// </summary>
    [Parameter]
    public bool Expanded { get; set; }

    /// <summary>
    /// Raised when the <see cref="Expanded"/> state changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> ExpandedChanged { get; set; }

    /// <summary>
    /// Gets or sets whether the component is disabled. Defaults to false.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets the accessible name of the expand/collapse button.
    /// When not set, the <see cref="Title"/> is used.
    /// </summary>
    [Parameter]
    public string? ExpandButtonLabel { get; set; }

    /// <summary>
    /// Gets or sets a URL. When set, the whole header becomes a link and an "open" icon is displayed at the end.
    /// </summary>
    [Parameter]
    public string? Href { get; set; }

    /// <summary>
    /// Gets or sets the link target (e.g. <c>_blank</c>) used with <see cref="Href"/>.
    /// </summary>
    [Parameter]
    public string? Target { get; set; }

    /// <summary />
    private static void NoOp()
    {
    }

    /// <summary />
    private async Task HeaderClickAsync(MouseEventArgs args)
    {
        if (IsExpandable && !HasLink)
        {
            await ToggleAsync(args);
        }
    }

    /// <summary />
    private async Task ToggleAsync(MouseEventArgs args)
    {
        if (Disabled)
        {
            return;
        }

        Expanded = !Expanded;
        await ExpandedChanged.InvokeAsync(Expanded);
    }
}
