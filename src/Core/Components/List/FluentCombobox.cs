// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A FluentCombobox allows one option to be selected from multiple items.
/// </summary>
/// <typeparam name="TOption"></typeparam>
public partial class FluentCombobox<TOption> : FluentSelect<TOption>
{
    /// <summary />
    public FluentCombobox(LibraryConfiguration configuration) : base(configuration) { }

    /// <summary />
    protected override string DropdownType => "combobox";

    /// <summary>
    /// Gets or sets whether the combobox allows free form entry.
    /// Use the <see cref="FreeOptionOutput"/> component to display the user entry.
    /// If the someone types a string that does not match any option in the list,
    /// you can allow submission of their free form entry by using this parameter.
    /// </summary>
    [Parameter]
    public RenderFragment? FreeOption { get; set; }

    /// <summary />
    protected override RenderFragment? RenderFreeFormOption()
    {
        return FreeOption;
    }
}
