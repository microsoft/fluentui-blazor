
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Components.FluentUI
{
    public class NavLinkFluentAnchor : NavLink
    {       
        [Parameter] public string Href { get; set; }

        [Parameter] public Appearance? Appearance { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "fluent-anchor");
            builder.AddAttribute(1, "href", Href);
            builder.AddAttribute(2, "appearance", Appearance.ToAttributeValue());
            builder.AddMultipleAttributes(3, AdditionalAttributes);
            builder.AddAttribute(4, "class", CssClass);
            builder.AddContent(5, ChildContent);

            builder.CloseElement();
        }
    }
}