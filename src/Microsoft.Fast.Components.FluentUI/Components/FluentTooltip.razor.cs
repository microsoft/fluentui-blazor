using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.Fast.Components.FluentUI
{
    public partial class FluentTooltip
    {
        [Parameter]
        public string? Anchor { get; set; }

        [Parameter]
        public TooltipPosition? Position { get; set; }

        [Parameter]
        public int? Delay { get; set; } = 300;
        [Parameter]
        public bool? Visible { get; set; } = false;
        [Parameter]
        public bool? HorizontalViewportLock { get; set; } = false;
        [Parameter]
        public bool? VerticalViewportLock { get; set; } = false;
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public IDictionary<string, object>? AdditionalAttributes { get; set; }
    }
}