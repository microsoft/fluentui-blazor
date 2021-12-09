using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentAccordion : FluentComponentBase
{
    [Parameter]
    public ExpandMode? ExpandMode { get; set; }
}