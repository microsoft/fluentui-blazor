using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public abstract class FluentComponentBase : ComponentBase
{
    private ElementReference _ref;

    /// <summary>
    /// The associated web component. 
    /// May be <see langword="null"/> if accessed before the component is rendered.
    /// </summary>
    public ElementReference Element
    {
        get => _ref;
        protected set
        {
            _ref = value;
            BackReference?.Set(value);
        }
    }

    /// <summary>
    /// Unique identifier. If not provided, a random value will be generated.
    /// The value will be used as the HTML <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/id">global id attribute</see>.
    /// </summary>
    [Parameter]
    public string? Id { get; set; }

    /// <summary>
    /// Optional CSS class names. If given, these will be included in the class attribute of the component.
    /// </summary>
    [Parameter]
    public virtual string? Class { get; set; } = null;

    /// <summary>
    /// Optional in-line styles. If given, these will be included in the style attribute of the component.
    /// </summary>
    [Parameter]
    public virtual string? Style { get; set; }

    /// <summary>
    /// Used to attach any user data object to the component.
    /// </summary>
    [Parameter]
    public virtual object? Data { get; set; } = null;

    /// <summary>
    /// A reference to the enclosing component.
    /// </summary>
    [Parameter]
    public virtual Reference? BackReference { get; set; }

    /// <summary>
    /// Gets or sets a collection of additional attributes that will be applied to the created element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public virtual IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    protected string? GetId()
    {
        return string.IsNullOrEmpty(Id) ? null : Id;
    }
}

