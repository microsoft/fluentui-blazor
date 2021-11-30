using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI
{
    public partial class FluentTextField
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
        public int? Size { get; set; }

        [Parameter]
        public Appearance? Appearance { get; set; }

        [Parameter]
        public TextFieldType? TextFieldType { get; set; }

        [Parameter]
        public string? Placeholder { get; set; }

        [Parameter]
        public int? MinLength { get; set; }

        [Parameter]
        public int? MaxLength { get; set; }

        [Parameter]
        public bool? Spellcheck { get; set; }

        //Pattern
        //List
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