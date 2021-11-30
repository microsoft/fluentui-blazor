using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;

using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI
{
    public partial class FluentNumberField<TValue>
    {
        [Parameter]
        public bool? Disabled { get; set; }

        [Parameter]
        public bool? Readonly { get; set; }

        [Parameter]
        public bool? Required { get; set; }

        [Parameter]
        public bool? Autofocus { get; set; }

        [Parameter]
        public int Size { get; set; } = 20;
        [Parameter]
        public Appearance? Appearance { get; set; }

        [Parameter]
        public string? Placeholder { get; set; }

        [Parameter]
        public string ParsingErrorMessage { get; set; } = "The {0} field must be a number.";
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [Parameter]
        public string? Min { get; set; } = GetMinOrMaxValue("MinValue");
        [Parameter]
        public string? Max { get; set; } = GetMinOrMaxValue("MaxValue");
        [Parameter]
        public int MinLength { get; set; } = 1;
        [Parameter]
        public int MaxLength { get; set; } = 14;
        [Parameter]
        public int Step { get; set; } = _stepAttributeValue;
        private readonly static int _stepAttributeValue;
        static FluentNumberField()
        {
            // Unwrap Nullable<T>, because InputBase already deals with the Nullable aspect
            // of it for us. We will only get asked to parse the T for nonempty inputs.
            var targetType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
            if (targetType == typeof(int) || targetType == typeof(long) || targetType == typeof(short) || targetType == typeof(float) || targetType == typeof(double) || targetType == typeof(decimal))
            {
                _stepAttributeValue = 1;
            }
            else
            {
                throw new InvalidOperationException($"The type '{targetType}' is not a supported numeric type.");
            }
        }

        protected override bool TryParseValueFromString(string? value, out TValue? result, [NotNullWhen(false)] out string? validationErrorMessage)
        {
            if (BindConverter.TryConvertTo<TValue>(value, CultureInfo.InvariantCulture, out result))
            {
                validationErrorMessage = null;
                return true;
            }
            else
            {
                validationErrorMessage = string.Format(CultureInfo.InvariantCulture, ParsingErrorMessage, FieldIdentifier.FieldName);
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
            switch (value)
            {
                case null:
                    return null;
                case int @int:
                    return BindConverter.FormatValue(@int, CultureInfo.InvariantCulture);
                case long @long:
                    return BindConverter.FormatValue(@long, CultureInfo.InvariantCulture);
                case short @short:
                    return BindConverter.FormatValue(@short, CultureInfo.InvariantCulture);
                case float @float:
                    return BindConverter.FormatValue(@float, CultureInfo.InvariantCulture);
                case double @double:
                    return BindConverter.FormatValue(@double, CultureInfo.InvariantCulture);
                case decimal @decimal:
                    return BindConverter.FormatValue(@decimal, CultureInfo.InvariantCulture);
                default:
                    throw new InvalidOperationException($"Unsupported type {value.GetType()}");
            }
        }

        private static string? GetMinOrMaxValue(string name)
        {
            var targetType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
            FieldInfo? field = targetType.GetField(name, BindingFlags.Public | BindingFlags.Static);
            if (field == null)
            {
                throw new InvalidOperationException("Invalid type argument for FluentNumberField<TValue?>: " + typeof(TValue).Name);
            }

            var value = field.GetValue(null);
            if (value is not null)
            {
                if (targetType == typeof(int) || targetType == typeof(short))
                {
                    return value.ToString();
                }

                if (targetType == typeof(long))
                {
                    //Precision in underlying Fast script is limited to 12 digits
                    return name == "MinValue" ? "-999999999999" : "999999999999";
                }

                if (targetType == typeof(float))
                {
                    return ((float)value).ToString(CultureInfo.InvariantCulture);
                }

                if (targetType == typeof(double))
                {
                    return ((double)value).ToString(CultureInfo.InvariantCulture);
                }

                if (targetType == typeof(decimal))
                {
                    return ((decimal)value).ToString("G12", CultureInfo.InvariantCulture);
                }
            }

            // This should never occur
            return "0";
        }
    }
}