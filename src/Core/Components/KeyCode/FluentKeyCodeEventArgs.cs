namespace Microsoft.FluentUI.AspNetCore.Components;

public class FluentKeyCodeEventArgs
{
    internal static FluentKeyCodeEventArgs Instance(string name, int keyCode, string value, bool ctrlKey, bool shiftKey, bool altKey, bool metaKey, int location, string targetId)
    {
        return new FluentKeyCodeEventArgs
        {
            Name = name,
            Location = Enum.IsDefined(typeof(KeyLocation), location) ? (KeyLocation)location : KeyLocation.Unknown,
            Key = Enum.IsDefined(typeof(KeyCode), keyCode) ? (KeyCode)keyCode : AspNetCore.Components.KeyCode.Unknown,
            KeyCode = keyCode,
            Value = value,
            CtrlKey = ctrlKey,
            ShiftKey = shiftKey,
            AltKey = altKey,
            MetaKey = metaKey,
            TargetId = targetId,
        };
    }

    /// <summary>
    /// Gets the name of the event ("keydown" or "keyup").
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Gets an <see cref="KeyLocation" /> representing the location of the key on the keyboard or other input device.
    /// <see href="https://developer.mozilla.org/en-US/docs/Web/API/KeyboardEvent/location"/>
    /// </summary>
    public KeyLocation Location { get; init; }

    /// <summary>
    /// Gets the <see cref="KeyCode"/> equivalent value.
    /// </summary>
    public KeyCode Key { get; init; }

    /// <summary>
    /// Gets the system- and implementation-dependent numerical code identifying the unmodified value of the pressed key.
    /// <see href="https://developer.mozilla.org/en-US/docs/Web/API/KeyboardEvent/keyCode"/>
    /// </summary>
    public int KeyCode { get; init; }

    /// <summary>
    /// Gets the value of the key pressed by the user
    /// </summary>
    public string Value { get; init; } = string.Empty;

    /// <summary>
    /// Gets a boolean value that indicates if the Control key was pressed
    /// </summary>
    public bool CtrlKey { get; init; }

    /// <summary>
    /// Gets a boolean value that indicates if the Shift key was pressed
    /// </summary>
    public bool ShiftKey { get; init; }

    /// <summary>
    /// Gets a boolean value that indicates if the Alt key was pressed
    /// </summary>
    public bool AltKey { get; init; }

    /// <summary>
    /// Gets a boolean value that indicates if the Meta key was pressed
    /// </summary>
    public bool MetaKey { get; init; }

    /// <summary>
    /// Gets the identifier of the targeted DOM element.
    /// </summary>
    public string TargetId { get; init; } = string.Empty;

    /// <summary>
    /// Returns a string that represents the key pressed.
    /// Example: "Ctrl + Shift + A"
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        var keys = new string[]
            {
                CtrlKey ? "Ctrl" : string.Empty,
                ShiftKey ? "Shift" : string.Empty,
                AltKey ? "Alt" : string.Empty,
                MetaKey ? "Meta" : string.Empty,
                Value.Length == 1 ? (Value == " " ? "Space" :Value.ToUpper()) : Value
            };

        return string.Join(" + ", keys.Where(i => !string.IsNullOrEmpty(i)));
    }
}
