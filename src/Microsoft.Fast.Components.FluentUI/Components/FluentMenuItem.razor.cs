using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentMenuItem : FluentComponentBase
{
    /// <summary>
    /// Gets or sets if the element is disabled
    /// </summary>
    [Parameter]
    public bool? Disabled { get; set; }

    /// <summary>
    /// Gets or sets if the element is checked
    /// </summary>
    [Parameter]
    public bool? Checked { get; set; }
}