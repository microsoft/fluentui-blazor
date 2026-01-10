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
/// <typeparam name="TOption"></typeparam>
/// <typeparam name="TValue"></typeparam>
public partial class FluentSelect<TOption, TValue> : FluentListBase<TOption, TValue>
{
    private const string JAVASCRIPT_FILE = FluentJSModule.JAVASCRIPT_ROOT + "List/FluentSelect.razor.js";

    /// <summary />
    public FluentSelect(LibraryConfiguration configuration) : base(configuration) { }

    /// <summary />
    protected virtual string DropdownType => "dropdown";

    /// <summary />
    protected virtual string? DropdownStyle => new StyleBuilder()
        .AddStyle("width", Width, when: !string.IsNullOrEmpty(Width))
        .Build();

    /// <summary />
    protected virtual string? ListStyle => new StyleBuilder()
        .AddStyle("min-width", Width, when: !string.IsNullOrEmpty(Width))
        .AddStyle("height", Height, when: !string.IsNullOrEmpty(Height))
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
            // Import the JavaScript module
            await JSModule.ImportJavaScriptModuleAsync(JAVASCRIPT_FILE);

            // By default, the combobox text is not bound to the Value property.
            // This method don't change the SelectedItems and Value properties.
            if (string.Equals(DropdownType, "combobox", StringComparison.Ordinal) && Value is TOption optionValue)
            {
                var defaultText = GetOptionText(optionValue);
                await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Select.SetComboBoxValue", Id, defaultText);
            }
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    /// <summary>
    /// Asynchronously clears the current value.
    /// </summary>
    public async Task ClearAsync()
    {
        await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Select.ClearValue", Id);

        CurrentValueAsString = null;

        SelectedItems = [];
        if (SelectedItemsChanged.HasDelegate)
        {
            await SelectedItemsChanged.InvokeAsync(SelectedItems);
        }
    }

    internal override async Task OnDropdownChangeHandlerAsync(DropdownEventArgs e)
    {
        // List of IDs received from the web component.
        var selectedIds = e.SelectedOptions?.Split(';', StringSplitOptions.TrimEntries) ?? Array.Empty<string>();
        SelectedItems = selectedIds.Length > 0
                      ? InternalOptions.Where(kvp => selectedIds.Contains(kvp.Key, StringComparer.Ordinal)).Select(kvp => kvp.Value).ToList()
                      : Array.Empty<TOption>();

        if (SelectedItemsChanged.HasDelegate)
        {
            await SelectedItemsChanged.InvokeAsync(SelectedItems);
        }

        if (ValueChanged.HasDelegate)
        {
            if (InternalOptions.Count == 0 && selectedIds.Length > 0)
            {
                var result = await JSModule.ObjectReference.InvokeAsync<string>("Microsoft.FluentUI.Blazor.Select.GetSelectedValue", selectedIds.FirstOrDefault());
                if (TryParseValueFromString(result, out var currentValue, out _))
                {
                    await ValueChanged.InvokeAsync(currentValue);
                }
            }
            else
            {
                if (SelectedItems.FirstOrDefault() is TValue value)
                {
                    await ValueChanged.InvokeAsync(value);
                }
                else if (TryParseValueFromString(CurrentValueAsString, out var parsedValue, out _))
                {
                    await ValueChanged.InvokeAsync(parsedValue);
                }
            }
        }
    }
}
