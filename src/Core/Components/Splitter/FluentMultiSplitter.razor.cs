// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a multi-pane splitter component that allows resizing, collapsing, and expanding of its panes. It supports
/// callbacks for pane events.
/// </summary>
public partial class FluentMultiSplitter : FluentComponentBase, IFluentComponentElementBase
{
    private DotNetObjectReference<FluentMultiSplitter>? _dotNetSplitterHelper;

    /// <summary />
    public FluentMultiSplitter(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    internal List<FluentMultiSplitterPane> Panes { get; } = [];

    /// <inheritdoc cref="IFluentComponentElementBase.Element" />
    [Parameter]
    public ElementReference Element { get; set; }

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
    protected string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-multi-splitter")
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("height", Height, () => !string.IsNullOrEmpty(Height))
        .AddStyle("width", Width, () => !string.IsNullOrEmpty(Width))
        .AddStyle("--fluent-multi-splitter-bar-size", BarSize, () => !string.IsNullOrEmpty(BarSize))
        .Build();

    /// <summary>
    /// Adds the pane.
    /// </summary>
    /// <param name="pane">The pane.</param>
    internal void AddPane(FluentMultiSplitterPane pane)
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
                    item.SizeRuntime = $"{Convert.ToString(100 / nbSizeAutoPanes, CultureInfo.InvariantCulture)}%";
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
            var arg = new FluentMultiSplitterResizeEventArgs(pane.Index, pane, sizeNew);

            if (OnResize.HasDelegate)
            {
                await OnResize.InvokeAsync(arg);
            }

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
                var arg = new FluentMultiSplitterResizeEventArgs(paneNext.Index, paneNext, sizeNextNew ?? 0);
                if (OnResize.HasDelegate)
                {
                    await OnResize.InvokeAsync(arg);
                }

                // cancel omitted because it is managed by the parent panel
            }

            paneNext.SizeRuntime = sizeNextNew?.ToString("0.00", CultureInfo.InvariantCulture) + "%" ?? string.Empty;
            paneNext.Refresh();
        }
    }

    /// <summary>
    /// Refreshes this instance.
    /// </summary>
    public Task RefreshAsync()
    {
        return InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Removes the pane.
    /// </summary>
    /// <param name="pane">The pane.</param>
    internal void RemovePane(FluentMultiSplitterPane pane)
    {
        Panes.Remove(pane);
        StateHasChanged();
    }

    /// <summary />
    internal async Task CollapseExecAsync(int paneIndex)
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
                var arg = new FluentMultiSplitterEventArgs(paneNext.Index, paneNext);
                if (OnExpand.HasDelegate)
                {
                    await OnExpand.InvokeAsync(arg);
                }

                if (arg.Cancel)
                {
                    return;
                }
            }

            paneNext.SetCollapsed(collapsed: false);
        }
        else
        {
            if (OnCollapse.HasDelegate)
            {
                var arg = new FluentMultiSplitterEventArgs(pane.Index, pane);
                if (OnCollapse.HasDelegate)
                {
                    await OnCollapse.InvokeAsync(arg);
                }

                if (arg.Cancel)
                {
                    return;
                }
            }

            pane.SetCollapsed(collapsed: true);
        }

        StateHasChanged();
    }

    /// <summary />
    internal async Task ExpandExecAsync(int paneIndex)
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
                var arg = new FluentMultiSplitterEventArgs(paneNext.Index, paneNext);

                if (OnCollapse.HasDelegate)
                {
                    await OnCollapse.InvokeAsync(arg);
                }

                if (arg.Cancel)
                {
                    return;
                }
            }

            paneNext.SetCollapsed(collapsed: true);
        }
        else
        {
            if (OnExpand.HasDelegate)
            {
                var arg = new FluentMultiSplitterEventArgs(pane.Index, pane);

                if (OnExpand.HasDelegate)
                {
                    await OnExpand.InvokeAsync(arg);
                }

                if (arg.Cancel)
                {
                    return;
                }
            }

            pane.SetCollapsed(collapsed: false);
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

            await JSRuntime.InvokeVoidAsync(
                "Microsoft.FluentUI.Blazor.Components.MultiSplitter.StartResize",
                Element,
                _dotNetSplitterHelper,
                pane.Id,
                paneNextResizable?.Id,
                Orientation.ToAttributeValue(),
                Orientation == Orientation.Horizontal ? args.ClientX : args.ClientY,
                pane.Min,
                pane.Max,
                paneNextResizable?.Min,
                paneNextResizable?.Max);
        }
    }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(Id);

        if (firstRender)
        {
            _dotNetSplitterHelper = DotNetObjectReference.Create(this);
        }

        await Task.CompletedTask;
    }
}
