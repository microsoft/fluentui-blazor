using System.Collections.Generic;

using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentTabs
{
    [Parameter]
    public bool? ActiveIndicator { get; set; }

    [Parameter]
    public Orientation? Orientation { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object>? AdditionalAttributes { get; set; }
}