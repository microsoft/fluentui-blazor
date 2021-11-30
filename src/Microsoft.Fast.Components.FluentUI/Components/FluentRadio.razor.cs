using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.Fast.Components.FluentUI
{
    public partial class FluentRadio
    {
        [Parameter]
        public string? Value { get; set; }

        [Parameter]
        public bool? Required { get; set; }

        [Parameter]
        public bool? Disabled { get; set; }

        [Parameter]
        public bool? Readonly { get; set; }

        [Parameter]
        public bool? Checked { get; set; }

        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public IDictionary<string, object>? AdditionalAttributes { get; set; }
    }
}