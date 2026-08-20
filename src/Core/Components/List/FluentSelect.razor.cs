// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A FluentSelect allows for selecting one or more options from a list of options.
/// </summary>
[CascadingTypeParameter(nameof(TValue))]
public partial class FluentSelect<TOption, TValue> : FluentListBase<TOption, TValue>, IFluentControlStyle, IFluentComponentElementBase
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

    /// <inheritdoc cref="IFluentComponentElementBase.Element" />
    [Parameter]
    public ElementReference Element { get; set; }

    /// <inheritdoc cref="IFluentControlStyle.ControlStyle" />
    [Parameter]
    public string? ControlStyle { get; set; }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // By default, the combobox text is not bound to the Value property.
            // This method don't change the SelectedItems and Value properties.
            var selectedOption = Value is TOption option
                ? option
                : SelectedItems is not null ? SelectedItems.FirstOrDefault() : default;
            var defaultText = selectedOption is not null ? base.GetOptionText(selectedOption) : "";
            await JSRuntime.InvokeFluentVoidAsync("Microsoft.FluentUI.Blazor.Components.Select.Initialize", Id, defaultText);

            if (!string.IsNullOrEmpty(ControlStyle))
            {
                await JSRuntime.InvokeFluentVoidAsync("Microsoft.FluentUI.Blazor.Utilities.Attributes.applyShadowStyle", Element, ":host .control", ControlStyle);
            }
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    /// <summary>
    /// Asynchronously clears the current value.
    /// </summary>
    public async Task ClearAsync()
    {
        await JSRuntime.InvokeFluentVoidAsync("Microsoft.FluentUI.Blazor.Components.Select.ClearValue", Id);

        CurrentValueAsString = null;

        SelectedItems = [];
        if (SelectedItemsChanged.HasDelegate)
        {
            await SelectedItemsChanged.InvokeAsync(SelectedItems);
        }

        NotifyValidationFieldChanged();
    }
}
