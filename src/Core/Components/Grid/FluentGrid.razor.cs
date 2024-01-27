// --------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
// --------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The grid component helps keeping layout consistent across various screen resolutions and sizes.
/// PowerGrid comes with a 12-point grid system and contains 5 types of breakpoints
/// that are used for specific screen sizes.
/// </summary>
public partial class FluentGrid : FluentComponentBase, IAsyncDisposable
{
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/Grid/FluentGrid.razor.js";
    private DotNetObjectReference<FluentGrid>? _dotNetHelper = null;

    public FluentGrid()
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary />
    private IJSObjectReference? Module { get; set; }

    /// <summary>
    /// Gets or sets the distance between flexbox items, using a multiple of 4px.
    /// Only values from 0 to 10 are possible.
    /// </summary>
    [Parameter]
    public int Spacing { get; set; } = 3;

    /// <summary>
    /// Defines how the browser distributes space between and around content items.
    /// </summary>
    [Parameter]
    public JustifyContent Justify { get; set; } = JustifyContent.FlexStart;

    /// <summary>
    /// Gets or sets the child content of component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// when page size falls within a specific size range (xs, sm, md, lg, xl, xxl).
    /// </summary>
    [Parameter]
    public EventCallback<GridItemSize> OnBreakpointEnter { get; set; }

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass($"fluent-grid")
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder(Style)
        .AddStyle("justify-content", Justify.ToAttributeValue())
        .Build();

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && OnBreakpointEnter.HasDelegate)
        {
            Module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
            _dotNetHelper = DotNetObjectReference.Create(this);
            await Module.InvokeVoidAsync("FluentGridInitialize", Id, _dotNetHelper);
        }
    }

    [JSInvokable]
    public async Task FluentGrid_MediaChangedAsync(string size)
    {
        if (OnBreakpointEnter.HasDelegate)
        {
            if (Enum.TryParse<GridItemSize>(size, ignoreCase: true, out var sizeEnum))
            {
                await OnBreakpointEnter.InvokeAsync(sizeEnum);
            }
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (Module != null)
        {
            await Module.InvokeVoidAsync("FluentGridCleanup", Id, _dotNetHelper);
        }
    }
}
