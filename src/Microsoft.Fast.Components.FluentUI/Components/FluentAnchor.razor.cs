using Microsoft.AspNetCore.Components;


namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentAnchor : FluentComponentBase
{
    /// <summary>
    /// The URL this item references.
    /// </summary>
    [Parameter]
    public string? Href { get; set; }

    /// <summary>
    /// Gets or sets the visual appearance. See <seealso cref="Appearance"/>
    /// </summary>
    [Parameter]
    public Appearance? Appearance { get; set; }


}