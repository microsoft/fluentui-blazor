namespace Microsoft.FluentUI.AspNetCore.Components;

public enum KeyCode
{
    Backspace = 8,
    Tab = 9,
    Enter = 13,
    Shift = 16,
    Ctrl = 17,
    Alt = 18,
    PauseBreak = 19,
    CapsLock = 20,
    Escape = 27,
    Space = 32,
    PageUp = 33,
    PageDown = 34,
    End = 35,
    Home = 36,
    Left = 37,
    Up = 38,
    Right = 39,
    Down = 40,
    Insert = 45,
    Delete = 46,
    Command = 91,
    LeftCommand = 91,   // Windows
    RightCommand = 93,
    NumpadMultiply = 106,
    NumpadAdd = 107,
    NumpadSubtract = 109,
    NumpadDecimal = 110,
    NumpadDivide = 111,
    NumLock = 144,
    ScrollLock = 145,
    MyComputer = 182,
    MyCalculator = 183,
    Semicolon = 186,
    Equal = 187,
    Comma = 188,
    Minus = 189,
    Dot = 190,
    Divide = 191,
    QuoteRight = 192,
    BracketRight = 219,
    BackSlash = 220,
    BracketLeft = 221,
    SimpleQuote = 22,

    /*
     *  // lower case chars
        for (i = 97; i < 123; i++) codes[String.fromCharCode(i)] = i - 32

        // numbers
        for (var i = 48; i < 58; i++) codes[i - 48] = i

        // function keys
        for (i = 1; i < 13; i++) codes['f'+i] = i + 111

        // numpad keys
        for (i = 0; i < 10; i++) codes['numpad '+i] = i + 96
     */
}
