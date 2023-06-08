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
    public virtual TextFieldAppearance Appearance { get; set; } = TextFieldAppearance.Outline;

    /// <summary />
    protected override bool TryParseValueFromString(string? value, out DateTime? result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        if (value != null && DateTime.TryParse(value, out var valueConverted))
        {
            result = valueConverted;
        }
        else
        {
            result = null;
        }

        validationErrorMessage = null;
        return true;
    }
}
