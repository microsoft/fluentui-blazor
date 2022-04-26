using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentRadio : FluentComponentBase
{
    [Parameter]
    public string? Value { get; set; }

    [Parameter]
    public bool? Required { get; set; }

    [Parameter]
    public bool? Disabled { get; set; }

    [Parameter]
    public bool? Readonly { get; set; }

    [Parameter]
    public bool? Checked { get; set; }
}