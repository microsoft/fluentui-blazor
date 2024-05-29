// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

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
