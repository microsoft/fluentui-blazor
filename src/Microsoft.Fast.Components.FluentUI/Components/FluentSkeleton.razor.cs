using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentSkeleton
{
    [Parameter]
    public Shape? Shape { get; set; }

    [Parameter]
    public bool? Shimmer { get; set; }

    [Parameter]
    public string? Pattern { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object>? AdditionalAttributes { get; set; }
}