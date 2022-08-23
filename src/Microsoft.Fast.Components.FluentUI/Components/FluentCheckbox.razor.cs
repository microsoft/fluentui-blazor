using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Fast.Components.FluentUI;
public partial class FluentCheckbox : FluentInputBase<bool>
{


    protected override bool TryParseValueFromString(string? value, out bool result, [NotNullWhen(false)] out string? validationErrorMessage) => throw new NotSupportedException($"This component does not parse string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");
}