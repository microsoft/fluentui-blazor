namespace Microsoft.FluentUI.AspNetCore.Components;

// See https://developer.mozilla.org/en-US/docs/Web/API/KeyboardEvent/location

/// <summary>
/// the location of the key on the keyboard or other input device
/// </summary>
public enum KeyLocation
{
    /// <summary>
    /// Not defined
    /// </summary>
    Unknown = -1,

    /// <summary>
    /// The key has only one version, or can't be distinguished between the left and right versions of the key, and was not pressed on the numeric keypad or a key that is considered to be part of the keypad.
    /// </summary>
    Standard = 0,

    /// <summary>
    /// The key was the left-hand version of the key; for example, the left-hand Control key was pressed on a standard 101 key US keyboard. This value is only used for keys that have more than one possible location on the keyboard.
    /// </summary>
    Left = 1,

    /// <summary>
    /// The key was the right-hand version of the key; for example, the right-hand Control key is pressed on a standard 101 key US keyboard. This value is only used for keys that have more than one possible location on the keyboard.
    /// </summary>
    Right = 2,

    /// <summary>
    /// The key was on the numeric keypad, or has a virtual key code that corresponds to the numeric keypad.
    /// </summary>
    NumPad = 3,

    /// <summary>
    /// The key was on a mobile device; this can be on either a physical keypad or a virtual keyboard.
    /// </summary>
    [Obsolete]
    Mobile = 4,

    /// <summary>
    /// The key was a button on a game controller or a joystick on a mobile device.
    /// </summary>
    [Obsolete]
    Joystick = 5,
}
