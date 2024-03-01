// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

[CascadingTypeParameter(nameof(TOption))]
public partial class FluentCombobox<TOption> : ListComponentBase<TOption> where TOption : notnull
{
    /// <summary>
    /// Gets or sets a value indicating whether the element auto completes. See <seealso cref="AspNetCore.Components.ComboboxAutocomplete"/>
    /// </summary>
    [Parameter]
    public ComboboxAutocomplete? Autocomplete { get; set; }

    /// <summary>
    /// Gets or sets the open attribute.
    /// </summary>
    [Parameter]
    public bool? Open { get; set; }

    /// <summary>
    /// Gets or sets the placeholder value of the element, generally used to provide a hint to the user.
    /// </summary>
    [Parameter]
    public string? Placeholder { get; set; }

    /// <summary>
    /// Gets or sets the placement for the listbox when the combobox is open.
    /// See <seealso cref="AspNetCore.Components.SelectPosition"/>
    /// </summary>
    [Parameter]
    public SelectPosition? Position { get; set; } = SelectPosition.Below;

    /// <summary>
    /// Gets or sets the visual appearance. See <seealso cref="AspNetCore.Components.Appearance"/>
    /// </summary>
    [Parameter]
    public Appearance? Appearance { get; set; }

    protected override string? StyleValue => new StyleBuilder(base.StyleValue)
        .AddStyle("min-width", Width, when: !string.IsNullOrEmpty(Width))
        .Build();

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        parameters.SetParameterProperties(this);

        var isSetSelectedOption = false;
        TOption? newSelectedOption = default;

        foreach (var parameter in parameters)
        {
            switch (parameter.Name)
            {
                case nameof(SelectedOption):
                    isSetSelectedOption = true;
                    newSelectedOption = (TOption?)parameter.Value;
                    break;
                default:
                    break;
            }
        }

        if (isSetSelectedOption && !Equals(_currentSelectedOption, newSelectedOption))
        {
            if (Items != null)
            {
                if (Items.Contains(newSelectedOption))
                {
                    _currentSelectedOption = newSelectedOption;
                }
                else
                {
                    // If the selected option is not in the list of items, reset the selected option
                    _currentSelectedOption = SelectedOption = default;
                    await SelectedOptionChanged.InvokeAsync(SelectedOption);
                }
            }
            else
            {
                // If Items is null, we don't know if the selected option is in the list of items, so we just set it
                _currentSelectedOption = newSelectedOption;
            }

            // Sync Value from selected option.
            // If it is null, we set it to the default value so the attribute is not deleted & the webcomponents don't throw an exception
            Value = GetOptionValue(_currentSelectedOption) ?? string.Empty;
            await ValueChanged.InvokeAsync(Value);
        }

        await base.SetParametersAsync(ParameterView.Empty);
    }

    protected async Task OnChangedHandlerAsync(ChangeEventArgs e)
    {
        if (e.Value is not null && Items is not null)
        {
            var value = e.Value.ToString();
            TOption? item = Items.FirstOrDefault(i => GetOptionText(i) == value);

            if (item is null)
            {
                SelectedOption = default;

                if (SelectedOptionChanged.HasDelegate)
                {
                    await SelectedOptionChanged.InvokeAsync(SelectedOption);
                }

                if (ValueChanged.HasDelegate)
                {
                    Value = value;
                    await ValueChanged.InvokeAsync(value);
                }

                StateHasChanged();
            }
            else
            {
                await OnSelectedItemChangedHandlerAsync(item);
            }
        }
    }

    protected override string? GetOptionValue(TOption? item)
    {
        if (item != null)
        {
            return OptionText.Invoke(item) ?? OptionValue.Invoke(item) ?? item.ToString();
        }
        else
        {
            return null;
        }
    }
}
