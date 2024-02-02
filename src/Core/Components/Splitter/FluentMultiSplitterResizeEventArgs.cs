// --------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
// --------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Supplies information about a <see cref="FluentMultiSplitter.OnResize" /> event that is being raised.
/// </summary>
public class FluentMultiSplitterResizeEventArgs : FluentMultiSplitterEventArgs
{
    /// <summary>
    /// The new size of the pane.
    /// </summary>
    public double NewSize { get; set; }
}
