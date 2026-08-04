// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A FluentSelect allows for selecting one or more options from a list of options.
/// </summary>
[CascadingTypeParameter(nameof(TValue))]
public partial class FluentSelect<TOption, TValue> : FluentListBase<TOption, TValue>
{
    /// <summary />
    public FluentSelect(LibraryConfiguration configuration) : base(configuration) { }

    /// <summary />
    protected virtual string DropdownType => "dropdown";

    /// <summary />
    protected virtual string? DropdownStyle => new StyleBuilder()
        .Build();

    /// <summary />
    protected virtual string? ListStyle => new StyleBuilder()
        .AddStyle("max-height", Height, when: !string.IsNullOrEmpty(Height))
        .Build();

    /// <summary>
    /// Gets or sets the placeholder text to display when no item is selected.
    /// </summary>
    [Parameter]
    public string? Placeholder { get; set; }

    /// <summary>
    /// Gets or sets the size of the list.
    /// Default is `null`. Internally the component uses <see cref="ListSize.Medium"/> as default.
    /// </summary>
    [Parameter]
    public ListSize? Size { get; set; }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // By default, the combobox text is not bound to the Value property.
            // This method don't change the SelectedItems and Value properties.
            if (string.Equals(DropdownType, "combobox", StringComparison.Ordinal))
            {
                var defaultText = Value is TOption option ? base.GetOptionText(option) : "";
                await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.Select.SetComboBoxValue", Id, defaultText);
            }
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    /// <summary>
    /// Asynchronously clears the current value.
    /// </summary>
    public async Task ClearAsync()
    {
        await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.Select.ClearValue", Id);

        CurrentValueAsString = null;

        SelectedItems = [];
        if (SelectedItemsChanged.HasDelegate)
        {
            await SelectedItemsChanged.InvokeAsync(SelectedItems);
        }

        NotifyValidationFieldChanged();
    }
}
