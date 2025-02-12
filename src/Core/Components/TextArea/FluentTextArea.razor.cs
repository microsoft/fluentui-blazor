// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A textarea component that allows users to enter and edit a single line of text.
/// </summary>
public partial class FluentTextArea : FluentInputImmediateBase<string?>, IFluentComponentElementBase
{

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentTextInput"/> class.
    /// </summary>
    public FluentTextArea()
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
    public TextAreaAppearance? Appearance { get; set; }

    /// <summary>
    /// Gets or sets the short hint displayed in the textarea before the user enters a value.
    /// </summary>
    [Parameter]
    public string? Placeholder { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of characters allowed in the textarea
    /// </summary>
    [Parameter]
    public int? MaxLength { get; set; }

    /// <summary>
    /// Gets or sets the minimum number of characters allowed in the textarea
    /// </summary>
    [Parameter]
    public int? MinLength { get; set; }

    /// <summary>
    /// Specifies whether a form or an textarea field should have autocomplete "on" or "off" or another value.
    /// An Id value must be set to use this property.
    /// </summary>
    [Parameter]
    public string? AutoComplete { get; set; }

    /// <summary>
    /// Whether the element’s height should be automatically changed based on the content.
    /// </summary>
    [Parameter]
    public bool? AutoResize { get; set; }    

    /// <summary>
    /// Gets or sets the size of the textarea. See <see cref="Components.TextAreaSize"/>
    /// </summary>
    [Parameter]
    public TextAreaSize? Size { get; set; }

    /// <summary>
    /// Gets or sets the how resize the element. See <see cref="Components.TextAreaResize"/>
    /// </summary>
    [Parameter]
    public TextAreaResize? Resize { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether spellcheck should be used.
    /// </summary>
    [Parameter]
    public bool? Spellcheck { get; set; }

    /// <inheritdoc cref="ComponentBase.OnAfterRenderAsync(bool)" />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Utilities.Attributes.observeAttributeChange", Element, "value");
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

    private string? displayShadow => Appearance == TextAreaAppearance.FilledDarkerShadow || Appearance == TextAreaAppearance.FilledLighterShadow ? "true" : null;
    private string? speelCheckValue => Spellcheck.HasValue ? Spellcheck.Value ? "true" : "false" : null;
}
