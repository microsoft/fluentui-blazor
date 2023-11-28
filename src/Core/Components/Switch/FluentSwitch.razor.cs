using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentSwitch : FluentInputBase<bool>
{
    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the checked message
    /// </summary>
    [Parameter]
    public string? CheckedMessage { get; set; }

    /// <summary>
    /// Gets or sets the unchecked message
    /// </summary>
    [Parameter]
    public string? UncheckedMessage { get; set; }

    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(CheckboxChangeEventArgs))]

    public FluentSwitch()
    {

    }

    protected override string? ClassValue
    {
        get
        {
            return new CssBuilder(base.ClassValue)
                .AddClass("checked", Value)
                .Build();
        }
    }

    protected override bool TryParseValueFromString(string? value, out bool result, [NotNullWhen(false)] out string? validationErrorMessage) => throw new NotSupportedException($"This component does not parse string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");
}