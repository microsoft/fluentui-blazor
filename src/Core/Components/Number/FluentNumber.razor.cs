// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A numeric input component that allows users to enter and edit numeric values.
/// </summary>
public partial class FluentNumber<TValue> : FluentInputImmediateBase<TValue>, IFluentComponentElementBase, ITooltipComponent
                                            where TValue : struct, INumber<TValue>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FluentNumber{TValue}"/> class.
    /// </summary>
    public FluentNumber(LibraryConfiguration configuration) : base(configuration)
    {
        // Default conditions for the message
        MessageCondition = (field) =>
        {
            field.MessageIcon = FluentStatus.ErrorIcon;
            field.Message = Localizer[Localization.LanguageResource.TextInput_RequiredMessage];

            return FocusLost &&
                   (Required ?? false)
                   && !(Disabled ?? false)
                   && !ReadOnly
                   && string.IsNullOrEmpty(CurrentValueAsString);
        };

    }

    /// <inheritdoc />
    protected override string? StyleValue => DefaultStyleBuilder
        .AddStyle("width", Width)
        .Build();

    /// <summary>
    /// Gets the CSS class to apply to the internal web-component.
    /// </summary>
    protected virtual string? ComponentStyleValue => new StyleBuilder()
        .AddStyle("width", Width)
        .Build();

    /// <inheritdoc cref="IFluentComponentElementBase.Element" />
    [Parameter]
    public ElementReference Element { get; set; }

    /// <summary>
    /// Gets or sets the visual appearance.
    /// </summary>
    [Parameter]
    public TextInputAppearance Appearance { get; set; } = TextInputAppearance.Outline;

    /// <summary>
    /// Gets or sets the short hint displayed in the input before the user enters a value.
    /// </summary>
    [Parameter]
    public string? Placeholder { get; set; }

    /// <summary>
    /// Gets or sets the content to prefix the input component.
    /// </summary>
    [Parameter]
    public virtual RenderFragment? StartTemplate { get; set; }

    /// <summary>
    /// Gets or sets the content to suffix the input component.
    /// </summary>
    [Parameter]
    public virtual RenderFragment? EndTemplate { get; set; }

    /// <summary>
    /// Gets or sets the width of the input field.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the size of the input. See <see cref="Components.TextInputSize"/>
    /// </summary>
    [Parameter]
    public TextInputSize? Size { get; set; }

    /// <inheritdoc cref="ITooltipComponent.Tooltip" />
    [Parameter]
    public string? Tooltip { get; set; }

    /// <summary>
    /// Gets or sets the culture of the component.
    /// By default <see cref="CultureInfo.CurrentCulture"/> to display using the OS culture.
    /// </summary>
    [Parameter]
    public virtual CultureInfo Culture { get; set; } = CultureInfo.CurrentCulture;

    /// <summary>
    /// Gets or sets the minimum allowed value.
    /// </summary>
    [Parameter]
    public TValue? Min { get; set; }

    /// <summary>
    /// Gets or sets the maximum allowed value.
    /// </summary>
    [Parameter]
    public TValue? Max { get; set; }

    /// <summary>
    /// Gets or sets the step increment. For decimal values, use e.g. Step="0.01".
    /// </summary>
    [Parameter]
    public TValue? Step { get; set; }

    /// <summary>
    /// Gets a value indicating whether the type is a floating-point type (e.g. float, double, decimal)
    /// </summary>
    public bool IsDecimal => default(TValue) is float or double or decimal or Half;

    /// <summary />
    protected override async Task OnInitializedAsync()
    {
        await base.RenderTooltipAsync(Tooltip);
    }

    /// <inheritdoc cref="ComponentBase.OnAfterRenderAsync(bool)" />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Utilities.Attributes.observeAttributeChange", Element, "value");

            // Initialize the 'immediate' custom event for the immediate mode
            await InitializeImmediateAsync();

            // Apply the number mask to the input element
            await ApplyNumberMaskAsync();
        }
    }

    /// <summary>
    /// Overrides the default parameter setting behavior to detect changes to the Culture parameter and re-apply the number mask when it changes,
    /// ensuring that the formatting updates accordingly.
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public override Task SetParametersAsync(ParameterView parameters)
    {
        // If Culture parameter changes, re-apply the number mask to update the formatting
        if (parameters.TryGetValue<CultureInfo?>(nameof(Culture), out var newCulture) && newCulture != null && !Equals(newCulture, Culture))
        {
            Culture = newCulture;
            _ = ApplyNumberMaskAsync();
        }

        // If Min/Max parameters change, re-apply the number mask to update the bounds
        if ((parameters.TryGetValue<TValue?>(nameof(Min), out var newMin) && newMin != Min) ||
            (parameters.TryGetValue<TValue?>(nameof(Max), out var newMax) && newMax != Max))
        {
            _ = ApplyNumberMaskAsync();
        }

        return base.SetParametersAsync(parameters);
    }

    /// <summary>
    /// Parses a string to create the value.
    /// HTML <c>&lt;input type="number"&gt;</c> always returns values with <c>.</c> as decimal separator
    /// (per the HTML spec), regardless of the user's locale. We must parse with
    /// <see cref="CultureInfo.InvariantCulture"/> to avoid misinterpretation
    /// (e.g. in fr-FR, <c>.</c> is the thousands separator, so <c>"2.5"</c> would be parsed as <c>25</c>).
    /// </summary>
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        if (TValue.TryParse(value, Culture, out var parsedValue))
        {
            // Clamp the parsed value to Min/Max bounds.
            if (Min.HasValue && parsedValue < Min.Value)
            {
                parsedValue = Min.Value;
            }

            if (Max.HasValue && parsedValue > Max.Value)
            {
                parsedValue = Max.Value;
            }

            result = parsedValue;
            validationErrorMessage = null;
            return true;
        }

        result = default;
        validationErrorMessage = "ERROR"; // TODO: string.Format(Culture, Localizer[Localization.LanguageResource.NumberInput_InvalidValue], DisplayName ?? FieldIdentifier.FieldName);
        return false;
    }

    /// <summary>
    /// Formats the value as a string using the configured <see cref="Culture"/>.
    /// </summary>
    protected override string? FormatValueAsString(TValue value)
    {
        return string.Format(Culture, IsDecimal ? "{0:N}" : "{0:N0}", value);
    }

    /// <summary>
    /// Handler for the OnFocus event.
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    protected virtual Task FocusOutHandlerAsync(FocusEventArgs e)
    {
        FocusLost = true;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Increments the current value by the defined <see cref="Step"/>.
    /// If the new value exceeds <see cref="Max"/>, it will be set to <see cref="Max"/>.
    /// </summary>
    private void IncrementValue()
    {
        if (Disabled == true || ReadOnly || Step == null || EqualityComparer<TValue>.Default.Equals(Step.Value, default))
        {
            return;
        }

        var newValue = CurrentValue + Step.Value;

        if (Max.HasValue && newValue > Max.Value)
        {
            newValue = Max.Value;
        }

        CurrentValue = newValue;
    }

    /// <summary>
    /// Decrements the current value by the defined <see cref="Step"/>.
    /// If the new value is less than <see cref="Min"/>, it will be set to <see cref="Min"/>.
    /// </summary>
    private void DecrementValue()
    {
        if (Disabled == true || ReadOnly || Step == null || EqualityComparer<TValue>.Default.Equals(Step.Value, default))
        {
            return;
        }

        var newValue = CurrentValue - Step.Value;

        if (Min.HasValue && newValue < Min.Value)
        {
            newValue = Min.Value;
        }

        CurrentValue = newValue;
    }

    /// <summary>
    /// Applies the number mask to the input element using JavaScript interop.
    /// </summary>
    /// <returns></returns>
    private async Task ApplyNumberMaskAsync()
    {
        // Set the mask pattern
        await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.TextMasked.applyNumberMask",
            Id,
            IsDecimal ? Culture.NumberFormat.NumberDecimalDigits : 0,       // Scale
            Culture.NumberFormat.NumberDecimalSeparator,                    // Radix char
            new[] { Culture.NumberFormat.NumberDecimalSeparator, CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator },  // Map to radix
            Min,                                                            // Min
            Max,                                                            // Max
            Culture.NumberFormat.NumberGroupSeparator);                     // Thousands separator
    }
}
