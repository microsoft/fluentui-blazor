using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

[CascadingTypeParameter(nameof(TOption))]
public partial class FluentCombobox<TOption> : ListComponentBase<TOption> where TOption : notnull
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
    public SelectPosition? Position { get; set; } = SelectPosition.Below;

    /// <summary>
    /// Gets or sets the visual appearance. See <seealso cref="FluentUI.Appearance"/>
    /// </summary>
    [Parameter]
    public Appearance? Appearance { get; set; }

    protected override string? StyleValue => new StyleBuilder(Style)
        .AddStyle("width", Width, when: !string.IsNullOrEmpty(Width))
        .AddStyle("min-width", Width, when: !string.IsNullOrEmpty(Width))
        .Build();

    protected override async Task OnChangedHandlerAsync(ChangeEventArgs e)
    {
        if (e.Value is not null && Items is not null)
        {
            string? value = e.Value.ToString();
            TOption? item = Items.FirstOrDefault(i => GetOptionText(i) == value);

            if (item is null)
            {
                SelectedOption = default;

                if (SelectedOptionChanged.HasDelegate)
                    await SelectedOptionChanged.InvokeAsync(SelectedOption);

                if (ValueChanged.HasDelegate)
                    await ValueChanged.InvokeAsync(value);

                StateHasChanged();
            }
            else
                await OnSelectedItemChangedHandlerAsync(item);
        }
    }

    protected override string? GetOptionValue(TOption? item)
    {
        if (item != null)
            return OptionText.Invoke(item) ?? OptionValue.Invoke(item) ?? item.ToString();
        else
            return null;
    }
}