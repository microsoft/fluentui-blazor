// --------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
// --------------------------------------------------------------

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
    public Action<FluentDragEventArgs<TItem>>? OnDragStart { get; set; }

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

    /// <summary />
    private bool IsOver { get; set; } = false;

    /// <summary />
    private void OnDragStartHandler(DragEventArgs e)
    {
        if (!Draggable)
        {
            return;
        }

        Container.SetStartedZone(this);

        if (OnDragStart != null)
        {
            OnDragStart(new FluentDragEventArgs<TItem>(this, this));
        }

        if (Container.OnDragStart != null)
        {
            Container.OnDragStart(new FluentDragEventArgs<TItem>(this, this));
        }
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

        if (OnDragEnter != null)
        {
            OnDragEnter(new FluentDragEventArgs<TItem>(Container.StartedZone, this));
        }

        if (Container.OnDragEnter != null)
        {
            Container.OnDragEnter(new FluentDragEventArgs<TItem>(Container.StartedZone, this));
        }
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

        if (OnDragOver != null)
        {
            OnDragOver(new FluentDragEventArgs<TItem>(Container.StartedZone, this));
        }

        if (Container.OnDragOver != null)
        {
            Container.OnDragOver(new FluentDragEventArgs<TItem>(Container.StartedZone, this));
        }
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

        if (OnDragLeave != null)
        {
            OnDragLeave(new FluentDragEventArgs<TItem>(Container.StartedZone, this));
        }

        if (Container.OnDragLeave != null)
        {
            Container.OnDragLeave(new FluentDragEventArgs<TItem>(Container.StartedZone, this));
        }
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

        if (OnDropEnd != null)
        {
            OnDropEnd(new FluentDragEventArgs<TItem>(Container.StartedZone, this));
        }

        if (Container.OnDropEnd != null)
        {
            Container.OnDropEnd(new FluentDragEventArgs<TItem>(Container.StartedZone, this));
        }

        Container.SetStartedZone(null);
    }
}
