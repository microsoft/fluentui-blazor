// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public class FluentDragEventArgs<TItem> : EventArgs
{
    /// <summary>
    /// Gets the zone where the drag started.
    /// </summary>
    public required FluentDropZone<TItem> Source { get; init; }

    /// <summary>
    /// Gets the zone where the drag ended.
    /// </summary>
    public required FluentDropZone<TItem> Target { get; init; }
}
