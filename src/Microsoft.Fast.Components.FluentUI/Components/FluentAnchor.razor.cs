using Microsoft.AspNetCore.Components;


namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentAnchor : FluentComponentBase
{
    /// <summary>
    /// If set to a URL, clicking the button will open the referenced document. 
    /// Use Target parameter to specify where.
    /// </summary>
    [Parameter]
    public string? Href { get; set; }

    /// <summary>
    /// Gets or sets the visual appearance. See <seealso cref="Appearance"/>
    /// </summary>
    [Parameter]
    public Appearance? Appearance { get; set; }

    /// <summary>
    /// The target attribute specifies where to open the link, if Href is specified. 
    /// Possible values: _blank | _self | _parent | _top.
    /// </summary>
    [Parameter]
    public string? Target { get; set; }
}