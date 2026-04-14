// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A numeric input component that allows users to enter and edit numeric values.
/// </summary>
public partial class FluentNumber<TValue> : FluentInputImmediateBase<TValue>, IFluentComponentElementBase, ITooltipComponent
{
    private readonly Type UnderlyingType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
    private readonly TValue ZeroValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentNumber{TValue}"/> class.
    /// </summary>
    [SuppressMessage("Design", "MA0051:Method is too long", Justification = "Optimizing for performance and readability.")]
    public FluentNumber(LibraryConfiguration configuration) : base(configuration)
    {
        // sbyte
        if (typeof(TValue) == typeof(sbyte) || typeof(TValue) == typeof(sbyte?))
        {
            ZeroValue = (TValue)(object)(sbyte)0;
            Min = (TValue)(object)sbyte.MinValue;
            Max = (TValue)(object)sbyte.MaxValue;
            Step = (TValue)(object)(sbyte)1;
        }
        // byte
        else if (typeof(TValue) == typeof(byte) || typeof(TValue) == typeof(byte?))
        {
            ZeroValue = (TValue)(object)(byte)0;
            Min = (TValue)(object)byte.MinValue;
            Max = (TValue)(object)byte.MaxValue;
            Step = (TValue)(object)(byte)1;
        }
        // short
        else if (typeof(TValue) == typeof(short) || typeof(TValue) == typeof(short?))
        {
            ZeroValue = (TValue)(object)(short)0;
            Min = (TValue)(object)short.MinValue;
            Max = (TValue)(object)short.MaxValue;
            Step = (TValue)(object)(short)1;
        }
        // ushort
        else if (typeof(TValue) == typeof(ushort) || typeof(TValue) == typeof(ushort?))
        {
            ZeroValue = (TValue)(object)(ushort)0;
            Min = (TValue)(object)ushort.MinValue;
            Max = (TValue)(object)ushort.MaxValue;
            Step = (TValue)(object)(ushort)1;
        }
        // int
        else if (typeof(TValue) == typeof(int) || typeof(TValue) == typeof(int?))
        {
            ZeroValue = (TValue)(object)0;
            Min = (TValue)(object)int.MinValue;
            Max = (TValue)(object)int.MaxValue;
            Step = (TValue)(object)1;
        }
        // uint
        else if (typeof(TValue) == typeof(uint) || typeof(TValue) == typeof(uint?))
        {
            ZeroValue = (TValue)(object)0u;
            Min = (TValue)(object)uint.MinValue;
            Max = (TValue)(object)uint.MaxValue;
            Step = (TValue)(object)1u;
        }
        // long
        else if (typeof(TValue) == typeof(long) || typeof(TValue) == typeof(long?))
        {
            ZeroValue = (TValue)(object)0L;
            Min = (TValue)(object)long.MinValue;
            Max = (TValue)(object)long.MaxValue;
            Step = (TValue)(object)1L;
        }
        // ulong
        else if (typeof(TValue) == typeof(ulong) || typeof(TValue) == typeof(ulong?))
        {
            ZeroValue = (TValue)(object)0UL;
            Min = (TValue)(object)ulong.MinValue;
            Max = (TValue)(object)ulong.MaxValue;
            Step = (TValue)(object)1UL;
        }
        // float
        else if (typeof(TValue) == typeof(float) || typeof(TValue) == typeof(float?))
        {
            ZeroValue = (TValue)(object)0.0f;
            Min = (TValue)(object)float.MinValue;
            Max = (TValue)(object)float.MaxValue;
            Step = (TValue)(object)1.0f;
        }
        // double
        else if (typeof(TValue) == typeof(double) || typeof(TValue) == typeof(double?))
        {
            ZeroValue = (TValue)(object)0.0;
            Min = (TValue)(object)double.MinValue;
            Max = (TValue)(object)double.MaxValue;
            Step = (TValue)(object)1.0;
        }
        // decimal
        else if (typeof(TValue) == typeof(decimal) || typeof(TValue) == typeof(decimal?))
        {
            ZeroValue = (TValue)(object)0.0m;
            Min = (TValue)(object)decimal.MinValue;
            Max = (TValue)(object)decimal.MaxValue;
            Step = (TValue)(object)1M;
        }
        else
        {
            throw new InvalidOperationException($"Unsupported type {typeof(TValue)}. Supported types are sbyte, byte, short, ushort, int, uint, long, ulong, float, double, and decimal (including nullable versions).");
        }

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
    /// By default <see cref="FluentNumberCultureInfo"/> to display using a consistent culture independent of the OS culture
    /// (2 decimal digits, CurrentCulture.NumberFormat.NumberDecimalSeparator as decimal separator, and no group separator).
    /// </summary>
    [Parameter]
    public virtual CultureInfo Culture { get; set; } = new FluentNumberCultureInfo();

    /// <summary>
    /// Gets or sets the minimum allowed value.
    /// </summary>
    [Parameter]
    public TValue Min { get; set; }

    /// <summary>
    /// Gets or sets the maximum allowed value.
    /// </summary>
    [Parameter]
    public TValue Max { get; set; }

    /// <summary>
    /// Gets or sets the step increment. For decimal values, use e.g. Step="0.01".
    /// Defaults to 1 for all types. If set to null or 0, the step buttons will be disabled.
    /// </summary>
    [Parameter]
    public TValue Step { get; set; }

    /// <summary>
    /// Gets or sets the visibility behavior of the step buttons (up/down arrows).
    /// Defaults to <see cref="NumberStepVisibility.Auto"/>.
    /// </summary>
    [Parameter]
    public NumberStepVisibility StepButtons { get; set; } = NumberStepVisibility.Auto;

    /// <summary>
    /// Gets a value indicating whether the type is a floating-point type (e.g. float, double, decimal)
    /// </summary>
    public bool IsDecimal => UnderlyingType == typeof(float) || UnderlyingType == typeof(double) || UnderlyingType == typeof(decimal);

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
        if ((parameters.TryGetValue<TValue?>(nameof(Min), out var newMin) && IsNotEquals(newMin, Min)) ||
            (parameters.TryGetValue<TValue?>(nameof(Max), out var newMax) && IsNotEquals(newMax, Max)))
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
        // Nullable types should allow empty string as null value
        if (string.IsNullOrEmpty(value) && UnderlyingType != typeof(TValue))
        {
            result = default!;
            validationErrorMessage = null;
            return true;
        }

        if (TryParse(value, Culture, out var parsedValue))
        {
            // Clamp the parsed value to Min/Max bounds.
            if (IsLowerThan(parsedValue, Min))
            {
                parsedValue = Min;
            }

            if (IsGreaterThan(parsedValue, Max))
            {
                parsedValue = Max;
            }

            result = parsedValue;
            validationErrorMessage = null;
            return true;
        }

        result = ZeroValue;
        validationErrorMessage = null;
        return true;
    }

    /// <summary>
    /// Formats the value as a string using the configured <see cref="Culture"/>.
    /// </summary>
    protected override string? FormatValueAsString(TValue? value)
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
        if (Disabled == true || ReadOnly || Step == null || IsEquals(Step, ZeroValue))
        {
            return;
        }

        var newValue = RoundIfDecimal(Add(CurrentValue ?? ZeroValue, Step));

        if (IsGreaterThan(newValue, Max))
        {
            newValue = Max;
        }

        CurrentValue = newValue;
    }

    /// <summary>
    /// Decrements the current value by the defined <see cref="Step"/>.
    /// If the new value is less than <see cref="Min"/>, it will be set to <see cref="Min"/>.
    /// </summary>
    private void DecrementValue()
    {
        if (Disabled == true || ReadOnly || Step == null || IsEquals(Step, ZeroValue))
        {
            return;
        }

        var newValue = RoundIfDecimal(Subtract(CurrentValue ?? ZeroValue, Step));

        if (IsLowerThan(newValue, Min))
        {
            newValue = Min;
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
            Culture.NumberFormat.NumberGroupSeparator,                      // Thousands separator
            Step);                                                          // Step
    }

    /// <summary>
    /// Removes all characters that are not ASCII digits or the decimal separator.
    /// This ensures reliable parsing regardless of which Unicode character the browser uses for group separators.
    /// </summary>
    private string? KeepOnlyDigits(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        var decimalSep = Culture.NumberFormat.NumberDecimalSeparator;
        return new string([.. value.Where(c => char.IsAsciiDigit(c) || decimalSep.Contains(c, StringComparison.Ordinal))]);
    }

    /// <summary>
    /// Rounds the value to the number of decimal digits defined by the current <see cref="Culture"/>.
    /// This avoids floating-point precision errors (e.g. 0.3 - 0.1 = 0.19999999999999998).
    /// </summary>
    private TValue? RoundIfDecimal(TValue? value)
    {
        if (value is not null && IsDecimal)
        {
            var decimals = Culture.NumberFormat.NumberDecimalDigits;
            var rounded = Math.Round(Convert.ToDouble(value, CultureInfo.InvariantCulture), decimals, MidpointRounding.AwayFromZero);
            return (TValue)Convert.ChangeType(rounded, UnderlyingType, CultureInfo.InvariantCulture);
        }

        return value;
    }

    /// <summary>
    /// Tries to parse the input string into a value of type <typeparamref name="TValue"/> using the specified <see cref="Culture"/>.
    /// It first removes all non-digit characters except the decimal separator to ensure reliable parsing regardless of which Unicode character the browser uses for group separators.
    /// </summary>
    private bool TryParse(string? value, IFormatProvider formatProvider, out TValue result)
    {
        // Remove all non-digit characters except the decimal separator before parsing.
        // This ensures that we can reliably parse the number regardless of which Unicode character the browser uses for group separators
        // (e.g. non-breaking space, narrow no-break space, etc.).
        var extractedDigits = KeepOnlyDigits(value);

        if (double.TryParse(extractedDigits, formatProvider, out var parsedValue))
        {
            result = (TValue)Convert.ChangeType(parsedValue, UnderlyingType, CultureInfo.InvariantCulture);
            return true;
        }

        result = default!;
        return false;
    }

    /// <summary>
    /// Returns true if <paramref name="a"/> is equal to <paramref name="b"/>.
    /// </summary>
    private static bool IsEquals(TValue? a, TValue? b) => Comparer<TValue>.Default.Compare(a, b) == 0;

    /// <summary>
    /// Returns true if <paramref name="a"/> is not equal to <paramref name="b"/>.
    /// </summary>
    private static bool IsNotEquals(TValue? a, TValue? b) => !IsEquals(a, b);

    /// <summary>
    /// Returns true if <paramref name="a"/> is strictly less than <paramref name="b"/>.
    /// </summary>
    private static bool IsLowerThan(TValue? a, TValue? b) => Comparer<TValue>.Default.Compare(a, b) < 0;

    /// <summary>
    /// Returns true if <paramref name="a"/> is strictly greater than <paramref name="b"/>.
    /// </summary>
    private static bool IsGreaterThan(TValue? a, TValue? b) => Comparer<TValue>.Default.Compare(a, b) > 0;

    /// <summary>
    /// Adds two values of type <typeparamref name="TValue"/> by converting them to double for the addition and then back to <typeparamref name="TValue"/>.
    /// </summary>
    private TValue Add(TValue a, TValue b) => (TValue)Convert.ChangeType(Convert.ToDouble(a, CultureInfo.InvariantCulture) + Convert.ToDouble(b, CultureInfo.InvariantCulture), UnderlyingType, CultureInfo.InvariantCulture);

    /// <summary>
    /// Subtracts two values of type <typeparamref name="TValue"/> by converting them to double for the subtraction and then back to <typeparamref name="TValue"/>.
    /// </summary>
    private TValue Subtract(TValue a, TValue b) => (TValue)Convert.ChangeType(Convert.ToDouble(a, CultureInfo.InvariantCulture) - Convert.ToDouble(b, CultureInfo.InvariantCulture), UnderlyingType, CultureInfo.InvariantCulture);
}
