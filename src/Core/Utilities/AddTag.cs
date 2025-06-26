// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components.Utilities;

/// <summary>
/// Internal component to conditionally render a tag and its content.
/// </summary>
public class AddTag : ComponentBase
{
    /// <summary>
    /// Gets or sets the name of the tag.
    /// </summary>
    [Parameter]
    public virtual required string Name { get; set; }

    /// <summary>
    /// Gets or sets the condition to add the tag <see cref="Name"/>.
    /// </summary>
    [Parameter]
    public Func<bool> TagWhen { get; set; } = () => true;

    /// <summary>
    /// Gets or sets the condition to add the <see cref="ChildContent"/>.
    /// </summary>
    [Parameter]
    public virtual Func<bool> ContentWhen { get; set; } = () => true;

    /// <summary>
    /// Gets or sets the content inside this dynamic element.
    /// </summary>
    [Parameter]
    public virtual RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets a collection of additional attributes that will be applied to the created element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public virtual IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    /// <summary />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (string.IsNullOrEmpty(Name))
        {
            throw new InvalidOperationException("Name property is required.");
        }

        var addTag = TagWhen();
        var addContent = ContentWhen();

        if (addTag && addContent)
        {
            builder.OpenElement(0, Name);
            builder.AddMultipleAttributes(1, AdditionalAttributes);
            builder.AddContent(2, ChildContent);
            builder.CloseElement();
        }
        else if (addContent)
        {
            builder.AddContent(3, ChildContent);
        }
        else if (addTag)
        {
            builder.OpenElement(4, Name);
            builder.AddMultipleAttributes(5, AdditionalAttributes);
            builder.CloseElement();
        }
    }
}
