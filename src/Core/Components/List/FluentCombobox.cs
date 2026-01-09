// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A FluentCombobox allows one option to be selected from multiple items.
/// </summary>
public partial class FluentCombobox<TOption, TValue> : FluentSelect<TOption, TValue>
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

    /// <summary>
    /// Gets or sets the icon to display as an indicator for this component.
    /// </summary>
    [Parameter]
    public Icon? Indicator { get; set; }

    /// <summary />
    protected override RenderFragment? RenderFreeFormOption()
    {
        return FreeOption;
    }

    /// <summary />
    protected override RenderFragment? RenderExtraFragment()
    {
        if (Indicator is not null)
        {
            return builder =>
            {
                builder.AddMarkupContent(0, Indicator.ToMarkup(slotName: "indicator", role: "button").Value);
            };
        }

        return null;
    }
}
