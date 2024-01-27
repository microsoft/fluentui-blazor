// --------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
// --------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// This component is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
/// Add this line to suppress de compilation error: `@{ #pragma warning disable FluentMultiSplitter }`
/// </summary>
[Experimental("FluentMultiSplitter", UrlFormat = "https://preview.fluentui-blazor.net/MultiSplitter")]
public partial class FluentMultiSplitter : FluentComponentBase
{
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/Splitter/FluentMultiSplitter.razor.js";
    private DotNetObjectReference<FluentMultiSplitter>? _objRef = null;

    internal List<FluentMultiSplitterPane> Panes { get; } = new();

    /// <summary />
    public FluentMultiSplitter()
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the child content.
    /// </summary>
    /// <value>The child content.</value>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the collapse callback.
    /// </summary>
    /// <value>The collapse callback.</value>
    [Parameter]
    public EventCallback<FluentMultiSplitterEventArgs> OnCollapse { get; set; }

    /// <summary>
    /// Gets or sets the expand callback.
    /// </summary>
    /// <value>The expand callback.</value>
    [Parameter]
    public EventCallback<FluentMultiSplitterEventArgs> OnExpand { get; set; }

    /// <summary>
    /// Gets or sets the resize callback.
    /// </summary>
    /// <value>The resize callback.</value>
    [Parameter]
    public EventCallback<FluentMultiSplitterResizeEventArgs> OnResize { get; set; }

    /// <summary>
    /// Gets or sets the size of the splitter bar in pixels. Default is 8
    /// </summary>
    [Parameter]
    public string? BarSize { get; set; }

    /// <summary>
    /// Gets or sets the orientation.
    /// </summary>
    /// <value>The orientation.</value>
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Horizontal;

    /// <summary>
    /// Gets or sets the width of the container.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the container.
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-multi-splitter")
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder(Style)
        .AddStyle("height", Height, () => !string.IsNullOrEmpty(Height))
        .AddStyle("width", Width, () => !string.IsNullOrEmpty(Width))
        .AddStyle("--fluent-multi-splitter-bar-size", BarSize, () => !string.IsNullOrEmpty(BarSize))
        .Build();

    /// <summary />
    [Inject]
    private IJSRuntime JS { get; set; } = default!;

    /// <summary />
    private IJSObjectReference Module { get; set; } = default!;

    /// <summary>
    /// Adds the pane.
    /// </summary>
    /// <param name="pane">The pane.</param>
    public void AddPane(FluentMultiSplitterPane pane)
    {
        // Add this pane if not already done
        if (Panes.IndexOf(pane) < 0)
        {
            pane.Index = Panes.Count;
            Panes.Add(pane);

            var nbSizeAutoPanes = Panes.Count(i => i.SizeAuto);
            foreach (var item in Panes)
            {
                if (item.SizeAuto)
                {
                    item.SizeRuntime = (100 / nbSizeAutoPanes) + "%";
                }
            }
        }
    }

    /// <summary>
    /// Called when pane resized (internal method).
    /// </summary>
    /// <param name="paneIndex">Index of the pane.</param>
    /// <param name="sizeNew">The size new.</param>
    /// <param name="paneNextIndex">Index of the pane next.</param>
    /// <param name="sizeNextNew">The size next new.</param>
    [JSInvokable("FluentMultiSplitter.OnPaneResizedAsync")]
    public async Task OnPaneResizedAsync(int paneIndex, double sizeNew, int? paneNextIndex, double? sizeNextNew)
    {
        // Current panel
        var pane = Panes[paneIndex];

        if (OnResize.HasDelegate)
        {
            var arg = new FluentMultiSplitterResizeEventArgs()
            {
                PaneIndex = pane.Index,
                Pane = pane,
                NewSize = sizeNew,
            };

            await OnResize.InvokeAsync(arg);

            if (arg.Cancel)
            {
                // TODO: to simplify
                var oldSize = pane.SizeRuntime;
                pane.SizeRuntime = "0";
                await InvokeAsync(StateHasChanged);
                pane.SizeRuntime = oldSize;
                await InvokeAsync(StateHasChanged);
                return;
            }
        }

        pane.SizeRuntime = sizeNew.ToString("0.00", CultureInfo.InvariantCulture) + "%";
        pane.Refresh();

        // Next panel
        if (paneNextIndex.HasValue)
        {
            var paneNext = Panes[paneNextIndex.Value];

            if (OnResize.HasDelegate)
            {
                var arg = new FluentMultiSplitterResizeEventArgs()
                {
                    PaneIndex = paneNext.Index,
                    Pane = paneNext,
                    NewSize = sizeNextNew ?? 0,
                };
                await OnResize.InvokeAsync(arg);

                // cancel omitted because it is managed by the parent panel
            }

            paneNext.SizeRuntime = sizeNextNew?.ToString("0.00", CultureInfo.InvariantCulture) + "%" ?? string.Empty;
            paneNext.Refresh();
        }
    }

    /// <summary>
    /// Refreshes this instance.
    /// </summary>
    public void Refresh()
    {
        StateHasChanged();
    }

    /// <summary>
    /// Removes the pane.
    /// </summary>
    /// <param name="pane">The pane.</param>
    public void RemovePane(FluentMultiSplitterPane pane)
    {
        if (Panes.Contains(pane))
        {
            Panes.Remove(pane);
            StateHasChanged();
        }
    }

    /// <summary />
    internal async Task CollapseExecAsync(object args, int paneIndex)
    {
        var pane = Panes[paneIndex];
        var paneNext = pane.Next();

        if (paneNext != null &&
            paneNext.Collapsible &&
            paneNext.IsLast &&
            paneNext.Collapsed)
        {
            if (OnExpand.HasDelegate)
            {
                var arg = new FluentMultiSplitterEventArgs()
                {
                    PaneIndex = paneNext.Index,
                    Pane = paneNext,
                };

                await OnExpand.InvokeAsync(arg);

                if (arg.Cancel)
                {
                    return;
                }
            }

            paneNext.SetCollapsed(false);
        }
        else
        {
            if (OnCollapse.HasDelegate)
            {
                var arg = new FluentMultiSplitterEventArgs()
                {
                    PaneIndex = pane.Index,
                    Pane = pane,
                };

                await OnCollapse.InvokeAsync(arg);

                if (arg.Cancel)
                {
                    return;
                }
            }

            pane.SetCollapsed(true);
        }

        StateHasChanged();
    }

    /// <summary />
    internal async Task ExpandExecAsync(MouseEventArgs args, int paneIndex)
    {
        var pane = Panes[paneIndex];
        var paneNext = pane.Next();

        if (paneNext != null &&
            paneNext.Collapsible &&
            paneNext.IsLast &&
            !pane.Collapsed)
        {
            if (OnCollapse.HasDelegate)
            {
                var arg = new FluentMultiSplitterEventArgs()
                {
                    PaneIndex = paneNext.Index,
                    Pane = paneNext,
                };

                await OnCollapse.InvokeAsync(arg);

                if (arg.Cancel)
                {
                    return;
                }
            }

            paneNext.SetCollapsed(true);
        }
        else
        {
            if (OnExpand.HasDelegate)
            {
                var arg = new FluentMultiSplitterEventArgs()
                {
                    PaneIndex = pane.Index,
                    Pane = pane,
                };

                await OnExpand.InvokeAsync(arg);

                if (arg.Cancel)
                {
                    return;
                }
            }

            pane.SetCollapsed(false);
        }

        StateHasChanged();
    }

    /// <summary />
    internal async Task ResizeExecAsync(MouseEventArgs args, int paneIndex)
    {
        var pane = Panes[paneIndex];
        if (pane.Resizable)
        {
            var paneNextResizable = Panes.Skip(paneIndex + 1)
                                         .FirstOrDefault(o => o.Resizable && !o.Collapsed);

            if (Module != null)
            {
                await Module.InvokeVoidAsync(
                    "startSplitterResize",
                    Id,
                    _objRef,
                    pane.Id,
                    paneNextResizable?.Id,
                    Orientation.ToString(),
                    Orientation == Orientation.Horizontal ? args.ClientX : args.ClientY,
                    pane.Min,
                    pane.Max,
                    paneNextResizable?.Min,
                    paneNextResizable?.Max);
            }
        }
    }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Module = await JS.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
            _objRef = DotNetObjectReference.Create(this);
        }
    }
}
