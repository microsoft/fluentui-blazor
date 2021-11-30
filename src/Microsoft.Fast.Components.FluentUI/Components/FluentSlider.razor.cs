using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentSlider
{
    [Parameter]
    public Orientation? Orientation { get; set; }

    [Parameter]
    public int? Min { get; set; }

    [Parameter]
    public int? Max { get; set; }

    [Parameter]
    public int? Step { get; set; }

    [Parameter]
    public bool? Disabled { get; set; }

    [Parameter]
    public bool? Readonly { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

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