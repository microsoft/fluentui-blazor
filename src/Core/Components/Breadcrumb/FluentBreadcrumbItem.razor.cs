using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;
public partial class FluentBreadcrumbItem : FluentComponentBase
{
    /// <summary>
    /// Prompts the user to save the linked URL. See <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/a">a element</see> for more information.
    /// </summary>
    [Parameter]
    public string? Download { get; set; }

    /// <summary>
    /// Gets or sets the URL the hyperlink references. See <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/a">a element</see> for more information.
    /// Use Target parameter to specify where.
    /// </summary>
    [Parameter]
    public string? Href { get; set; }

    /// <summary>
    /// Hints at the language of the referenced resource. See <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/a">a element</see> for more information.
    /// </summary>
    [Parameter]
    public string? Hreflang { get; set; }

    /// <summary>
    /// See <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/a">a element</see> for more information.
    /// </summary>
    [Parameter]
    public string? Ping { get; set; }

    /// <summary>
    /// See <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/a">a element</see> for more information.
    /// </summary>
    [Parameter]
    public string? Referrerpolicy { get; set; }

    /// <summary>
    /// See <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/a">a element</see> for more information.
    /// </summary>
    [Parameter]
    public string? Rel { get; set; }

    /// <summary>
    /// Gets or sets the target attribute that specifies where to open the link, if Href is specified. 
    /// See <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/a">a element</see> for more information.
    /// Possible values: _blank | _self | _parent | _top.
    /// </summary>
    [Parameter]
    public string? Target { get; set; }

    /// <summary>
    /// See <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/a">a element</see> for more information.
    /// </summary>
    [Parameter]
    public string? Type { get; set; }

    /// <summary>
    /// Gets or sets the visual appearance. See <seealso cref="AspNetCore.Components.Appearance"/>
    /// </summary>
    [Parameter]
    public Appearance? Appearance { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    protected override void OnParametersSet()
    {
        string[] values = { "_self", "_blank", "_parent", "_top" };
        if (!string.IsNullOrEmpty(Target) && !values.Contains(Target))
        {
            throw new ArgumentException("Target must be one of the following values: _self, _blank, _parent, _top");
        }

        base.OnParametersSet();
    }
}
