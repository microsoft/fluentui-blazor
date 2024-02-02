// --------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
// --------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Supplies information about a <see cref="FluentMultiSplitter.OnExpand" /> or <see cref="FluentMultiSplitter.OnCollapse" /> event that is being raised.
/// </summary>
public class FluentMultiSplitterEventArgs
{
    /// <summary>
    /// Gets the index of the pane.
    /// </summary>
    public int PaneIndex { get; set; } = 0;

    /// <summary>
    /// Gets the pane which the event applies to.
    /// </summary>
    /// <value>The pane.</value>
    public FluentMultiSplitterPane Pane { get; set; } = default!;

    /// <summary>
    /// Gets or sets a value which will cancel the event.
    /// </summary>
    /// <value><c>true</c> to cancel the event; otherwise, <c>false</c>.</value>
    public bool Cancel { get; set; } = false;
}
