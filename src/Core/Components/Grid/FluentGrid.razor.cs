// --------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
// --------------------------------------------------------------
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
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

    public FluentGrid()
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    [Inject]
    private LibraryConfiguration LibraryConfiguration { get; set; } = default!;

    /// <summary />
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary />
    private IJSObjectReference? _jsModule { get; set; }

    /// <summary />
    internal GridItemSize? CurrentSize { get; private set; }

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
    /// Gets or sets the adaptive rendering, which not render the HTML code when the item is hidden (true) or only hide the item by CSS (false).
    /// Default is false.
    /// </summary>
    [Parameter]
    public bool AdaptiveRendering { get; set; } = false;

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
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder(Style)
        .Build();

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && OnBreakpointEnter.HasDelegate)
        {
            _jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE.FormatCollocatedUrl(LibraryConfiguration));
            DotNetObjectReference<FluentGrid> dotNetHelper = DotNetObjectReference.Create(this);
            await _jsModule.InvokeVoidAsync("FluentGridInitialize", Id, dotNetHelper);
        }
    }

    [JSInvokable]
    public async Task FluentGrid_MediaChangedAsync(string size)
    {
        bool valid = Enum.TryParse<GridItemSize>(size, ignoreCase: true, out var sizeEnum);
        CurrentSize = valid ? sizeEnum : null;

        if (OnBreakpointEnter.HasDelegate)
        {
            if (valid)
            {
                await OnBreakpointEnter.InvokeAsync(sizeEnum);
            }
        }
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_jsModule is not null)
            {
                await _jsModule.InvokeVoidAsync("FluentGridCleanup", Id);
                await _jsModule.DisposeAsync();
            }
        }
        catch (Exception ex) when (ex is JSDisconnectedException ||
                                   ex is OperationCanceledException)
        {
            // The JSRuntime side may routinely be gone already if the reason we're disposing is that
            // the client disconnected. This is not an error.
        }
    }
}
