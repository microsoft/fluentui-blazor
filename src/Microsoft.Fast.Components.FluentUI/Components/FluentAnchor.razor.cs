using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentAnchor : FluentComponentBase
{
    /// <summary>
    /// Prompts the user to save the linked URL. <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/a">a element</see> for more information.
    /// </summary>
    [Parameter]
    public string? Download { get; set; }

    /// <summary>
    /// The URL the hyperlink references. <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/a">a element</see> for more information.
    /// Use Target parameter to specify where.
    /// </summary>
    [Parameter]
    public string? Href { get; set; }

    /// <summary>
    /// Hints at the language of the referenced resource. <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/a">a element</see> for more information.
    /// </summary>
    [Parameter]
    public string? Hreflang { get; set; }

    /// <summary>
    /// <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/a">a element</see> for more information.
    /// </summary>
    [Parameter]
    public string? Ping { get; set; }

    /// <summary>
    /// <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/a">a element</see> for more information.
    /// </summary>
    [Parameter]
    public string? Referrerpolicy { get; set; }

    /// <summary>
    /// <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/a">a element</see> for more information.
    /// </summary>
    [Parameter]
    public string? Rel { get; set; }

    /// <summary>
    /// The target attribute specifies where to open the link, if Href is specified. 
    /// Possible values: _blank | _self | _parent | _top.
    /// </summary>
    [Parameter]
    public string? Target { get; set; }

    /// <summary>
    /// <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/a">a element</see> for more information.
    /// </summary>
    [Parameter]
    public string? Type { get; set; }

    /// <summary>
    /// Gets or sets the visual appearance. See <seealso cref="Appearance"/>
    /// </summary>
    [Parameter]
    public Appearance? Appearance { get; set; }
}