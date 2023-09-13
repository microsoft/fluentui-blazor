using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentTimePicker : FluentInputBase<DateTime?>
{
    /// <summary />
    protected override string? StyleValue => new StyleBuilder()
        .AddStyle(Style)
        .Build();

    /// <summary>
    /// Gets or sets the design of this input.
    /// </summary>
    [Parameter]
    public virtual FluentInputAppearance Appearance { get; set; } = FluentInputAppearance.Outline;

    /// <summary />
    protected override bool TryParseValueFromString(string? value, out DateTime? result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        DateTime currentValue = Value ?? DateTime.MinValue;

        if (value != null && DateTime.TryParse(value, out var valueConverted))
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
