// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A FluentTabs allows people to switch between categories of related information without going to different pages
/// </summary>
public partial class FluentTab : FluentComponentBase, ITooltipComponent
{
    /// <summary />
    public FluentTab()
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .Build();

    /// <summary />
    [CascadingParameter]
    private FluentTabs? Owner { get; set; }

    /// <summary />
    internal int Index { get; set; }

    /// <summary>
    /// Gets or sets whether the tab is disabled.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets the label of the tab.
    /// </summary>
    [Parameter]
    public string? Header { get; set; }

    /// <summary>
    /// Gets or sets the header content of the tab.
    /// </summary>
    [Parameter]
    public RenderFragment? HeaderTemplate { get; set; }

    /// <summary>
    /// Gets or sets the icon to be displayed at the start of the tab.
    /// </summary>
    [Parameter]
    public Icon? IconStart { get; set; }

    /// <summary>
    /// Gets or sets the icon color.
    /// </summary>
    [Parameter]
    public Color? IconColor { get; set; }

    /// <summary>
    /// Gets or sets whether the tab content is rendered only when the tab is selected.
    /// To reduce the HTML page size, the tab content is cleared when the tab is unselected.
    /// </summary>
    [Parameter]
    public bool DeferredLoading { get; set; } = false;

    /// <summary>
    /// Gets or sets the customized loading content message when using deferred loading.
    /// </summary>
    [Parameter]
    public RenderFragment? LoadingTemplate { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets whether the tab is displayed. Default is true.
    /// </summary>
    [Parameter]
    public bool Visible { get; set; } = true;

    /// <inheritdoc cref="ITooltipComponent.Tooltip" />
    [Parameter]
    public string? Tooltip { get; set; }

    /// <summary />
    protected override async Task OnInitializedAsync()
    {
        await base.RenderTooltipAsync(Tooltip);

        if (Owner is not null)
        {
            Index = await Owner.AddTabAsync(this);
        }
    }

    /// <summary />
    internal string TabPanelId => $"{Id}-panel";
}
