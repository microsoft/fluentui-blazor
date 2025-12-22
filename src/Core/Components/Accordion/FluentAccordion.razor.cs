// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The FluentAccordion component allows to toggle the display of content by expanding or collapsing sections.
/// </summary>
public partial class FluentAccordion : FluentComponentBase
{
    private readonly Dictionary<string, FluentAccordionItem> _items = [];

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder.Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder.Build();

    /// <summary />
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(AccordionItemEventArgs))]
    public FluentAccordion(LibraryConfiguration configuration) : base(configuration)
    {

    }

    /// <summary>
    /// Controls the expand mode of the Accordion, either allowing
    /// single or multiple item expansion. <seealso cref="AccordionExpandMode" />.
    /// </summary>
    [Parameter]
    public AccordionExpandMode? ExpandMode { get; set; }

    /// <summary>
    /// Gets or sets a callback when the expand mode is changed.
    /// </summary>
    [Parameter]
    public EventCallback<AccordionExpandMode?> ExpandModeChanged { get; set; }

    /// <summary>
    /// Gets or sets the <see href="https://www.w3.org/TR/wai-aria-1.1/#aria-level">level</see> of the heading element.
    /// Possible values: 1 | 2 | 3 | 4 | 5 | 6
    /// </summary>
    [Parameter]
    public int? HeadingLevel { get; set; }

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
    [Parameter]
    public EventCallback<string?> ActiveIdChanged { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets a callback when an accordion item is changed.
    /// </summary>
    [Parameter]
    public EventCallback<AccordionItemEventArgs> OnAccordionItemChange { get; set; }

    private async Task HandleOnAccordionChangedAsync(AccordionItemEventArgs args)
    {
        if (args is not null)
        {
            ActiveId = args.Id;
            if (ActiveIdChanged.HasDelegate)
            {
                await ActiveIdChanged.InvokeAsync(ActiveId);
            }

            if (ActiveId is not null && _items.TryGetValue(ActiveId, out var item))
            {
                args.Item = item;
                await args.Item.SetExpandedAsync(args.Expanded);
                if (OnAccordionItemChange.HasDelegate)
                {
                    await OnAccordionItemChange.InvokeAsync(args);
                }
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
        if (_items.TryGetValue(id, out var item))
        {
            await HandleOnAccordionChangedAsync(new AccordionItemEventArgs()
            {
                Item = item,
                Id = id,
                Expanded = expanded,
                HeaderText = item.Header,
            });
            await ActiveIdChanged.InvokeAsync(id);
        }
    }

    internal void Register(FluentAccordionItem item)
    {

        if (!string.IsNullOrEmpty(item.Id))
        {
            if (!_items.TryAdd(item.Id, item))
            {
                _items[item.Id] = item;
            }
        }
    }

    internal void Unregister(FluentAccordionItem item)
    {
        if (!string.IsNullOrEmpty(item.Id))
        {
            _items.Remove(item.Id);
        }
    }

    internal Dictionary<string, FluentAccordionItem> GetItems()
    {
        return _items;
    }
}
