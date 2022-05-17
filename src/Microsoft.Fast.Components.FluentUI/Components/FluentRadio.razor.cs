using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentRadio : FluentComponentBase
{
    /// <summary>
    /// Gets or sets the current value
    /// </summary>
    [Parameter]
    public string? Value { get; set; }

    /// <summary>
    /// Gets or sets if the element is required
    /// </summary>
    [Parameter]
    public bool? Required { get; set; }

    /// <summary>
    /// Gets or sets if the element is disabled
    /// </summary>
    [Parameter]
    public bool? Disabled { get; set; }

    /// <summary>
    /// Gets or sets if the element is readonly
    /// </summary>
    [Parameter]
    public bool? Readonly { get; set; }

    /// <summary>
    /// Gets or sets if the element is checked
    /// </summary>
    [Parameter]
    public bool? Checked { get; set; }
}