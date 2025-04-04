// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The FluentAccordion component allows to toggle the display of content by expanding or collapsing sections.
/// </summary>
public partial class FluentAccordion : FluentComponentBase
{
    private readonly Dictionary<string, FluentAccordionItem> items = [];

    /// <summary>
    /// Controls the expand mode of the Accordion, either allowing
    /// single or multiple item expansion. <seealso cref="AccordionExpandMode" />.
    /// </summary>
    [Parameter]
    public AccordionExpandMode? ExpandMode { get; set; } = AccordionExpandMode.Multi;

    /// <summary>
    /// Gets or sets the <see href="https://www.w3.org/TR/wai-aria-1.1/#aria-level">level</see> of the heading element.
    /// Possible values: 1 | 2 | 3 | 4 | 5 | 6
    /// </summary>
    [Parameter]
    public string? HeadingLevel { get; set; }

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
    /// Gets or sets the id of the active accordion item.
    /// </summary>
    [Parameter]
    public string? ActiveId { get; set; }

    /// <summary>
    /// Gets or sets a callback when the active id is changed.
    /// </summary>
    public EventCallback<string?> ActiveIdChanged { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets a callback when a accordion item is changed.
    /// </summary>
    [Parameter]
    public EventCallback<AccordionItemEventArgs> OnAccordionItemChange { get; set; }

    /// <summary>
    /// Gets or sets a callback when the expand mode is changed.
    /// </summary>
    public EventCallback<AccordionExpandMode> OnExpandModeChanged { get; set; }

    /// <summary />
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(AccordionItemEventArgs))]
    public FluentAccordion()
    {

    }

    private async Task HandleOnAccordionChangedAsync(AccordionItemEventArgs args)
    {
        if (args is not null)
        {
            ActiveId = args.Id;
            await ActiveIdChanged.InvokeAsync(ActiveId);

            if (ActiveId is not null && items.TryGetValue(ActiveId, out var item))
            {
                args.Item = item;
                await args.Item.SetExpandedAsync(args.Expanded);
                await OnAccordionItemChange.InvokeAsync(args);
            }
        }
    }

    /// <summary>
    /// Expands the accordion item with the specified id.
    /// </summary>
    public async Task ExpandItemAsync(string id)
    {
        await HandleStateAsync(id, expanded: true);
    }

    /// <summary>
    /// Collapses the accordion item with the specified id.
    /// </summary>
    public async Task CollapseItemAsync(string id)
    {
        await HandleStateAsync(id, expanded: false);
    }

    internal async Task HandleStateAsync(string id, bool expanded)
    {
        if (items.TryGetValue(id, out var item))
        {
            await HandleOnAccordionChangedAsync(new AccordionItemEventArgs()
            {
                Item = item,
                Id = id,
                Expanded = expanded,
                HeaderText = item.Header,
            });
        }
    }

    internal void Register(FluentAccordionItem item)
    {
        items.Add(item.Id!, item);
    }

    internal void Unregister(FluentAccordionItem item)
    {
        items.Remove(item.Id!);
    }
}
