// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Base class for FluentUI Blazor components.
/// </summary>
public abstract class FluentComponentBase : ComponentBase
{
    private IJSObjectReference? _jsModule;

    /// <summary>
    /// Gets the root path for the JavaScript files.
    /// </summary>
    protected const string JAVASCRIPT_ROOT = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/";

    /// <summary>
    /// Gets or sets a reference to the JavaScript runtime.
    /// This property is injected by the Blazor framework.
    /// </summary>
    [Inject]
    protected virtual IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary>
    /// Gets the JavaScript module imported with the <see cref="ImportJavaScriptModuleAsync"/> method.
    /// You need to call this method (in the `OnAfterRenderAsync` method) before using the module.
    /// </summary>
    protected virtual IJSObjectReference JSModule => _jsModule ?? throw new InvalidOperationException("You must call `ImportJavaScriptModuleAsync` method before accessing the JSModule property.");

    /// <summary>
    /// Invoke the JavaScript runtime to import the JavaScript module.
    /// </summary>
    /// <param name="file">Name of the JavaScript file to import (e.g. JAVASCRIPT_ROOT + "Button/FluentButton.razor.js").</param>
    /// <returns></returns>
    protected virtual async Task<IJSObjectReference> ImportJavaScriptModuleAsync(string file)
    {
        if (_jsModule is null)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", file);  // .FormatCollocatedUrl(LibraryConfiguration)
        }

        return _jsModule;
    }

    /// <summary>
    /// Gets or sets the unique identifier.
    /// The value will be used as the HTML <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/id">global id attribute</see>.
    /// </summary>
    [Parameter]
    public virtual string? Id { get; set; }

    /// <summary>
    /// Gets or sets the CSS class names. If given, these will be included in the class attribute of the component.
    /// </summary>
    [Parameter]
    public virtual string? Class { get; set; }

    /// <summary>
    /// Gets or sets the in-line styles. If given, these will be included in the style attribute of the component.
    /// </summary>
    [Parameter]
    public virtual string? Style { get; set; }

    /// <summary>
    /// Gets or sets custom data, to attach any user data object to the component.
    /// </summary>
    [Parameter]
    public virtual object? Data { get; set; }

    /// <summary>
    /// Gets or sets a collection of additional attributes that will be applied to the created element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public virtual IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }
}
