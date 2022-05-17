using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;
public partial class FluentBadge : FluentComponentBase
{
    /// <summary>
    /// Gets or sets the color
    /// </summary>
    [Parameter]
    public Color? Color { get; set; }

    /// <summary>
    /// Gets or sets the background color
    /// </summary>
    [Parameter]
    public Fill? Fill { get; set; }

    /// <summary>
    /// Gets or sets the visual appearance. See <seealso cref="FluentUI.Appearance"/>
    /// </summary>
    [Parameter]
    public Appearance? Appearance { get; set; }
}