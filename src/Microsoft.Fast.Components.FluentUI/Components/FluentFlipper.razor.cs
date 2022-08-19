using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentFlipper : FluentComponentBase
{
    /// <summary>
    /// Gets or set if the element is disabled
    /// </summary>
    [Parameter]
    public bool? Disabled { get; set; }

    /// <summary>
	/// Indicates the flipper should be hidden from assistive technology. Because flippers are often supplementary navigation, they are often hidden from assistive technology.
	/// </summary>
	[Parameter]
	public bool? AriaHidden { get; set; }
	
	    /// <summary>
    /// Gets or sets the direction. See <see cref="FluentUI.FlipperDirection"/>
    /// </summary>

    [Parameter]
    public FlipperDirection? Direction { get; set; }
}