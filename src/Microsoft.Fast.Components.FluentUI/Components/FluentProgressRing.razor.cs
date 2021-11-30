using System.Collections.Generic;

using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI
{
    public partial class FluentProgressRing
    {
        [Parameter]
        public int? Min { get; set; }

        [Parameter]
        public int? Max { get; set; }

        [Parameter]
        public string? Value { get; set; }

        [Parameter]
        public bool? Paused { get; set; }

        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public IDictionary<string, object>? AdditionalAttributes { get; set; }
    }
}