using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

[CascadingTypeParameter(nameof(TOption))]
public partial class FluentCombobox<TOption> : ListBase<TOption>
{
    /// <summary>
    /// Gets or sets if the element is auto completes. See <seealso cref="FluentUI.ComboboxAutocomplete"/>
    /// </summary>
    [Parameter]
    public ComboboxAutocomplete? Autocomplete { get; set; }

    /// <summary>
    /// The open attribute.
    /// </summary>
    [Parameter]
    public bool? Open { get; set; }

    /// <summary>
    /// Sets the placeholder value of the element, generally used to provide a hint to the user.
    /// </summary>
    [Parameter]
    public string? Placeholder { get; set; }

    /// <summary>
    /// The placement for the listbox when the combobox is open.
    /// See <seealso cref="FluentUI.SelectPosition"/>
    /// </summary>
    [Parameter]
    public SelectPosition? Position { get; set; }

    /// <summary>
    /// Gets or sets the visual appearance. See <seealso cref="FluentUI.Appearance"/>
    /// </summary>
    [Parameter]
    public Appearance? Appearance { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // an item may have been selected through the data
            if (Items != null && Items.Any() && SelectedItem == null && InternalValue == null)
            {
                if (OptionSelected is not null)
                    SelectedItem = Items.FirstOrDefault(i => OptionSelected(i));
                else if (OptionDisabled is not null)
                    SelectedItem = Items.FirstOrDefault(i => !OptionDisabled(i));

            }
            await RaiseChangedEvents();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    protected override async Task OnChangedHandlerAsync(ChangeEventArgs e)
    {
        if (e.Value is not null && Items is not null)
        {
            string? value = e.Value.ToString();
            TOption? item = Items.FirstOrDefault(i => GetOptionText(i) == value);

            if (item is null)
            {
                SelectedItem = default;

                if (SelectedItemChanged.HasDelegate)
                    await SelectedItemChanged.InvokeAsync(SelectedItem);

                if (ValueChanged.HasDelegate)
                    await ValueChanged.InvokeAsync(value);

                StateHasChanged();
            }
            else
                await OnSelectedItemChangedHandlerAsync(item);
        }
    }

}