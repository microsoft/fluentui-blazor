// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

[CascadingTypeParameter(nameof(TOption))]
public partial class FluentCombobox<TOption> : ListComponentBase<TOption>, IAsyncDisposable where TOption : notnull
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
    /// Gets or sets the option to allow closing the FluentCombobox list by clicking the dropdown button. Default is false.
    /// </summary>
    [Parameter]
    public bool? EnableClickToClose { get; set; } = false;

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

                if (EnableClickToClose ?? true)
                {
                    await Module.InvokeVoidAsync("attachIndicatorClickHandler", Id);
                }
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
            var item = Items.FirstOrDefault(i => GetOptionText(i) == value);

            if (item is null)
            {
                SelectedOption = default;
                await SelectedOptionChanged.InvokeAsync(SelectedOption);
            }
            else
            {
                await InvokeAsync(async () => await OnSelectedItemChangedHandlerAsync(item));
            }

            if (Value != value)
            {
                await base.ChangeHandlerAsync(e);
            }
        }
    }

    private string? GetComboboxContent()
    {
        if (SelectedOption != null)
        {
            return OptionText.Invoke(SelectedOption) ?? OptionValue.Invoke(SelectedOption) ?? SelectedOption.ToString();
        }
        else
        {
            return null;
        }
    }

    public new async ValueTask DisposeAsync()
    {
        try
        {
            if (Module is not null && !string.IsNullOrEmpty(Id))
            {
                await Module.InvokeVoidAsync("detachIndicatorClickHandler", Id);
                await Module.DisposeAsync();
            }
            await base.DisposeAsync();
        }
        catch (Exception ex) when (ex is JSDisconnectedException ||
                                   ex is OperationCanceledException)
        {
            // The JSRuntime side may routinely be gone already if the reason we're disposing is that
            // the client disconnected. This is not an error.
        }
    }
}
