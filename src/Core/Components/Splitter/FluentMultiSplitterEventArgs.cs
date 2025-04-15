// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Supplies information about a <see cref="FluentMultiSplitter.OnExpand" />
/// or <see cref="FluentMultiSplitter.OnCollapse" /> event that is being raised.
/// </summary>
public class FluentMultiSplitterEventArgs
{
    /// <summary />
    internal FluentMultiSplitterEventArgs(int paneIndex, FluentMultiSplitterPane pane)
    {
        PaneIndex = paneIndex;
        Pane = pane;
    }

    /// <summary>
    /// Gets the index of the pane.
    /// </summary>
    public int PaneIndex { get; }

    /// <summary>
    /// Gets the pane which the event applies to.
    /// </summary>
    /// <value>The pane.</value>
    public FluentMultiSplitterPane Pane { get; }

    /// <summary>
    /// Gets or sets a value which will cancel the event.
    /// </summary>
    /// <value>`true` to cancel the event; otherwise, `false`.</value>
    public bool Cancel { get; set; }
}
