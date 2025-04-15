// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Supplies information about a <see cref="FluentMultiSplitter.OnResize" /> event that is being raised.
/// </summary>
public class FluentMultiSplitterResizeEventArgs : FluentMultiSplitterEventArgs
{
    /// <summary />
    internal FluentMultiSplitterResizeEventArgs(int paneIndex, FluentMultiSplitterPane pane, double newSize)
        : base(paneIndex, pane)
    {
        NewSize = newSize;
    }

    /// <summary>
    /// Gets the new size of the pane.
    /// </summary>
    public double NewSize { get; }
}
