// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A base class for Fluent UI form input components. This base class automatically
/// integrates with an <see cref="EditContext"/>, which must be supplied
/// as a cascading parameter.
/// </summary>
/// <typeparam name="TValue">The type of the value to be edited.</typeparam>
public abstract partial class FluentInputBase<TValue> : InputBase<TValue>, IFluentComponentBase
{
    private FluentJSModule? _jsModule;

    /// <summary />
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary>
    /// Gets the JavaScript module imported with the <see cref="FluentJSModule.ImportJavaScriptModuleAsync"/> method.
    /// You need to call this method (in the `OnAfterRenderAsync` method) before using the module.
    /// </summary>
    internal FluentJSModule JSModule => _jsModule ??= new FluentJSModule(JSRuntime);

    #region IFluentComponentBase

    /// <inheritdoc />
    [Parameter]
    public virtual string? Id { get; set; }

    /// <inheritdoc />
    [Parameter]
    public virtual string? Class { get; set; }

    /// <inheritdoc />
    [Parameter]
    public virtual string? Style { get; set; }

    /// <inheritdoc />
    [Parameter]
    public virtual object? Data { get; set; }

    #endregion

    #region FluentInputBase

    /// <summary>
    /// Gets a CSS class string that combines the `Class` attribute and and a string indicating
    /// the status of the field being edited (a combination of "modified", "valid", and "invalid").
    /// Derived components should typically use this value for the primary HTML element class attribute.
    /// </summary>
    protected virtual string? ClassValue => new CssBuilder(Class).AddClass(base.CssClass).Build();

    /// <summary>
    /// Gets the optional in-line styles. If given, these will be included in the style attribute of the component.
    /// </summary>
    protected virtual string? StyleValue => new StyleBuilder(Style).Build();

    /// <summary>
    /// Determines if the element should receive document focus on page load.
    /// </summary>
    [Parameter]
    public virtual bool Autofocus { get; set; }

    /// <summary>
    /// Gets or sets the text used on `aria-label` attribute.
    /// </summary>
    [Parameter]
    public virtual string? AriaLabel { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the form control is disabled and doesn't participate in form submission.
    /// </summary>
    [Parameter]
    public virtual bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets the text to label the input. This is usually displayed just above the input.
    /// </summary>
    [Parameter]
    public virtual string? Label { get; set; }

    /// <summary>
    /// Gets or sets the content to label the input component.
    /// This is usually displayed just above the input
    /// </summary>
    [Parameter]
    public virtual RenderFragment? LabelTemplate { get; set; }

    /// <summary>
    /// Gets or sets the name of the element.
    /// Allows access by name from the associated form.
    /// ⚠️ This value needs to be set manually for SSR scenarios to work correctly.
    /// </summary>
    [Parameter]
    public virtual string? Name { get; set; }

    /// <summary>
    /// Gets or sets whether the control will be immutable by user interaction.
    /// </summary>
    [Parameter]
    public virtual bool ReadOnly { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the element needs to have a value.
    /// </summary>
    [Parameter]
    public bool Required { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "TODO")]
    protected virtual Task ChangeHandlerAsync(ChangeEventArgs e)
    {
        var isValid = TryParseValueFromString(e.Value?.ToString(), out var result, out var validationErrorMessage);

        if (isValid)
        {
            CurrentValue = result;
        }
        else
        {
            // TODO
        }

        return Task.CompletedTask;
    }

    #endregion
}
