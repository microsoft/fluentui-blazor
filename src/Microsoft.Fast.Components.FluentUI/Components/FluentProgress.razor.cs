using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentProgress : FluentComponentBase
{
    [Parameter]
    public int? Min { get; set; }

    [Parameter]
    public int? Max { get; set; }

    [Parameter]
    public string? Value { get; set; }

    [Parameter]
    public bool? Paused { get; set; }
}