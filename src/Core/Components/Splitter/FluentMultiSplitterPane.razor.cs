// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a pane in a multi-splitter layout, allowing for child content, resizing, and collapsing behavior.
/// It manages its size and state within a splitter.
/// </summary>
public partial class FluentMultiSplitterPane : FluentComponentBase
{
    /// <summary />
    public FluentMultiSplitterPane(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-multi-splitter-pane")
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("flex-basis", string.IsNullOrWhiteSpace(SizeRuntime) ? Size : SizeRuntime)
        .Build();

    /// <summary>
    /// Gets or sets the splitter.
    /// </summary>
    [CascadingParameter]
    internal FluentMultiSplitter? Splitter { get; set; }

    /// <summary>
    /// Gets or sets the child content.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets a collapsed indicating whether this <see cref="FluentMultiSplitterPane"/> is collapsed.
    /// </summary>
    [Parameter]
    public bool Collapsed { get; set; } = false;

    /// <summary>
    /// Gets or sets a collapsed indicating whether this <see cref="FluentMultiSplitterPane"/> is collapsible.
    /// </summary>
    [Parameter]
    public bool Collapsible { get; set; } = false;

    /// <summary>
    /// Determines the maximum collapsed.
    /// </summary>
    [Parameter]
    public string Max { get; set; } = string.Empty;

    /// <summary>
    /// Determines the minimum collapsed.
    /// </summary>
    [Parameter]
    public string Min { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a collapsed indicating whether this <see cref="FluentMultiSplitterPane"/> is resizable.
    /// </summary>
    [Parameter]
    public bool Resizable { get; set; } = true;

    /// <summary>
    /// Gets or sets the size.
    /// </summary>
    [Parameter]
    public string? Size { get; set; }

    /// <summary />
    internal int Index { get; set; }

    /// <summary />
    internal bool IsCollapsible
    {
        get
        {
            if (Collapsible && !Collapsed)
            {
                return true;
            }

            var paneNext = Next();
            if (paneNext == null)
            {
                return false;
            }

            return paneNext.IsLast && paneNext.Collapsible && paneNext.Collapsed;
        }
    }

    /// <summary />
    internal bool IsExpandable
    {
        get
        {
            if (Collapsed)
            {
                return true;
            }

            var paneNext = Next();
            if (paneNext == null)
            {
                return false;
            }

            return paneNext.IsLast && paneNext.Collapsible && !paneNext.Collapsed;
        }
    }

    /// <summary />
    internal bool IsLast => Splitter?.Panes.Count - 1 == Index;

    /// <summary />
    internal bool IsLastResizable
    {
        get => Splitter?.Panes.LastOrDefault(o => o.Resizable && !o.Collapsed) == this;
    }

    /// <summary />
    internal bool IsResizable
    {
        get
        {
            var paneNext = Next();

            if (Collapsed ||
                (Index == Splitter?.Panes.Count - 2 && paneNext?.IsResizable == false) ||
                (IsLastResizable && paneNext?.Collapsed == true))
            {
                return false;
            }

            return Resizable;
        }
    }

    /// <summary />
    internal bool SizeAuto => string.IsNullOrWhiteSpace(Size);

    /// <summary />
    internal string SizeRuntime { get; set; } = string.Empty;

    /// <summary />
    protected override void OnInitialized()
    {
        base.OnInitialized();
        Splitter?.AddPane(this);
    }

    /// <summary>
    /// Disposes the component and removes it from the splitter.
    /// </summary>
    /// <returns></returns>
    public override ValueTask DisposeAsync()
    {
        Splitter?.RemovePane(this);
        return base.DisposeAsync();
    }

    /// <summary />
    internal FluentMultiSplitterPane? Next()
    {
        return Index <= Splitter?.Panes.Count - 2
            ? Splitter?.Panes[Index + 1]
            : null;
    }

    /// <summary />
    internal void Refresh()
    {
        StateHasChanged();
    }

    /// <summary />
    internal void SetCollapsed(bool collapsed)
    {
        Collapsed = collapsed;
    }

    /// <summary />
    private string GetClassStatus()
    {
        if (Collapsed)
        {
            return "collapsed";
        }

        if (IsLastResizable)
        {
            return "last-resizable";
        }

        if (IsResizable)
        {
            return "resizable";
        }

        return "locked";
    }
}
