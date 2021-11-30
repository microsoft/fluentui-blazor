using System.Collections.Generic;

using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI
{
    public partial class FluentDialog
    {
        [Parameter]
        public bool? Modal { get; set; }

        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public IDictionary<string, object>? AdditionalAttributes { get; set; }

        [Parameter]
        public bool Hidden { get; set; } = false;
        public void Show() => Hidden = false;
        public void Hide() => Hidden = true;
    }
}