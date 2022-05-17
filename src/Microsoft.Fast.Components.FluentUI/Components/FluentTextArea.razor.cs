using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentTextArea : FluentInputBase<string?>
{
    /// <summary>
    /// Gets or sets if the text area is disabled
    /// </summary>
    [Parameter]
    public bool? Disabled { get; set; }

    /// <summary>
    /// Gets or sets if the text area is readonly
    /// </summary>
    [Parameter]
    public bool? Readonly { get; set; }

    /// <summary>
    /// Gets or sets if the text area is required
    /// </summary>
    [Parameter]
    public bool? Required { get; set; }

    /// <summary>
    /// Gets or sets if the text area is auto focussed
    /// </summary>
    [Parameter]
    public bool? Autofocus { get; set; }

    /// <summary>
    /// Gets or sets if the text area is resizeable. See <see cref="FluentUI.Resize"/>
    /// </summary>
    [Parameter]
    public Resize? Resize { get; set; }


    /// <summary>
    /// Gets or sets the visual appearance. See <see cref="FluentUI.Appearance"/>
    /// </summary>
    [Parameter]
    public Appearance? Appearance { get; set; }

    /// <summary>
    /// Gets or sets the placholder text
    /// </summary>
    [Parameter]
    public string? Placeholder { get; set; }

    protected override bool TryParseValueFromString(string? value, out string? result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        result = value;
        validationErrorMessage = null;
        return true;
    }
}