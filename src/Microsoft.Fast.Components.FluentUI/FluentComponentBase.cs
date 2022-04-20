using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public abstract class FluentComponentBase : ComponentBase
{
    private ElementReference _ref;

    /// <summary>
    /// Gets or sets the associated <see cref="ElementReference"/>.
    /// <para>
    /// May be <see langword="null"/> if accessed before the component is rendered.
    /// </para>
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

    [Parameter]
    public Reference? BackReference { get; set; }



    /// <summary>
    /// Gets or sets the content to be rendered inside the component
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets a collection of additional attributes that will be applied to the created element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }
}

