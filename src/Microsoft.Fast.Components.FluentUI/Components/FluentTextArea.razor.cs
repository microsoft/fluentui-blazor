using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI
{
    public partial class FluentTextArea
    {
        [Parameter]
        public bool? Disabled { get; set; }

        [Parameter]
        public bool? Readonly { get; set; }

        [Parameter]
        public bool? Required { get; set; }

        [Parameter]
        public bool? Autofocus { get; set; }

        [Parameter]
        public Resize? Resize { get; set; }

        [Parameter]
        public Appearance? Appearance { get; set; }

        [Parameter]
        public string? Placeholder { get; set; }

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