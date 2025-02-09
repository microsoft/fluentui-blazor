// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The grid component helps keeping layout consistent across various screen resolutions and sizes.
/// PowerGrid comes with a 12-point grid system and contains 5 types of breakpoints
/// that are used for specific screen sizes.
/// </summary>
public partial class FluentGrid : FluentComponentBase
{
    private const string JAVASCRIPT_FILE = FluentJSModule.JAVASCRIPT_ROOT + "Grid/FluentGrid.razor.js";

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentGrid"/> class.
    /// </summary>
    public FluentGrid()
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
    /// Event raised when page size falls within a specific size range (xs, sm, md, lg, xl, xxl).
    /// </summary>
    [Parameter]
    public EventCallback<GridItemSize> OnBreakpointEnter { get; set; }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && OnBreakpointEnter.HasDelegate)
        {
            // Import the JavaScript module
            var jsModule = await JSModule.ImportJavaScriptModuleAsync(JAVASCRIPT_FILE);

            // Call a function from the JavaScript module
            var dotNetHelper = DotNetObjectReference.Create(this);
            await jsModule.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Grid.FluentGridInitialize", Id, dotNetHelper);
        }
    }

    /// <summary />
    [JSInvokable]
    public async Task FluentGrid_MediaChangedAsync(string size)
    {
        var valid = Enum.TryParse<GridItemSize>(size, ignoreCase: true, out var sizeEnum);
        CurrentSize = valid ? sizeEnum : null;

        if (OnBreakpointEnter.HasDelegate)
        {
            if (valid)
            {
                await OnBreakpointEnter.InvokeAsync(sizeEnum);
            }
        }
    }

    /// <summary>
    /// <inheritdoc cref="IAsyncDisposable.DisposeAsync" />
    /// </summary>
    /// <returns></returns>
    [ExcludeFromCodeCoverage(Justification = "Tested via integration tests.")]
    protected override async ValueTask DisposeAsync(IJSObjectReference? jsModule)
    {
        if (jsModule != null)
        {
            await jsModule.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Grid.FluentGridCleanup", Id);
        }
    }
}
