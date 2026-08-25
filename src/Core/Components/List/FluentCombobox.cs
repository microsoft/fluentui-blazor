// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A FluentCombobox allows one option to be selected from multiple items.
/// </summary>
[CascadingTypeParameter(nameof(TValue))]
public partial class FluentCombobox<TOption, TValue> : FluentSelect<TOption, TValue>, IFluentInputImmediate
{
    private readonly FluentInputImmediateManager _immediateManager;
    private TValue? _lastSelectedValue;

    /// <summary />
    public FluentCombobox(LibraryConfiguration configuration) : base(configuration)
    {
        _immediateManager = new FluentInputImmediateManager(this);
    }

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

    /// <summary>
    /// Change the content of this input field when the user write text (based on 'OnInput' HTML event).
    /// This mode is not supported when <see cref="FluentListBase{TOption, TValue}.Multiple"/> is enabled
    /// and an <see cref="InvalidOperationException"/> is thrown when both parameters are set to <see langword="true"/>.
    /// </summary>
    [Parameter]
    public bool Immediate { get; set; } = false;

    /// <summary>
    /// Gets or sets the delay, in milliseconds, before to raise the event.
    /// Default is 200 milliseconds.
    /// </summary>
    [Parameter]
    public int ImmediateDelay { get; set; } = 200;

    /// <summary>
    /// Gets or sets the text displayed in the input field.
    /// It is synchronized with the selected option text written to <c>input.value</c> when <c>Value</c> changes.
    /// In immediate mode, user input changes are reported through <see cref="ImmediateTextChanged"/>.
    /// </summary>
    [Parameter]
    public string? ImmediateText { get; set; }

    /// <summary>
    /// Gets or sets the callback to invoke when the <see cref="ImmediateText"/> property changes.
    /// </summary>
    [Parameter]
    public EventCallback<string?> ImmediateTextChanged { get; set; }

    /// <summary>
    /// Raised when the field gains focus. Since the native `onfocusin` DOM event is owned internally
    /// by <see cref="FocusInHandlerAsync"/> (an <c>@onfocusin</c> passed via unmatched attributes would
    /// silently replace it instead of merging), use this callback to observe focus-in from a consumer.
    /// </summary>
    [Parameter]
    public EventCallback<FocusEventArgs> OnFocusIn { get; set; }

    /// <summary>
    /// Raised when the field loses focus. Since the native `onfocusout` DOM event is owned internally
    /// by <see cref="FocusOutHandlerAsync"/> (an <c>@onfocusout</c> passed via unmatched attributes would
    /// silently replace it instead of merging), use this callback to observe focus-out from a consumer.
    /// </summary>
    [Parameter]
    public EventCallback<FocusEventArgs> OnFocusOut { get; set; }

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

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (Multiple && Immediate)
        {
            throw new InvalidOperationException("Immediate mode is not supported when Multiple selection is enabled.");
        }

        if (Multiple)
        {
            return;
        }

        if (firstRender)
        {
            // Initialize the 'immediate' custom event for the immediate mode
            await InitializeImmediateAsync();
        }

        var selectedOption = GetSelectedSingleOption();
        var selectedValue = GetOptionValue(selectedOption);

        if (!EqualityComparer<TValue>.Default.Equals(_lastSelectedValue, selectedValue))
        {
            var selectedText = GetOptionText(selectedOption);
            await JSRuntime.InvokeFluentVoidAsync("Microsoft.FluentUI.Blazor.Components.Select.UpdateValue", Id, selectedText ?? string.Empty);
            await UpdateImmediateTextAsync(selectedText);
        }

        _lastSelectedValue = selectedValue;
    }

    /// <summary />
    private TOption? GetSelectedSingleOption()
    {
        if (Value is not null and TOption selectedValue)
        {
            return selectedValue;
        }

        return SelectedItemsChanged.HasDelegate && SelectedItems is not null
            ? SelectedItems.FirstOrDefault()
            : default(TOption);
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _immediateManager.CheckAndSetExternalValue(parameters, Value, FormatValueAsString);
        return base.SetParametersAsync(parameters);
    }

    /// <see cref="FluentInputImmediateManager.GetImmediateValueAsString(string?)" />
    protected string? ImmediateValueAsString => _immediateManager.GetImmediateValueAsString(CurrentValueAsString);

    /// <see cref="FluentInputImmediateManager.InputHandlerAsync(ChangeEventArgs, Func{ChangeEventArgs, Task})" />
    protected override Task InputHandlerAsync(ChangeEventArgs e) => _immediateManager.InputHandlerAsync(e, ImmediateTextChangeHandlerAsync);

    private Task ImmediateTextChangeHandlerAsync(ChangeEventArgs e) => ImmediateTextChanged.InvokeAsync(e.Value?.ToString());

    /// <see cref="FluentInputImmediateManager.ShouldRender" />
    protected override bool ShouldRender() => _immediateManager.ShouldRender();

    /// <see cref="FluentInputImmediateManager.FocusInHandlerAsync(FocusEventArgs)" />
    protected override Task FocusInHandlerAsync(FocusEventArgs e) => _immediateManager.FocusInHandlerAsync(e);

    /// <see cref="FluentInputImmediateManager.FocusOutHandlerAsync(FocusEventArgs, Func{Task}?)" />
    protected override async Task FocusOutHandlerAsync(FocusEventArgs e)
    {
        // Call the base FocusOutHandlerAsync to handle the focus out event and set FocusLost to true
        await base.FocusOutHandlerAsync(e);
        await _immediateManager.FocusOutHandlerAsync(e, async () =>
        {
            FocusLost = true;

            // Update the ImmediateText property with the selected option text when the combobox loses focus
            var value = GetOptionText(GetSelectedSingleOption());
            await UpdateImmediateTextAsync(value);
        });
    }

    /// <see cref="FluentInputImmediateManager.InitializeImmediateAsync(JSInterop.IJSRuntime, string?)" />
    protected virtual Task InitializeImmediateAsync() => _immediateManager.InitializeImmediateAsync(JSRuntime, Id);

    /// <summary />
    private async Task UpdateImmediateTextAsync(string? value)
    {
        if (!string.Equals(ImmediateText, value, StringComparison.Ordinal))
        {
            ImmediateText = value;

            if (ImmediateTextChanged.HasDelegate)
            {
                await ImmediateTextChanged.InvokeAsync(value);
            }
        }
    }
}
