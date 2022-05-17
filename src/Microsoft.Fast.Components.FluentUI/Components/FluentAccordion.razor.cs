using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentAccordion : FluentComponentBase
{

    /// <summary>
    /// Controls the expand mode of the Accordion, either allowing single or multiple item expansion <seealso cref="ExpandMode" />.
    /// </summary>
    [Parameter]
    public ExpandMode? ExpandMode { get; set; }
}