// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
///  Represents an item in a Fluent Accordion component, allowing for customization of its heading, expanded state, and
///  content. It also manages its registration with the owning FluentTreeView and handles state changes.
/// </summary>
public partial class FluentAccordionItem : FluentComponentBase, IDisposable
{
    /// <summary>
    /// Gets or sets the owning FluentTreeView.
    /// </summary>
    [CascadingParameter]
    public FluentAccordion? Owner { get; set; } = default!;

    /// <summary>
    /// Gets or sets the heading of the accordion item.
    /// Use either this or the <see cref="HeaderTemplate"/> parameter."/>
    /// If both are set, this parameter will be used.
    /// </summary>
    [Parameter]
    public string? Header { get; set; }

    /// <summary>
    /// Gets or sets the heading content of the accordion item.
    /// Use either this or the <see cref="Header"/> parameter."/>
    /// If both are set, this parameter will not be used.
    /// </summary>
    [Parameter]
    public RenderFragment? HeaderTemplate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the item is expanded or collapsed.
    /// </summary>
    [Parameter]
    public bool Expanded { get; set; } = false;

    /// <summary>
    /// Gets or sets a callback for when the expanded state changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> ExpandedChanged { get; set; }

    /// <summary>
    /// Gets or sets the <see href="https://www.w3.org/TR/wai-aria-1.1/#aria-level">level</see> of the heading element.
    /// Possible values: 1 | 2 | 3 | 4 | 5 | 6
    /// </summary>
    [Parameter]
    public string? HeadingLevel { get; set; }

    /// <summary>
    /// Gets or sets whether the accordion item is disabled.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets the size of the accordion item.
    /// </summary>
    [Parameter]
    public AccordionItemSize? Size { get; set; }

    /// <summary>
    /// Gets or sets the position of the expand/collapse marker.
    /// </summary>
    [Parameter]
    public AccordionItemMarkerPosition? MarkerPosition { get; set; }

    /// <summary>
    /// Gets or sets the width of the focus state
    /// </summary>
    [Parameter]
    public bool? Block { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary />
    public FluentAccordionItem()
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected override void OnInitialized()
    {
        Owner?.Register(this);

    }

    /// <summary />
    protected override void OnParametersSet()
    {
        HeadingLevel = Owner?.HeadingLevel;
        MarkerPosition = Owner?.MarkerPosition;
        Block = Owner?.Block;
        Size = Owner?.Size;
    }

    /// <summary>
    /// Sets the expanded state of the accordion item.
    /// </summary>
    public async Task SetExpandedAsync(bool expanded)
    {
        Expanded = expanded;
        if (ExpandedChanged.HasDelegate)
        {
            await ExpandedChanged.InvokeAsync(Expanded);
        }
    }

    /// <summary />
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {

            Owner?.Unregister(this);
        }
    }

    /// <summary />
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
