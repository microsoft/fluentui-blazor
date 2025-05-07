// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/*  
 *  -----------------------------------------------------------------------------------------------------------------------------------
 *  Example of usage (the component class must implemented `IFluentComponentElementBase` and `IFluentComponentChangeAfterKeyPress` :
 *  -----------------------------------------------------------------------------------------------------------------------------------
 *  
 *  /// <inheritdoc cref="IFluentComponentChangeAfterKeyPress.ChangeAfterKeyPress" />
 *  [Parameter]
 *  public KeyPress[]? ChangeAfterKeyPress { get; set; }
 *
 *  /// <inheritdoc cref="IFluentComponentChangeAfterKeyPress.OnChangeAfterKeyPress" />
 *  [Parameter]
 *  public EventCallback<FluentKeyPressEventArgs> OnChangeAfterKeyPress { get; set; }
 *
 *  /// <inheritdoc cref="IFluentComponentChangeAfterKeyPress.ChangeAfterKeyPressHandlerAsync(string, KeyPress)" />
 *  [JSInvokable]
 *  public async Task ChangeAfterKeyPressHandlerAsync(string value, KeyPress key)
 *  {
 *      await ChangeHandlerAsync(new ChangeEventArgs() { Value = value });
 *
 *      if (OnChangeAfterKeyPress.HasDelegate)
 *      {
 *          await OnChangeAfterKeyPress.InvokeAsync(new FluentKeyPressEventArgs() { KeyPress = key});
 *      }
 *  }
 * 
 *  ------------------------------------------
 *  And in the `OnAfterRenderAsync` method:
 *  ------------------------------------------
 *  
 *  protected override async Task OnAfterRenderAsync(bool firstRender)
 *  {
 *      if (firstRender)
 *      {
 *          // Initialize the change after key press event
 *          await IFluentComponentChangeAfterKeyPress.InitializeRuntimeAsync(this, JSRuntime, Element);
 *      }
 *  }
 *  
 */

/// <summary>
/// 
/// </summary>
public interface IFluentComponentChangeAfterKeyPress
{
    /// <summary>
    /// Gets or sets the key press events that will trigger a change.
    /// This property must have a `[Parameter]` attribute to be used in the component.
    /// </summary>
    KeyPress[]? ChangeAfterKeyPress { get; set; }

    /// <summary>
    /// Gets or sets the callback to be invoked when the value changes after a <see cref="ChangeAfterKeyPress"/> key press.
    /// This property must have a `[Parameter]` attribute to be used in the component.
    /// </summary>
    EventCallback<FluentKeyPressEventArgs> OnChangeAfterKeyPress { get; set; }

    /// <summary>
    /// Handles the change in value after a key press event.
    /// This method is called by the JavaScript function and must have a `[JSInvokable]` attribute.
    /// </summary>
    Task ChangeAfterKeyPressHandlerAsync(string value, KeyPress key);
  
    /// <summary>
    /// Initializes a key press event listener for the specified component, enabling the execution of actions after
    /// specific key presses.
    /// </summary>
    /// <remarks>This method sets up a JavaScript event listener for key press events on the specified DOM element</remarks>
    /// <param name="component">The component implementing <see cref="IFluentComponentChangeAfterKeyPress"/> that defines the key press behavior.</param>
    /// <param name="jsRuntime">The <see cref="IJSRuntime"/> instance used to invoke JavaScript interop calls.</param>
    /// <param name="element">The <see cref="ElementReference"/> representing the DOM element to which the key press event listener will be attached.</param>
    /// <returns></returns>
    internal static async Task InitializeRuntimeAsync(IFluentComponentChangeAfterKeyPress component, IJSRuntime jsRuntime, ElementReference element)
    {
        if (component.ChangeAfterKeyPress != null && component.ChangeAfterKeyPress.Length > 0)
        {
            await jsRuntime.InvokeVoidAsync(
                "Microsoft.FluentUI.Blazor.Utilities.KeyPress.addKeyPressEventListener",
                element,
                DotNetObjectReference.Create(component),
                component.ChangeAfterKeyPress);
        }
    }
}
