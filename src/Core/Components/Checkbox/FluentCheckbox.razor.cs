// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The FluentCheckbox component is used to render a checkbox input.
/// </summary>
public partial class FluentCheckbox : FluentInputBase<bool?>, IFluentComponentElementBase
{
    /// <inheritdoc />
    [Parameter]
    public ElementReference Element { get; set; }

    /// <summary>
    /// Gets or sets the checked state of the checkbox
    /// </summary>
    [Parameter]
    public bool? Checked { get; set; }

    /// <summary>
    /// Gets or sets the indeterminate state of the checkbox
    /// </summary>
    [Parameter]
    public bool? Indeterminate { get; set; }

    /// <summary>
    /// Gets or sets the shape of the checkbox
    /// </summary>
    [Parameter]
    public CheckboxShape Shape { get; set; } = CheckboxShape.Square;

    /// <summary>
    /// Gets or sets the short hint displayed in the input before the user enters a value.
    /// </summary>
    [Parameter]
    public string? Placeholder { get; set; }

    /// <summary>
    /// Gets or sets the text to required label when the checkbox should be checked.
    /// </summary>
    [Parameter]
    public string? RequiredLabel { get; set; }

    /// <summary>
    /// Gets or sets the content to required label Gets or sets the text to required label when the checkbox should be checked..
    /// This is usually displayed just above the checkbox
    /// </summary>
    [Parameter]
    public virtual RenderFragment? RequiredLabelTemplate { get; set; }

    /// <summary>
    /// Gets or sets the content to prefix the input component.
    /// </summary>
    [Parameter]
    public virtual RenderFragment? StartTemplate { get; set; }

    /// <summary>
    /// Gets or sets the content to suffix the input component.
    /// </summary>
    [Parameter]
    public virtual RenderFragment? EndTemplate { get; set; }

    /// <summary>
    /// Gets or sets the size of the checkbox. See <see cref="Components.CheckboxSize"/>
    /// </summary>
    [Parameter]
    public CheckboxSize? Size { get; set; } = CheckboxSize.Medium;

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        Id ??= $"checkbox-{Identifier.NewId()}";
    }

    /// <summary>
    /// Parses a string to create the <see cref="Microsoft.AspNetCore.Components.Forms.InputBase{TValue}.Value"/>.
    /// </summary>
    /// <param name="value">The string value to be parsed.</param>
    /// <param name="result">The result to inject into the Value.</param>
    /// <param name="validationErrorMessage">If the value could not be parsed, provides a validation error message.</param>
    /// <returns>True if the value could be parsed; otherwise false.</returns>
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out bool? result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        if (bool.TryParse(value, out var parsedValue))
        {
            result = parsedValue;
            validationErrorMessage = null;
            return true;
        }

        result = null;
        validationErrorMessage = "The provided value is not a valid boolean.";
        return false;
    }
}
