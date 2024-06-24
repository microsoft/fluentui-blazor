using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentTimePicker : FluentInputBase<DateTime?>
{
    /// <summary />
    protected override string? StyleValue => new StyleBuilder(Style).Build();

    /// <summary>
    /// Gets or sets the design of this input.
    /// </summary>
    [Parameter]
    public virtual FluentInputAppearance Appearance { get; set; } = FluentInputAppearance.Outline;

    /// <summary />
    protected override bool TryParseValueFromString(string? value, out DateTime? result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        DateTime currentValue = Value ?? DateTime.MinValue;

        if (string.IsNullOrWhiteSpace(value))
        {
            result = null;
        }
        else if (value != null && DateTime.TryParse(value, out var valueConverted))
        {
            result = currentValue.Date + valueConverted.TimeOfDay;
        }
        else
        {
            result = Value?.Date;
        }

        validationErrorMessage = null;
        return true;
    }
}
