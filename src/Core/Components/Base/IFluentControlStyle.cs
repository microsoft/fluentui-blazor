// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/*
 *  -----------------------------------------------------------------------------------------------------------------------------------
 *  Example of usage (the component class must implement `IFluentComponentElementBase` and `IFluentControlStyle`):
 *  -----------------------------------------------------------------------------------------------------------------------------------
 *
 *  /// <inheritdoc cref="IFluentControlStyle.ControlStyle" />
 *  [Parameter]
 *  public string? ControlStyle { get; set; }
 *
 *  ------------------------------------------
 *  And in the `OnAfterRenderAsync` method:
 *  ------------------------------------------
 *
 *  protected override async Task OnAfterRenderAsync(bool firstRender)
 *  {
 *      if (firstRender && !string.IsNullOrEmpty(ControlStyle))
 *      {
 *          await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Utilities.Attributes.applyShadowStyle", Element, ":host .control", ControlStyle);
 *      }
 *  }
 *
 */

/// <summary>
/// Adds support for applying custom CSS directly to a component's internal "control" element inside its
/// shadow DOM (the wrapped <c>&lt;input&gt;</c> or <c>&lt;textarea&gt;</c>), for style rules that
/// <c>::part(control)</c> cannot reach from outside the shadow boundary, such as pseudo-elements like
/// <c>::-ms-reveal</c> (the Edge password reveal icon).
/// Only components that expose a single, styleable internal control element should implement this
/// interface (e.g. <see cref="FluentTextInput"/>, <see cref="FluentTextArea"/>). Components without such
/// an internal element (e.g. <see cref="FluentCheckbox"/>, <see cref="FluentSwitch"/>) should not, since
/// their host element can already be styled directly with <see cref="IFluentComponentBase.Style"/>.
/// </summary>
public interface IFluentControlStyle
{
    /// <summary>
    /// Gets or sets custom CSS applied to the internal control element (inside the shadow DOM).
    /// Plain declarations without a selector (e.g. <c>"color: red;"</c>) are applied to the control element
    /// itself; text containing a selector (e.g. <c>"::-ms-reveal { display: none; }"</c>) is injected as-is.
    /// Only applied on first render; later changes to this parameter are not reflected.
    /// </summary>
    string? ControlStyle { get; set; }
}
