using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentSlider<TValue> : FluentInputBase<TValue>
#if NET7_0_OR_GREATER
    where TValue : System.Numerics.INumber<TValue>
#else
    where TValue : IComparable, IComparable<TValue>, IConvertible, IEquatable<TValue>, IFormattable
#endif
{
    /// <summary>
    /// Gets or sets the slider's minimal value
    /// </summary>
    [Parameter, EditorRequired]
    public TValue? Min { get; set; }

    /// <summary>
    /// Gets or sets the slider's maximum value
    /// </summary>
    [Parameter, EditorRequired]
    public TValue? Max { get; set; }

    /// <summary>
    /// Gets or sets the slider's step value
    /// </summary>
    [Parameter, EditorRequired]
    public TValue? Step { get; set; }

    /// <summary>
    /// Gets or sets the orentation of the slider. See <see cref="FluentUI.Orientation"/>
    /// </summary>
    [Parameter]
    public Orientation? Orientation { get; set; }

    /// <summary>
    /// The selection mode.
    /// </summary>
    [Parameter]
    public SliderMode? Mode { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }


    protected override void OnParametersSet()
    {
        ArgumentNullException.ThrowIfNull(Min, nameof(Min));
        ArgumentNullException.ThrowIfNull(Max, nameof(Max));
        ArgumentNullException.ThrowIfNull(Step, nameof(Step));
    }

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        if (BindConverter.TryConvertTo<TValue>(value, CultureInfo.InvariantCulture, out result))
        {
            validationErrorMessage = null;
            return true;
        }
        else
        {
            validationErrorMessage = string.Format(CultureInfo.InvariantCulture, "The {0} field must be a number.", DisplayName ?? FieldIdentifier.FieldName);
            return false;
        }
    }

    /// <summary>
    /// Formats the value as a string. Derived classes can override this to determine the formatting used for <c>CurrentValueAsString</c>.
    /// </summary>
    /// <param name = "value">The value to format.</param>
    /// <returns>A string representation of the value.</returns>
    protected override string? FormatValueAsString(TValue? value)
    {
        return InputHelpers<TValue>.FormatValueAsString(value);
    }

    private static readonly string _stepAttributeValue = GetStepAttributeValue();

    private static string GetStepAttributeValue()
    {
        // Unwrap Nullable<T>, because InputBase already deals with the Nullable aspect
        // of it for us. We will only get asked to parse the T for nonempty inputs.
        var targetType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
        if (targetType == typeof(int) ||
            targetType == typeof(long) ||
            targetType == typeof(short) ||
            targetType == typeof(float) ||
            targetType == typeof(double) ||
            targetType == typeof(decimal))
        {
            return "1";
        }
        else
        {
            throw new InvalidOperationException($"The type '{targetType}' is not a supported numeric type.");
        }
    }
}