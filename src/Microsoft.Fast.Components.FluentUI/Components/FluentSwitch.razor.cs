using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentSwitch : FluentInputBase<bool>
{
    /// <summary>
    /// Gets or sets if the switch is disabled
    /// </summary>
    [Parameter]
    public bool? Disabled { get; set; }

    /// <summary>
    /// Gets or sets if the switch is checked
    /// </summary>
    [Parameter]
    public bool? Checked { get; set; }

    /// <summary>
    /// Gets or sets if the switch is required
    /// </summary>
    [Parameter]
    public bool? Required { get; set; }

    protected override bool TryParseValueFromString(string? value, out bool result, [NotNullWhen(false)] out string? validationErrorMessage) => throw new NotSupportedException($"This component does not parse string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");
}