// --------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
// --------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentDragContainer<TItem> : FluentComponentBase
{
    /// <summary />
    protected virtual string? ClassValue => new CssBuilder(Class).Build();

    /// <summary />
    protected virtual string? StyleValue => new StyleBuilder(Style).Build();

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

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

    /// <summary>
    /// property to keep the zone currently dragged.
    /// </summary>
    internal FluentDropZone<TItem>? StartedZone { get; private set; }

    /// <summary />
    internal void SetStartedZone(FluentDropZone<TItem>? value)
    {
        StartedZone = value;
        StateHasChanged();
    }
}
