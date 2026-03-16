// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentDropZone<TItem> : FluentComponentBase

{
    /// <summary />
    protected virtual string? ClassValue => new CssBuilder(Class).Build();

    /// <summary />
    protected virtual string? StyleValue => new StyleBuilder(Style).Build();

    /// <summary />
    [CascadingParameter]
    private FluentDragContainer<TItem> Container { get; set; } = default!;

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
    public EventCallback<FluentDragEventArgs<TItem>> OnDragStart { get; set; }

    /// <summary>
    /// This event is fired when the drag operation ends (such as releasing a mouse button or hitting the Esc key).
    /// </summary>
    [Parameter]
    public EventCallback<FluentDragEventArgs<TItem>> OnDragEnd { get; set; }

    /// <summary>
    /// This event is fired when a dragged element enters a valid drop target.
    /// </summary>
    [Parameter]
    public EventCallback<FluentDragEventArgs<TItem>> OnDragEnter { get; set; }

    /// <summary>
    /// This event is fired when an element is being dragged over a valid drop target.
    /// </summary>
    [Parameter]
    public EventCallback<FluentDragEventArgs<TItem>> OnDragOver { get; set; }

    /// <summary>
    /// This event is fired when a dragged element leaves a valid drop target.
    /// </summary>
    [Parameter]
    public EventCallback<FluentDragEventArgs<TItem>> OnDragLeave { get; set; }

    /// <summary>
    /// This event is fired when an element is dropped on a valid drop target.
    /// </summary>
    [Parameter]
    public EventCallback<FluentDragEventArgs<TItem>> OnDropEnd { get; set; }

    /// <summary>
    /// Gets or sets a way to prevent further propagation of the current event in the capturing and bubbling phases.
    /// </summary>
    [Parameter]
    public bool StopPropagation { get; set; }

    /// <summary />
    private bool IsOver { get; set; } = false;

    /// <summary />
    private async Task OnDragStartHandlerAsync(DragEventArgs e)
    {
        if (!Draggable)
        {
            return;
        }

        Container.SetStartedZone(this);

        if (OnDragStart.HasDelegate)
        {
            await OnDragStart.InvokeAsync(new FluentDragEventArgs<TItem>(source: this, target: this));
        }

        if (Container.OnDragStart.HasDelegate)
        {
            await Container.OnDragStart.InvokeAsync(new FluentDragEventArgs<TItem>(source: this, target: this));
        }
    }

    /// <summary />
    private async Task OnDragEndHandlerAsync(DragEventArgs e)
    {
        if (!Draggable)
        {
            return;
        }

        if (OnDragEnd.HasDelegate)
        {
            await OnDragEnd.InvokeAsync(new FluentDragEventArgs<TItem>(source: this, target: this));
        }

        if (Container.OnDragEnd.HasDelegate)
        {
            await Container.OnDragEnd.InvokeAsync(new FluentDragEventArgs<TItem>(source: this, target: this));
        }
    }

    /// <summary />
    private async Task OnDragEnterHandlerAsync(DragEventArgs e)
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

        if (OnDragEnter.HasDelegate)
        {
            await OnDragEnter.InvokeAsync(new FluentDragEventArgs<TItem>(source: Container.StartedZone, target: this));
        }

        if (Container.OnDragEnter.HasDelegate)
        {
            await Container.OnDragEnter.InvokeAsync(new FluentDragEventArgs<TItem>(source: Container.StartedZone, target: this));
        }
    }

    /// <summary />
    private async Task OnDragOverHandlerAsync(DragEventArgs e)
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

        if (OnDragOver.HasDelegate)
        {
            await OnDragOver.InvokeAsync(new FluentDragEventArgs<TItem>(source: Container.StartedZone, target: this));
        }

        if (Container.OnDragOver.HasDelegate)
        {
            await Container.OnDragOver.InvokeAsync(new FluentDragEventArgs<TItem>(source: Container.StartedZone, target: this));
        }
    }

    /// <summary />
    private async Task OnDragLeaveHandlerAsync(DragEventArgs e)
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

        if (OnDragLeave.HasDelegate)
        {
            await OnDragLeave.InvokeAsync(new FluentDragEventArgs<TItem>(source: Container.StartedZone, target: this));
        }

        if (Container.OnDragLeave.HasDelegate)
        {
            await Container.OnDragLeave.InvokeAsync(new FluentDragEventArgs<TItem>(source: Container.StartedZone, target: this));
        }
    }

    /// <summary />
    private async Task OnDropHandlerAsync(DragEventArgs e)
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

        if (OnDropEnd.HasDelegate)
        {
            await OnDropEnd.InvokeAsync(new FluentDragEventArgs<TItem>(source: Container.StartedZone, target: this));
        }

        if (Container.OnDropEnd.HasDelegate)
        {
            await Container.OnDropEnd.InvokeAsync(new FluentDragEventArgs<TItem>(source: Container.StartedZone, target: this));
        }

        Container.SetStartedZone(null);
    }
}
