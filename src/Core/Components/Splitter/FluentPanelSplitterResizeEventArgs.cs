// --------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
// --------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Supplies information about a <see cref="FluentPanelSplitter.OnResize" /> event that is being raised.
/// </summary>
public class FluentPanelSplitterResizeEventArgs : FluentPanelSplitterEventArgs
{
    /// <summary>
    /// The new size of the pane.
    /// </summary>
    public double NewSize { get; set; }
}
