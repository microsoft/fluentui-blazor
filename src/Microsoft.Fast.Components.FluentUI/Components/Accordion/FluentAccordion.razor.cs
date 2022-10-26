using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentAccordion : FluentComponentBase
{
    private readonly Dictionary<string, FluentAccordionItem> items = new();

    /// <summary>
    /// Controls the expand mode of the Accordion, either allowing
    /// single or multiple item expansion. <seealso cref="AccordionExpandMode" />.
    /// </summary>
    [Parameter]
    public AccordionExpandMode? ExpandMode { get; set; } = AccordionExpandMode.Multi;

    /// <summary>
    /// Gets or sets the id of the active accordion item
    /// </summary>
    [Parameter]
    public string? ActiveId { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }


    /// <summary>
    /// Gets or sets a callback that updates the bound value.
    /// </summary>
    [Parameter]
    public EventCallback<string?> ActiveIdChanged { get; set; }

    /// <summary>
    /// Gets or sets a callback when a accordion item is changed .
    /// </summary>
    [Parameter]
    public EventCallback<FluentAccordionItem> OnAccordionItemChange { get; set; }

    private async Task HandleOnAccordionChanged(AccordionChangeEventArgs args)
    {
        string? Id = args.ActiveId;
        if (items.TryGetValue(Id!, out FluentAccordionItem? item))
        {
            await OnAccordionItemChange.InvokeAsync(item);
            await ActiveIdChanged.InvokeAsync(Id);
        }
    }

    internal void Register(FluentAccordionItem item)
    {
        items.Add(item.Id, item);
    }

    internal void Unregister(FluentAccordionItem item)
    {
        items.Remove(item.Id);
    }
}