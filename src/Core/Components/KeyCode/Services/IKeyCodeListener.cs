// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Defines a contract for handling key press and release events asynchronously.
/// </summary>
public interface IKeyCodeListener
{
    /// <summary>
    /// Method called when a key is pressed.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    Task OnKeyDownAsync(FluentKeyCodeEventArgs args);

    /// <summary>
    /// Method called when a key is unpressed.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    Task OnKeyUpAsync(FluentKeyCodeEventArgs args);
}
