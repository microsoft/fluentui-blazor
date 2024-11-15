// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A text input component that allows users to enter and edit a single line of text.
/// </summary>
public partial class FluentTextInput : FluentInputImmediateBase<string?>, IFluentComponentElementBase
{
    private const string JAVASCRIPT_FILE = FluentJSModule.JAVASCRIPT_ROOT + "TextInput/FluentTextInput.razor.js";

    /// <inheritdoc />
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

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // Import the JavaScript module
            var jsModule = await JSModule.ImportJavaScriptModuleAsync(JAVASCRIPT_FILE);

            // Call a function from the JavaScript module
            // Wait for this PR to delete the code: https://github.com/microsoft/fluentui/pull/33144
            await jsModule.InvokeVoidAsync("Microsoft.FluentUI.Blazor.TextInput.ObserveAttributeChanges", Element);
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
}
