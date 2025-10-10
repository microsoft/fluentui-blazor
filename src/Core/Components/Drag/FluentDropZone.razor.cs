// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentDropZone<TItem> : FluentComponentBase
{
    /// <summary />
    public FluentDropZone(LibraryConfiguration configuration) : base(configuration) { }

    /// <summary />
    protected virtual string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary />
    protected virtual string? StyleValue => DefaultStyleBuilder
        .Build();

    /// <summary />
    [CascadingParameter]
    public required FluentDragContainer<TItem> Container { get; set; }

    /// <summary>
    /// Gets or sets the item to identify a draggable zone.
    /// </summary>
    [Parameter]
    public TItem Item { get; set; } = default!;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the element can receive a dragged item.
    /// </summary>
    [Parameter]
    public bool Droppable { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the element can be dragged.
    /// </summary>
    [Parameter]
    public bool Draggable { get; set; }

    /// <summary>
    /// This event is fired when the user starts dragging an element.
    /// </summary>
    [Parameter]
    public Action<FluentDragEventArgs<TItem>>? OnDragStart { get; set; }

    /// <summary>
    /// This event is fired when the drag operation ends (such as releasing a mouse button or hitting the Esc key).
    /// </summary>
    [Parameter]
    public Action<FluentDragEventArgs<TItem>>? OnDragEnd { get; set; }

    /// <summary>
    /// This event is fired when a dragged element enters a valid drop target.
    /// </summary>
    [Parameter]
    public Action<FluentDragEventArgs<TItem>>? OnDragEnter { get; set; }

    /// <summary>
    /// This event is fired when an element is being dragged over a valid drop target.
    /// </summary>
    [Parameter]
    public Action<FluentDragEventArgs<TItem>>? OnDragOver { get; set; }

    /// <summary>
    /// This event is fired when a dragged element leaves a valid drop target.
    /// </summary>
    [Parameter]
    public Action<FluentDragEventArgs<TItem>>? OnDragLeave { get; set; }

    /// <summary>
    /// This event is fired when an element is dropped on a valid drop target.
    /// </summary>
    [Parameter]
    public Action<FluentDragEventArgs<TItem>>? OnDropEnd { get; set; }

    /// <summary>
    /// Gets or sets a way to prevent further propagation of the current event in the capturing and bubbling phases.
    /// </summary>
    [Parameter]
    public bool StopPropagation { get; set; }

    /// <summary />
    private bool IsOver { get; set; }

    /// <summary />
    private void OnDragStartHandler(DragEventArgs e)
    {
        if (!Draggable)
        {
            return;
        }

        Container.SetStartedZone(this);

        OnDragStart?.Invoke(new FluentDragEventArgs<TItem>
        {
            Source = this,
            Target = this,
        });

        Container.OnDragStart?.Invoke(new FluentDragEventArgs<TItem>
        {
            Source = this,
            Target = this,
        });
    }

    /// <summary />
    private void OnDragEndHandler(DragEventArgs e)
    {
        if (!Draggable)
        {
            return;
        }

        OnDragEnd?.Invoke(new FluentDragEventArgs<TItem>
        {
            Source = this,
            Target = this,
        });

        Container.OnDragEnd?.Invoke(new FluentDragEventArgs<TItem>
        {
            Source = this,
            Target = this,
        });
    }

    /// <summary />
    private void OnDragEnterHandler(DragEventArgs e)
    {
        if (!Droppable)
        {
            return;
        }

        IsOver = true;

        if (Container.StartedZone == null)
        {
            return;
        }

        OnDragEnter?.Invoke(new FluentDragEventArgs<TItem>
        {
            Source = Container.StartedZone,
            Target = this,
        });

        Container.OnDragEnter?.Invoke(new FluentDragEventArgs<TItem>
        {
            Source = Container.StartedZone,
            Target = this,
        });
    }

    /// <summary />
    private void OnDragOverHandler(DragEventArgs e)
    {
        if (!Droppable)
        {
            return;
        }

        if (Container.StartedZone == null)
        {
            return;
        }

        IsOver = true;

        OnDragOver?.Invoke(new FluentDragEventArgs<TItem>
        {
            Source = Container.StartedZone,
            Target = this,
        });

        Container.OnDragOver?.Invoke(new FluentDragEventArgs<TItem>
        {
            Source = Container.StartedZone,
            Target = this,
        });
    }

    /// <summary />
    private void OnDragLeaveHandler(DragEventArgs e)
    {
        if (!Droppable)
        {
            return;
        }

        IsOver = false;

        if (Container.StartedZone == null)
        {
            return;
        }

        OnDragLeave?.Invoke(new FluentDragEventArgs<TItem>
        {
            Source = Container.StartedZone,
            Target = this,
        });

        Container.OnDragLeave?.Invoke(new FluentDragEventArgs<TItem>
        {
            Source = Container.StartedZone,
            Target = this,
        });
    }

    /// <summary />
    private void OnDropHandler(DragEventArgs e)
    {
        if (!Droppable)
        {
            return;
        }

        IsOver = false;

        if (Container.StartedZone == null)
        {
            return;
        }

        OnDropEnd?.Invoke(new FluentDragEventArgs<TItem>
        {
            Source = Container.StartedZone,
            Target = this,
        });

        Container.OnDropEnd?.Invoke(new FluentDragEventArgs<TItem>
        {
            Source = Container.StartedZone,
            Target = this,
        });

        Container.SetStartedZone(value: null);
    }
}
