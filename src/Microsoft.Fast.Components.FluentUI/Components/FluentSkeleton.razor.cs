using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentSkeleton : FluentComponentBase
{
    [Parameter]
    public Shape? Shape { get; set; }

    [Parameter]
    public bool? Shimmer { get; set; }

    [Parameter]
    public string? Pattern { get; set; }
}