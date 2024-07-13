using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentNumberField<TValue> : FluentInputBase<TValue>
{
    /// <summary>
    /// When true, spin buttons will not be rendered.
    /// </summary>
    [Parameter]
    public bool HideStep { get; set; }

    /// <summary>
    /// Allows associating a <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/datalist">datalist</see> to the element by <see href="https://developer.mozilla.org/en-US/docs/Web/API/Element/id">id</see>.
    /// </summary>
    [Parameter]
    public string? DataList { get; set; }

    /// <summary>
    /// Gets or sets the maximum length.
    /// </summary>
    [Parameter]
    public int MaxLength { get; set; } = 14;

    /// <summary>
    /// Gets or sets the minimum length.
    /// </summary>
    [Parameter]
    public int MinLength { get; set; } = 1;

    /// <summary>
    /// Gets or sets the size.
    /// </summary>
    [Parameter]
    public int Size { get; set; } = 20;

    /// <summary>
    /// Gets or sets the amount to increase/decrease the number with. Only use whole number when TValue is int or long. 
    /// </summary>
    [Parameter]
    public string Step { get; set; } = _stepAttributeValue;

    /// <summary>
    /// Gets or sets the maximum value.
    /// </summary>
    [Parameter]
    public string? Max { get; set; }

    /// <summary>
    /// Gets or sets the minimum value.
    /// </summary>
    [Parameter]
    public string? Min { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="AspNetCore.Components.Appearance" />.
    /// </summary>
    [Parameter]
    public FluentInputAppearance Appearance { get; set; } = FluentInputAppearance.Outline;

    /// <summary>
    /// Gets or sets the error message to show when the field can not be parsed.
    /// </summary>
    [Parameter]
    public string ParsingErrorMessage { get; set; } = "The {0} field must be a number.";

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private static readonly string _stepAttributeValue = GetStepAttributeValue();

    private static string GetStepAttributeValue()
    {
        // Unwrap Nullable<T>, because InputBase already deals with the Nullable aspect
        // of it for us. We will only get asked to parse the T for nonempty inputs.
        var targetType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
        if (targetType == typeof(sbyte) ||
            targetType == typeof(int) ||
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

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        if (BindConverter.TryConvertTo(value, CultureInfo.InvariantCulture, out result))
        {
            validationErrorMessage = null;
            return true;
        }
        else
        {
            validationErrorMessage = string.Format(CultureInfo.InvariantCulture, ParsingErrorMessage, FieldBound ? FieldIdentifier.FieldName : UnknownBoundField);
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
        // Avoiding a cast to IFormattable to avoid boxing.
        return value switch
        {
            null => null,
            sbyte @sbyte => BindConverter.FormatValue(Convert.ToInt16(@sbyte), CultureInfo.InvariantCulture),
            int @int => BindConverter.FormatValue(@int, CultureInfo.InvariantCulture),
            long @long => BindConverter.FormatValue(@long, CultureInfo.InvariantCulture),
            short @short => BindConverter.FormatValue(@short, CultureInfo.InvariantCulture),
            float @float => BindConverter.FormatValue(@float, CultureInfo.InvariantCulture),
            double @double => BindConverter.FormatValue(@double, CultureInfo.InvariantCulture),
            decimal @decimal => BindConverter.FormatValue(@decimal, CultureInfo.InvariantCulture),
            _ => throw new InvalidOperationException($"Unsupported type {value.GetType()}"),
        };
    }

    protected override void OnParametersSet()
    {
        InputHelpers<TValue>.ValidateInputParameters(Max, Min);
        base.OnParametersSet();
    }
}
