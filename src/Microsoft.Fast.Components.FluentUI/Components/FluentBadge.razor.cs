using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.Fast.Components.FluentUI
{
    public partial class FluentBadge
    {
        [Parameter]
        public Color? Color { get; set; }

        [Parameter]
        public Fill? Fill { get; set; }

        [Parameter]
        public Appearance? Appearance { get; set; }

        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public IDictionary<string, object>? AdditionalAttributes { get; set; }
    }
}