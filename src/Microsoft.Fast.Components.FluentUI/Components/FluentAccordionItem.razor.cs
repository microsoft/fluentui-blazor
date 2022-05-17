using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentAccordionItem : FluentComponentBase
{
    /// <summary>
    /// Expands or collapses the item.
    /// </summary>
    [Parameter]
    public bool? Expanded { get; set; }
}