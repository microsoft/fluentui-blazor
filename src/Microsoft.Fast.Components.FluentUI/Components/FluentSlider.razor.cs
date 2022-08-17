using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentSlider : FluentInputBase<int>
{
    /// <summary>
    /// Gets or sets if the slider is readonly
    /// </summary>
    [Parameter]
    public bool? Readonly { get; set; }

    /// <summary>
    /// Gets or sets the slider's minimal value
    /// </summary>
    [Parameter]
    public int? Min { get; set; }

    /// <summary>
    /// Gets or sets the slider's maximum value
    /// </summary>
    [Parameter]
    public int? Max { get; set; }

    /// <summary>
    /// Gets or sets the slider's step value
    /// </summary>
    [Parameter]
    public int? Step { get; set; }

    /// <summary>
    /// Gets or sets the orentation of the slider. See <see cref="FluentUI.Orientation"/>
    /// </summary>
    [Parameter]
    public Orientation? Orientation { get; set; }

    /// <summary>
    /// Gets or sets if the slider is disabled
    /// </summary>
    [Parameter]
    public bool? Disabled { get; set; }

    protected override bool TryParseValueFromString(string? value, out int result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        if (BindConverter.TryConvertTo<int>(value, CultureInfo.InvariantCulture, out result))
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

    protected override string? FormatValueAsString(int value)
    {
        return BindConverter.FormatValue(value, CultureInfo.InvariantCulture);
    }
}