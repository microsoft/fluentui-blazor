namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public class FluentDragEventArgs<TItem> : EventArgs
{
    /// <summary />
    public FluentDragEventArgs()
    {
    }

    /// <summary />
    internal FluentDragEventArgs(FluentDropZone<TItem> source, FluentDropZone<TItem> target)
    {
        Source = source;
        Target = target;
    }

    /// <summary>
    /// Gets the zone where the drag started.
    /// </summary>
    public FluentDropZone<TItem> Source { get; } = null!;

    /// <summary>
    /// Gets the zone where the drag ended.
    /// </summary>
    public FluentDropZone<TItem> Target { get; } = null!;
}
