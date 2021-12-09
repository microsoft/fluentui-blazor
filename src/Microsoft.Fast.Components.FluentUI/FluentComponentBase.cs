using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public class FluentComponentBase : ComponentBase
{
    /// <summary>
    /// Gets or sets the associated <see cref="ElementReference"/>.
    /// <para>
    /// May be <see langword="null"/> if accessed before the component is rendered.
    /// </para>
    /// </summary>
    public ElementReference Element { get; protected set; }

    /// <summary>
    /// Gets or sets the child content to be rendered inside the component
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets a collection of additional attributes that will be applied to the created element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }
}

