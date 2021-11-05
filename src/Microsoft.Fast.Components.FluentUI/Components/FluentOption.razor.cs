using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI
{
    public partial class FluentOption<TValue> : ComponentBase
    {
        internal FluentOptionContext? Context { get; private set; }

        [Parameter]
        public bool? Disabled { get; set; }

        /// <summary>
        /// Gets or sets the value of this option.
        /// </summary>
        [Parameter]
        public string? Value { get; set; }

        /// <summary>
        /// Gets or sets the name of the parent container component.
        /// </summary>
        [Parameter] public string? Name { get; set; }

        [Parameter]
        public bool? Selected { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public IDictionary<string, object>? AdditionalAttributes { get; set; }

        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [CascadingParameter] private FluentOptionContext? CascadedContext { get; set; }

        /// <inheritdoc />
        protected override void OnParametersSet()
        {
            Context = string.IsNullOrEmpty(Name) ? CascadedContext : CascadedContext?.FindContextInAncestors(Name);

            if (Context == null)
            {
                throw new InvalidOperationException($"{GetType()} must have an ancestor of type FluentSelect, FluentListbox or FluentCombobox " +
                    $"with a matching 'Name' property, if specified.");
            }
        }
    }
}