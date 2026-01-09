// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The Option element is used to define an item contained in a List component.
/// </summary>
public partial class FluentOption<TValue> : FluentComponentBase
{
    /// <summary />
    public FluentOption(LibraryConfiguration configuration) : base(configuration) { }

    /// <summary>
    /// Gets or sets the context of the list.
    /// </summary>
    [CascadingParameter(Name = "ListContext")]
    private InternalListContext<TValue>? InternalListContext { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the element is disabled.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets the value of this option.
    /// </summary>
    [Parameter]
    public TValue? Value { get; set; }

    /// <summary>
    /// Gets or sets the name of this option.
    /// </summary>
    [Parameter]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the text to display in the dropdown when the option is selected
    /// </summary>
    [Parameter]
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets the value indicating whether the element is selected.
    /// </summary>
    [Parameter]
    public bool Selected { get; set; }

    /// <summary>
    /// Gets or sets the content to display below the main option text.
    /// This can be used to add additional textual information (no markup) about the option.
    /// </summary>
    [Parameter]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary />
    private string? ValueFormatted
    {
        get
        {
            if (InternalListContext is null ||
                InternalListContext.ListComponent.OptionValueFormatted is null)
            {
                return Value?.ToString();
            }

            return InternalListContext.ListComponent.OptionValueFormatted.Invoke(Value);
        }
    }

    /// <summary />
    protected override Task OnInitializedAsync()
    {
        if (InternalListContext is not null)
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Identifier.NewId();
            }

            InternalListContext.AddOption(this);

            // Use OptionSelectedComparer if available, otherwise fallback to EqualityComparer<TValue>.Default
            if (Value is not null)
            {
                var comparer = InternalListContext.ListComponent.OptionSelectedComparer;
                if ((comparer != null && comparer(InternalListContext.ListComponent.Value, Value)) ||
                    (comparer == null && EqualityComparer<TValue>.Default.Equals(InternalListContext.ListComponent.Value, Value)))
                {
                    Selected = true;
                }
            }
        }

        return Task.CompletedTask;
    }

    /// <summary />
    public override async ValueTask DisposeAsync()
    {
        InternalListContext?.RemoveOption(this);

        await base.DisposeAsync();
    }
}
