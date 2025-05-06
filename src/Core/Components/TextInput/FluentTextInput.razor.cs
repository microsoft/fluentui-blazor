// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A text input component that allows users to enter and edit a single line of text.
/// </summary>
public partial class FluentTextInput : FluentInputImmediateBase<string?>, IFluentComponentElementBase, ITooltipComponent, IFluentComponentChangeAfterKeyPress
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FluentTextInput"/> class.
    /// </summary>
    public FluentTextInput()
    {
        // Default conditions for the message
        MessageCondition = (field) =>
        {
            field.MessageIcon = FluentStatus.ErrorIcon;
            field.Message = Localizer[Localization.LanguageResource.TextInput_RequiredMessage];

            return FocusLost &&
                   (Required ?? false)
                   && !(Disabled ?? false)
                   && !ReadOnly
                   && string.IsNullOrEmpty(CurrentValueAsString);
        };

    }

    /// <inheritdoc cref="IFluentComponentElementBase.Element" />
    [Parameter]
    public ElementReference Element { get; set; }

    /// <summary>
    /// Gets or sets the visual appearance.
    /// </summary>
    [Parameter]
    public TextInputAppearance Appearance { get; set; } = TextInputAppearance.Outline;

    /// <summary>
    /// Gets or sets the short hint displayed in the input before the user enters a value.
    /// </summary>
    [Parameter]
    public string? Placeholder { get; set; }

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
    /// Gets or sets the id of a datalist element that provides a list of suggested values.
    /// <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/datalist">datalist</see>.
    /// </summary>
    [Parameter]
    public string? DataList { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of characters allowed in the input
    /// </summary>
    [Parameter]
    public int? MaxLength { get; set; }

    /// <summary>
    /// Gets or sets the minimum number of characters allowed in the input
    /// </summary>
    [Parameter]
    public int? MinLength { get; set; }

    /// <summary>
    /// Gets or sets a regular expression that the value must match to pass validation.
    /// </summary>
    [Parameter]
    public string? Pattern { get; set; }

    /// <summary>
    /// Specifies whether a form or an input field should have autocomplete "on" or "off" or another value.
    /// An Id value must be set to use this property.
    /// </summary>
    [Parameter]
    public string? AutoComplete { get; set; }

    /// <summary>
    /// Gets or sets the text filed type. See <see cref="Components.TextInputType"/>
    /// </summary>
    [Parameter]
    public TextInputType? TextFieldType { get; set; }

    /// <summary>
    /// Gets or sets the size of the input. See <see cref="Components.TextInputSize"/>
    /// </summary>
    [Parameter]
    public TextInputSize? Size { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether spellcheck should be used.
    /// </summary>
    [Parameter]
    public bool? Spellcheck { get; set; }           // TODO: To verify if this is supported by the component

    /// <summary>
    /// Gets or sets the type of data that can be entered by the user when editing the element or its content.
    /// This allows a browser to display an appropriate virtual keyboard. Not supported by Safari.
    /// </summary>
    [Parameter]
    public TextInputMode? InputMode { get; set; }   // TODO: To verify if this is supported by the component

    /// <inheritdoc cref="ITooltipComponent.Tooltip" />
    [Parameter]
    public string? Tooltip { get; set; }

    /// <inheritdoc cref="IFluentComponentChangeAfterKeyPress.ChangeAfterKeyPress" />
    [Parameter]
    public KeyPress[]? ChangeAfterKeyPress { get; set; }

    /// <inheritdoc cref="IFluentComponentChangeAfterKeyPress.OnChangeAfterKeyPress" />
    [Parameter]
    public EventCallback<FluentKeyPressEventArgs> OnChangeAfterKeyPress { get; set; }

    /// <inheritdoc cref="IFluentComponentChangeAfterKeyPress.ChangeAfterKeyPressHandlerAsync(string, KeyPress)" />
    [JSInvokable]
    public async Task ChangeAfterKeyPressHandlerAsync(string value, KeyPress key)
    {
        await ChangeHandlerAsync(new ChangeEventArgs() { Value = value });

        if (OnChangeAfterKeyPress.HasDelegate)
        {
            await OnChangeAfterKeyPress.InvokeAsync(new FluentKeyPressEventArgs() { KeyPress = key});
        }
    }

    /// <summary />
    protected override async Task OnInitializedAsync()
    {
        await base.RenderTooltipAsync(Tooltip);
    }

    /// <inheritdoc cref="ComponentBase.OnAfterRenderAsync(bool)" />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Utilities.Attributes.observeAttributeChange", Element, "value");

            // Initialize the change after key press event
            await IFluentComponentChangeAfterKeyPress.InitializeRuntimeAsync(this, JSRuntime, Element);
        }
    }

    /// <summary>
    /// Parses a string to create the <see cref="Microsoft.AspNetCore.Components.Forms.InputBase{TValue}.Value"/>.
    /// </summary>
    /// <param name="value">The string value to be parsed.</param>
    /// <param name="result">The result to inject into the Value.</param>
    /// <param name="validationErrorMessage">If the value could not be parsed, provides a validation error message.</param>
    /// <returns>True if the value could be parsed; otherwise false.</returns>
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out string? result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        result = value;
        validationErrorMessage = null;
        return true;
    }

    /// <summary>
    /// Handler for the OnFocus event.
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    protected virtual Task FocusOutHandlerAsync(FocusEventArgs e)
    {
        FocusLost = true;
        return Task.CompletedTask;
    }
}
