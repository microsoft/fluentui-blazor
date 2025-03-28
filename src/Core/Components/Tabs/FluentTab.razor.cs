// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// </summary>
public partial class FluentTab : FluentComponentBase
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
    /// Gets or sets whether the tab content is rendered only when the tab is selected.
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

    /// <summary />
    protected override async Task OnInitializedAsync()
    {
        if (Owner is not null)
        {
            await Owner.AddTabAsync(this);
        }
    }
}
