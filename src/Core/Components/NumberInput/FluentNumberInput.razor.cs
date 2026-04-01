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
/// A numeric input component that allows users to enter and edit decimal or integer numbers.
/// Wraps <c>&lt;fluent-text-input type="number"&gt;</c> and sets step/min/max on the inner input.
/// </summary>
/// <typeparam name="TValue">A numeric struct type such as int, double, float, decimal, etc.</typeparam>
public partial class FluentNumberInput<TValue> : FluentInputImmediateBase<TValue>, IFluentComponentElementBase, ITooltipComponent
    where TValue : struct, IComparable<TValue>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FluentNumberInput{TValue}"/> class.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when TValue is not a supported numeric type.</exception>
    public FluentNumberInput(LibraryConfiguration configuration) : base(configuration)
    {
        if (typeof(TValue) != typeof(byte) &&
            typeof(TValue) != typeof(sbyte) &&
            typeof(TValue) != typeof(short) &&
            typeof(TValue) != typeof(ushort) &&
            typeof(TValue) != typeof(int) &&
            typeof(TValue) != typeof(uint) &&
            typeof(TValue) != typeof(long) &&
            typeof(TValue) != typeof(ulong) &&
            typeof(TValue) != typeof(float) &&
            typeof(TValue) != typeof(double) &&
            typeof(TValue) != typeof(decimal))
        {
            throw new InvalidOperationException("FluentNumberInput only supports numeric types.");
        }

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
    /// Gets the CSS style to apply to the internal web-component.
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
    /// Gets or sets the width of the input field.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the size of the input. See <see cref="TextInputSize"/>
    /// </summary>
    [Parameter]
    public TextInputSize? Size { get; set; }

    /// <inheritdoc cref="ITooltipComponent.Tooltip" />
    [Parameter]
    public string? Tooltip { get; set; }

    /// <summary />
    protected override async Task OnInitializedAsync()
    {
        await base.RenderTooltipAsync(Tooltip);
    }

    private string? _lastStep;
    private string? _lastMin;
    private string? _lastMax;

    /// <inheritdoc cref="ComponentBase.OnAfterRenderAsync(bool)" />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Utilities.Attributes.observeAttributeChange", Element, "value");

            // Initialize the 'immediate' custom event for the immediate mode
            await InitializeImmediateAsync();
        }

        // Set step/min/max on the inner <input> inside the shadow DOM.
        // Reapply whenever the values change (not only on first render).
        var currentStep = FormatNullableValue(Step);
        var currentMin = FormatNullableValue(Min);
        var currentMax = FormatNullableValue(Max);

        if (firstRender ||
            !string.Equals(currentStep, _lastStep, StringComparison.Ordinal) ||
            !string.Equals(currentMin, _lastMin, StringComparison.Ordinal) ||
            !string.Equals(currentMax, _lastMax, StringComparison.Ordinal))
        {
            _lastStep = currentStep;
            _lastMin = currentMin;
            _lastMax = currentMax;

            await JSRuntime.InvokeVoidAsync(
                "Microsoft.FluentUI.Blazor.Components.NumberInput.setNumberAttributes",
                Id,
                currentStep,
                currentMin,
                currentMax);
        }
    }

    /// <summary>
    /// Formats the value as a string using InvariantCulture.
    /// </summary>
    protected override string? FormatValueAsString(TValue value)
    {
        return Convert.ToString(value, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Parses a string to create the value.
    /// HTML <c>&lt;input type="number"&gt;</c> always returns values with <c>.</c> as decimal separator
    /// (per the HTML spec), regardless of the user's locale. We must parse with
    /// <see cref="CultureInfo.InvariantCulture"/> to avoid misinterpretation
    /// (e.g. in fr-FR, <c>.</c> is the thousands separator, so <c>"2.5"</c> would be parsed as <c>25</c>).
    /// </summary>
    [SuppressMessage("Trimming", "IL2091:Target generic argument does not satisfy 'DynamicallyAccessedMembersAttribute' in target method or type. The generic parameter of the source method or type does not have matching annotations.",
                 Justification = "In the context, the 'TValue' will not be trimmed.")]
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        // Normalize comma to dot to handle edge cases where a browser
        // might send the locale-formatted decimal separator.
        var normalized = value?.Replace(',', '.');

        if (BindConverter.TryConvertTo<TValue>(normalized, CultureInfo.InvariantCulture, out var parsedValue))
        {
            result = parsedValue;
            validationErrorMessage = null;
            return true;
        }

        result = default;
        validationErrorMessage = $"The '{DisplayName ?? "Unknown Bound Field"}' field is not valid.";
        return false;
    }

    /// <summary>
    /// Handler for the OnFocusOut event.
    /// </summary>
    protected virtual Task FocusOutHandlerAsync(FocusEventArgs e)
    {
        FocusLost = true;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Formats a nullable TValue to a string, or returns null.
    /// </summary>
    private static string? FormatNullableValue(TValue? value)
    {
        return value is null ? null : Convert.ToString(value.Value, CultureInfo.InvariantCulture);
    }
}
