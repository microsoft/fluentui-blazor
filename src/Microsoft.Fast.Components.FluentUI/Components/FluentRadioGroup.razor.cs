using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.Fast.Components.FluentUI
{
    public partial class FluentRadioGroup
    {
        [Parameter]
        public string? Name { get; set; }

        [Parameter]
        public bool? Required { get; set; }

        [Parameter]
        public Orientation? Orientation { get; set; }

        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        protected override bool TryParseValueFromString(string? value, out string? result, [NotNullWhen(false)] out string? validationErrorMessage)
        {
            result = value;
            validationErrorMessage = null;
            return true;
        }
    }
}