// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
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
        bool addTag = TagWhen();
        bool addContent = ContentWhen();

        if (addTag && addContent)
        {
            builder.OpenElement(0, Name);
            builder.AddAttribute(1, "b-ui");
            builder.AddMultipleAttributes(2, AdditionalAttributes);
            builder.AddContent(3, ChildContent);
            builder.CloseElement();
        }
        else if (addContent)
        {
            builder.AddContent(4, ChildContent);
        }
        else if (addTag)
        {
            builder.OpenElement(5, Name);
            builder.AddAttribute(6, "b-ui");
            builder.AddMultipleAttributes(7, AdditionalAttributes);
            builder.CloseElement();
        }
    }
}
