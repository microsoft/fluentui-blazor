// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components.Rendering;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A FreeOptionOutput allows one option to be written in free form.
/// </summary>
public class FreeOptionOutput : FluentComponentBase
{
    /// <summary />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "output");
        builder.AddAttribute(1, "id", Id);
        builder.AddAttribute(2, "class", DefaultClassBuilder.Build());
        builder.AddAttribute(3, "style", DefaultStyleBuilder.Build());
        builder.AddMultipleAttributes(4, AdditionalAttributes);
        builder.CloseElement();
    }
}
