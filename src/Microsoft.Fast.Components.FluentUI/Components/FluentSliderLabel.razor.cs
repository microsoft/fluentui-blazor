using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentSliderLabel
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public int? Position { get; set; }

    [Parameter]
    public bool? HideMark { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object>? AdditionalAttributes { get; set; }
}