using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentAccordion : FluentComponentBase
{

    /// <summary>
    /// Controls the expand mode of the Accordion, either allowing single or multiple item expansion <seealso cref="AccordionExpandMode" />.
    /// </summary>
    [Parameter]
    public AccordionExpandMode? ExpandMode { get; set; }

    /// <summary>
    /// Gets or sets the id of the active accordion item
    /// </summary>
    [Parameter]
    public string? ActiveId { get; set; }

    /// <summary>
    /// Gets or sets a callback that updates the bound value.
    /// </summary>
    [Parameter]
    public EventCallback<string?> ActiveIdChanged { get; set; }

    private async Task OnAccordionChange(AccordionChangeEventArgs args)
    {
        await ActiveIdChanged.InvokeAsync(args.ActiveId);
    }
}