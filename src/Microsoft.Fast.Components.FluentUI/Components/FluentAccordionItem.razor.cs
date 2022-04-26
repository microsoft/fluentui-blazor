using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentAccordionItem : FluentComponentBase
{
    [Parameter]
    public bool? Expanded { get; set; }
}