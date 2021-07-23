using System.Collections.Generic;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Microsoft.Fast.Components.FluentUI
{
    public class FluentDesignSystemProvider : ComponentBase
    {
        [Parameter] public RenderFragment? ChildContent { get; set; }
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

        /// <inheritdoc />
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "fluent-design-system-provider");

            if (AdditionalAttributes != null && AdditionalAttributes.Count > 0) {
                builder.AddMultipleAttributes(1, AdditionalAttributes);
            }

            builder.AddContent(2, ChildContent);
            builder.CloseElement();
        }
    }
}