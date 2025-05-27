// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a key press event, including the key pressed and associated modifier keys.
/// </summary>
/// <remarks>The <see cref="KeyPress"/> record provides a way to encapsulate information about a key press event,
/// including the key itself, modifier keys (e.g., Ctrl, Alt, Shift, Meta), and whether the event should be prevented
/// from propagating further in the client-side environment.
/// </remarks>
public record KeyPress
{
    /// <summary>
    /// Creates a new <see cref="KeyPress"/> instance for the specified key.
    /// </summary>
    /// <param name="key">The key to associate with the <see cref="KeyPress"/> instance.</param>
    /// <returns>A <see cref="KeyPress"/> instance with the specified key.</returns>
    public static KeyPress For(KeyCode key)
    {
        return new KeyPress
        {
            Key = key,
        };
    }

    /// <summary>
    /// Gets the <see cref="KeyCode"/> equivalent value.
    /// </summary>
    public KeyCode Key { get; init; }

    /// <summary>
    /// Gets a boolean value that indicates if the Control key was pressed.
    /// Default is false.
    /// </summary>
    public bool CtrlKey { get; init; }

    /// <summary>
    /// Gets a boolean value that indicates if the Shift key was pressed.
    /// Default is false.
    /// </summary>
    public bool ShiftKey { get; init; }

    /// <summary>
    /// Gets a boolean value that indicates if the Alt key was pressed.
    /// Default is false.
    /// </summary>
    public bool AltKey { get; init; }

    /// <summary>
    /// Gets a boolean value that indicates if the Meta key was pressed.
    /// Default is false.
    /// </summary>
    public bool MetaKey { get; init; }

    /// <summary>
    /// Gets or sets a boolean value that indicates if the JS client event should be cancelled.
    /// Default is true, this means that the key will not be propagated and added to the component value.
    /// </summary>
    public bool PreventDefault { get; init; } = true;

    /// <summary>
    /// Returns a new <see cref="KeyPress"/> instance with the <see cref="CtrlKey"/> property set to the specified value.
    /// </summary>
    /// <param name="pressed">A value indicating whether the Ctrl key is pressed. The default is <see langword="true"/>.</param>
    /// <returns>A new <see cref="KeyPress"/> instance with the updated <see cref="CtrlKey"/> property.</returns>
    public KeyPress AndCtrlKey(bool pressed = true)
    {
        return this with { CtrlKey = pressed };
    }

    /// <summary>
    /// Returns a new <see cref="KeyPress"/> instance with the <see cref="AltKey"/> property set to the specified value.
    /// </summary>
    /// <param name="pressed">A value indicating whether the Alt key is pressed. The default is <see langword="true"/>.</param>
    /// <returns>A new <see cref="KeyPress"/> instance with the updated <see cref="AltKey"/> property.</returns>
    public KeyPress AndAltKey(bool pressed = true)
    {
        return this with { AltKey = pressed };
    }

    /// <summary>
    /// Returns a new <see cref="KeyPress"/> instance with the <see cref="ShiftKey"/> property set to the specified value.
    /// </summary>
    /// <param name="pressed">A value indicating whether the Shift key is pressed. The default is <see langword="true"/>.</param>
    /// <returns>A new <see cref="KeyPress"/> instance with the updated <see cref="ShiftKey"/> property.</returns>
    public KeyPress AndShiftKey(bool pressed = true)
    {
        return this with { ShiftKey = pressed };
    }

    /// <summary>
    /// Returns a new <see cref="KeyPress"/> instance with the <see cref="MetaKey"/> property set to the specified value.
    /// </summary>
    /// <param name="pressed">A value indicating whether the Meta key is pressed. The default is <see langword="true"/>.</param>
    /// <returns>A new <see cref="KeyPress"/> instance with the updated <see cref="MetaKey"/> property.</returns>
    public KeyPress AndMetaKey(bool pressed = true)
    {
        return this with { MetaKey = pressed };
    }

    /// <summary>
    /// Returns a new <see cref="KeyPress"/> instance with the <see cref="PreventDefault"/> property set to the specified value.
    /// </summary>
    /// <param name="prevent">A value indicating whether the PreventDefault value is set. The default is <see langword="false"/>.</param>
    /// <returns>A new <see cref="KeyPress"/> instance with the updated <see cref="PreventDefault"/> property.</returns>
    public KeyPress WithPreventDefault(bool prevent = false)
    {
        return this with { PreventDefault = prevent };
    }
}
