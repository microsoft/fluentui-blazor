// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components.Rendering;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A FreeOptionOutput allows one option to be written in free form.
/// </summary>
public class FreeOptionOutput : FluentComponentBase
{
    /// <summary />
    public FreeOptionOutput(LibraryConfiguration configuration) : base(configuration) { }

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
