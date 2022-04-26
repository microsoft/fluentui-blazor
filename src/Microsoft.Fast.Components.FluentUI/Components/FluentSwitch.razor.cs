using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentSwitch : FluentInputBase<bool>
{
    [Parameter]
    public bool? Disabled { get; set; }

    [Parameter]
    public bool? Checked { get; set; }

    [Parameter]
    public bool? Required { get; set; }

    protected override bool TryParseValueFromString(string? value, out bool result, [NotNullWhen(false)] out string? validationErrorMessage) => throw new NotSupportedException($"This component does not parse string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");
}