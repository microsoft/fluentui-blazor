using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;
public partial class FluentBadge : FluentComponentBase
{
    /// <summary>
    /// Gets or sets the color
    /// </summary>
    [Parameter]
    public string? Color { get; set; }

    /// <summary>
    /// Gets or sets the background color
    /// </summary>
    [Parameter]
    public string? Fill { get; set; }

    /// <summary>
    /// Gets or sets if the badge is rendered circular
    /// </summary>
    [Parameter]
    public bool Circular { get; set; } = false;

    /// <summary>
    /// Gets or sets the visual appearance. See <seealso cref="FluentUI.Appearance"/>
    /// </summary>
    [Parameter]
    public Appearance? Appearance { get; set; } = null;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}