using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentRadioGroup : FluentInputBase<string?>
{
    [Parameter]
    public string? Name { get; set; }

    [Parameter]
    public bool? Required { get; set; }

    [Parameter]
    public Orientation? Orientation { get; set; }

    protected override bool TryParseValueFromString(string? value, out string? result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        result = value;
        validationErrorMessage = null;
        return true;
    }
}