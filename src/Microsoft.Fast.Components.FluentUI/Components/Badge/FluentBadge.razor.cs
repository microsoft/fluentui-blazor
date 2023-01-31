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
    /// Possible values are Accent (default), Neutral or Lightweight
    /// </summary>
    [Parameter]
    public Appearance? Appearance { get; set; } = FluentUI.Appearance.Accent;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    protected override Task OnParametersSetAsync()
    {
        if (Appearance != FluentUI.Appearance.Accent && 
            Appearance != FluentUI.Appearance.Lightweight && 
            Appearance != FluentUI.Appearance.Neutral)
        {
            throw new ArgumentException("FluentBadge Appearance needs to be one of Accent, Lightweight or Neutral.");
        }

        return base.OnParametersSetAsync();
    }
}