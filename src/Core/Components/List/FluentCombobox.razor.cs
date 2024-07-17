// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

[CascadingTypeParameter(nameof(TOption))]
public partial class FluentCombobox<TOption> : ListComponentBase<TOption> where TOption : notnull
{
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/List/FluentCombobox.razor.js";

    /// <summary />
    [Inject]
    private LibraryConfiguration LibraryConfiguration { get; set; } = default!;

    /// <summary />
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary />
    private IJSObjectReference? Module { get; set; }

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

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                Module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE.FormatCollocatedUrl(LibraryConfiguration));
                await Module.InvokeVoidAsync("setControlAttribute", Id, "autocomplete", "off");
            }
        }
    }

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
                else if (OptionSelected != null && newSelectedOption != null && OptionSelected(newSelectedOption))
                {
                    // The selected option might not be part of the Items list. But we can use OptionSelected to compare the current option.
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
            var value = GetOptionValue(_currentSelectedOption) ?? string.Empty;
            if (Value != value)
            {
                Value = value;
                await ValueChanged.InvokeAsync(Value);
            }
        }

        await base.SetParametersAsync(ParameterView.Empty);
    }

    protected override async Task ChangeHandlerAsync(ChangeEventArgs e)
    {

        if (e.Value is not null && Items is not null)
        {
            var value = e.Value.ToString();
            TOption? item = Items.FirstOrDefault(i => GetOptionText(i) == value);

            if (item is null)
            {
                SelectedOption = default;
                await base.ChangeHandlerAsync(e);
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
