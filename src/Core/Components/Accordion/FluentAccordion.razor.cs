using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

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
    /// Gets or sets the id of the active accordion item.
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
    /// Gets or sets a callback when a accordion item is changed.
    /// </summary>
    [Parameter]
    public EventCallback<FluentAccordionItem> OnAccordionItemChange { get; set; }

    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(AccordionChangeEventArgs))]

    public FluentAccordion()
    {

    }

    private async Task HandleOnAccordionChangedAsync(AccordionChangeEventArgs args)
    {
        if (args is not null)
        {
            var Id = args.ActiveId;
            if (Id is not null && items.TryGetValue(Id!, out FluentAccordionItem? item))
            {
                await OnAccordionItemChange.InvokeAsync(item);
                await ActiveIdChanged.InvokeAsync(Id);
            }
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
