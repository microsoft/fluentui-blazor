using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentRadio : FluentComponentBase
{
    /// <summary>
    /// Gets or sets if the element is readonly
    /// </summary>
    [Parameter]
    public bool Readonly { get; set; }

    /// <summary>
    /// The id attribute of the element.Used for label association.
    /// </summary>
    [Parameter]
    public string? Id { get; set; }

    /// <summary>
    /// The value of the element
    /// </summary>
    [Parameter]
    public string? Value { get; set; }

    /// <summary>
    /// The element's current value 
    /// </summary>
    [Parameter]
    public string? CurrentValue { get; set; }

    /// <summary>
    /// Disables the form control, ensuring it doesn't participate in form submission
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// The name of the element.Allows access by name from the associated form.
    /// </summary>
    [Parameter]
    public string? Name { get; set; }

    /// <summary>
    /// The element needs to have a value
    /// </summary>
    [Parameter]
    public bool Required { get; set; }
    /// <summary>
    /// Gets or sets if the element is checked
    /// </summary>
    [Parameter]
    public bool? Checked { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}