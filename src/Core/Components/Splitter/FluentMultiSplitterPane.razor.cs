// --------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
// --------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

#pragma warning disable FluentMultiSplitter // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
public partial class FluentMultiSplitterPane : FluentComponentBase, IDisposable
{
    private string _size = string.Empty;

    private FluentMultiSplitter _splitter = default!;

    /// <summary />
    public FluentMultiSplitterPane()
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
    /// Gets or sets a value indicating whether this <see cref="FluentMultiSplitterPane"/> is collapsed.
    /// </summary>
    /// <value><c>true</c> if collapsed; otherwise, <c>false</c>.</value>
    [Parameter]
    public bool Collapsed { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="FluentMultiSplitterPane"/> is collapsible.
    /// </summary>
    /// <value><c>true</c> if collapsible; otherwise, <c>false</c>.</value>
    [Parameter]
    public bool Collapsible { get; set; } = false;

    /// <summary>
    /// Determines the maximum value.
    /// </summary>
    /// <value>The maximum value.</value>
    [Parameter]
    public string Max { get; set; } = string.Empty;

    /// <summary>
    /// Determines the minimum value.
    /// </summary>
    /// <value>The minimum value.</value>
    [Parameter]
    public string Min { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="FluentMultiSplitterPane"/> is resizable.
    /// </summary>
    /// <value><c>true</c> if resizable; otherwise, <c>false</c>.</value>
    [Parameter]
    public bool Resizable { get; set; } = true;

    /// <summary>
    /// Gets or sets the size.
    /// </summary>
    /// <value>The size.</value>
    [Parameter]
    public string Size
    {
        get
        {
            return string.IsNullOrWhiteSpace(SizeRuntime) ? _size : SizeRuntime;
        }

        set
        {
            _size = value;
        }
    }

    /// <summary>
    /// Gets or sets the splitter.
    /// </summary>
    /// <value>The splitter.</value>
    [CascadingParameter]
    public FluentMultiSplitter Splitter
    {
        get => _splitter;
        set
        {
            if (_splitter != value)
            {
                _splitter = value;
                _splitter.AddPane(this);
            }
        }
    }

    /// <summary />
    internal int Index { get; set; } = 0;

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
    internal bool IsLast => Splitter.Panes.Count - 1 == Index;

    /// <summary />
    internal bool IsLastResizable
    {
        get => Splitter.Panes.LastOrDefault(o => o.Resizable && !o.Collapsed) == this;
    }

    /// <summary />
    internal bool IsResizable
    {
        get
        {
            var paneNext = Next();

            if (Collapsed ||
                (Index == Splitter.Panes.Count - 2 && paneNext?.IsResizable == false) ||
                (IsLastResizable && paneNext?.Collapsed == true))
            {
                return false;
            }
            else
            {
                return Resizable;
            }
        }
    }

    /// <summary />
    internal bool SizeAuto => string.IsNullOrWhiteSpace(_size);

    /// <summary />
    internal string SizeRuntime { get; set; } = string.Empty;

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-multi-splitter-pane")
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder(Style)
        .AddStyle("flex-basis", Size)
        .Build();

    /// <summary />
    public void Dispose()
    {
        Splitter?.RemovePane(this);
    }

    /// <summary />
    internal FluentMultiSplitterPane? Next()
    {
        return Index <= Splitter.Panes.Count - 2
            ? Splitter.Panes[Index + 1]
            : null;
    }

    /// <summary />
    internal void Refresh()
    {
        StateHasChanged();
    }

    /// <summary />
    internal void SetCollapsed(bool value)
    {
        Collapsed = value;
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
            return "lastresizable";
        }

        if (IsResizable)
        {
            return "resizable";
        }

        return "locked";
    }
}
#pragma warning restore FluentMultiSplitter // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
