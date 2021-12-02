using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentAccordion
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public ExpandMode? ExpandMode { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object>? AdditionalAttributes { get; set; }
}