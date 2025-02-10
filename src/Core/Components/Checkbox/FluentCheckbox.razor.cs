// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The FluentCheckbox component is used to render a checkbox input
/// </summary>
public partial class FluentCheckbox : FluentInputBase<bool>, IFluentComponentElementBase
{
    /// <summary>
    /// 
    /// </summary>
    public FluentCheckbox()
    {
        LabelPosition = Components.LabelPosition.After;
    }

    /// <inheritdoc cref="IFluentComponentElementBase.Element" />
    [Parameter]
    public ElementReference Element { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user can display the indeterminate state by clicking the CheckBox.
    /// </summary>
    /// <remarks>If this is not the case, the checkbox can be started in the indeterminate state, but the user cannot activate it with the mouse.</remarks>
    /// <value>true</value>
    [Parameter]
    public bool ShowIndeterminate { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the CheckBox will allow three check states rather than two.
    /// </summary>
    [Parameter]
    public bool ThreeState { get; set; }

    /// <summary>
    /// Gets or sets a value indicating the order of the three states of the CheckBox.
    /// False(by default), the order is Unchecked -> Checked -> Intermediate.
    /// True: the order is Unchecked -> Intermediate -> Checked.
    /// </summary>
    [Parameter]
    public bool ThreeStateOrderUncheckToIntermediate { get; set; }

    /// <summary>
    /// Gets or sets the shape of the checkbox
    /// </summary>
    [Parameter]
    public CheckboxShape Shape { get; set; } = CheckboxShape.Square;

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
    /// Gets or sets the state of the CheckBox: true, false or null.
    /// Useful when the mode ThreeState is enable
    /// </summary>
    [Parameter]
    public bool? CheckState { get; set; }

    /// <summary>
    /// Action to be called when the CheckBox state changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool?> CheckStateChanged { get; set; }

    /// <summary>
    /// Gets or sets the size of the checkbox. See <see cref="CheckboxSize"/>
    /// </summary>
    [Parameter]
    public CheckboxSize Size { get; set; } = CheckboxSize.Medium;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (HasLabel())
        {
            // When the id is not provided, generate a unique id. This allow to use the label for.
            Id ??= $"{Identifier.NewId()}";
        }

        if (ThreeState && CheckState.HasValue)
        {
            await SetValueChangedAsync(CheckState.Value);
        }
    }

    /// <summary>
    /// Parses a string to create the <see cref="Microsoft.AspNetCore.Components.Forms.InputBase{TValue}.Value"/>.
    /// </summary>
    /// <param name="value">The string value to be parsed.</param>
    /// <param name="result">The result to inject into the Value.</param>
    /// <param name="validationErrorMessage">If the value could not be parsed, provides a validation error message.</param>
    /// <returns>True if the value could be parsed; otherwise false.</returns>
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out bool result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        if (bool.TryParse(value, out var parsedValue))
        {
            result = parsedValue;
            validationErrorMessage = null;
            return true;
        }

        result = default;
        throw new ArgumentOutOfRangeException("The provided value is not a valid boolean.");
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Utilities.Attributes.observeAttributeChange", Element, "checked", "boolean");
            await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Utilities.Attributes.observeAttributeChange", Element, "indeterminate", "boolean", "", true);
        }
    }

    private bool _checked => CheckState ?? Value;

    private bool _indeterminate => ThreeState
        ? !CheckState.HasValue
        : !ShowIndeterminate && !CheckState.HasValue;

    private bool HasLabel() => !string.IsNullOrEmpty(Label) || LabelTemplate is not null;

    private async Task SetValueChangedAsync(bool newValue)
    {
        if (Value == newValue)
        {
            return;
        }

        Value = newValue;

        if (ValueChanged.HasDelegate)
        {
            await ValueChanged.InvokeAsync(newValue);
        }
    }

    private async Task SetCheckStateChangedAsync(bool? newValue)
    {
        if (CheckState == newValue)
        {
            return;
        }

        CheckState = newValue;

        await SetValueChangedAsync(newValue ?? false);

        if (CheckStateChanged.HasDelegate)
        {
            await CheckStateChanged.InvokeAsync(newValue);
        }
    }

    private async Task OnCheckChangedHandlerAsync(ChangeEventArgs e)
    {
        ArgumentNullException.ThrowIfNull(e);

        if (ThreeState)
        {
            if (_checked)
            {
                // Current Check
                if (ThreeStateOrderUncheckToIntermediate)
                {
                    await SetToUncheckedAsync();
                }
                else
                {
                    await SetToIndeterminateAsync();
                }
            }
            else if (_indeterminate)
            {
                // Current _indeterminate
                if (ThreeStateOrderUncheckToIntermediate)
                {
                    await SetToCheckedAsync();
                }
                else
                {
                    await SetToUncheckedAsync();
                }
            }
            else
            {
                // Current Uncheck 
                if (ThreeStateOrderUncheckToIntermediate && ShowIndeterminate)
                {
                    await SetToIndeterminateAsync();
                }
                else
                {
                    await SetToCheckedAsync();
                }
            }
        }
        else
        {
            await SetCheckStateChangedAsync(!_checked);
        }
    }

    private async Task SetToIndeterminateAsync()
    {
        await SetCheckStateChangedAsync(ShowIndeterminate ? null : false);
    }

    private async Task SetToCheckedAsync()
    {
        await SetCheckStateChangedAsync(true);
    }

    private async Task SetToUncheckedAsync()
    {
        await SetCheckStateChangedAsync(newValue: false);
    }
}
