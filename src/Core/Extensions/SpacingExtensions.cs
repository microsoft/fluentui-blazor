// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Globalization;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Convert a spacing value to a CSS styleValue.
/// </summary>
public static class SpacingExtensions
{
    private static KeyValuePair<string?, (string Style, string Class)> _previousSpacingToStyle = new(key: null, value: ("", ""));
    private static readonly char[] Separators = [' ', ';'];
    private static readonly string[] StyleKeyWords = ["auto", "inherit", "initial", "revert", "revert-layer", "unset"];

    /// <summary>
    /// Converts a spacing value to a CSS styleValue.
    /// The <paramref name="value"/> can be a string with one or more values separated by spaces, coma or semicolon.
    /// You can use the `padding` or `margin` pattern defined my the W3C
    /// or a classValue name if the value is not a valid CSS keyword like `auto`, `inherit`, `initial`, ...
    /// </summary>
    /// <example>
    ///  - SpacingToStyle("auto")             => Style = "auto"               Class = ""
    ///  - SpacingToStyle("10px")             => Style = "10px"               Class = ""
    ///  - SpacingToStyle("10px 20px")        => Style = "10px 20px"          Class = ""
    ///  - SpacingToStyle("10px 20px 30px")   => Style = "10px 20px 30px"     Class = ""
    ///  - SpacingToStyle("mr-0")             => Style = ""                   Class = "mr-0"
    ///  - SpacingToStyle("my-classValue")         => Style = ""                   Class = "my-classValue"
    ///  - SpacingToStyle("mr-0 my-classValue")    => Style = ""                   Class = "mr-0 my-classValue"
    /// </example>
    /// <param name="value"></param>    
    /// <returns></returns>
    public static (string Style, string Class) SpacingToStyle(this string? value)
    {
        // To optimize multiple calls with the same value
        if (string.Equals(_previousSpacingToStyle.Key, value, StringComparison.Ordinal))
        {
            return _previousSpacingToStyle.Value;
        }

        if (value == null)
        {
            return SaveInCacheAndReturns(value, "", "");
        }

        var values = value.Split(Separators, StringSplitOptions.RemoveEmptyEntries)
                          .Select(i => i.Trim())
                          .ToArray();

        // No value
        if (values.Length == 0)
        {
            return SaveInCacheAndReturns(value, "", "");
        }

        // A keyword cannot be used with other values
        if (values.Length > 1 && values.Intersect(StyleKeyWords, StringComparer.OrdinalIgnoreCase).Any())
        {
            throw new ArgumentException("The value cannot contain a CSS keyword and a class name or style value.", nameof(value));
        }

        // A keyword
        var firstValue = values[0];
        if (StyleKeyWords.Contains(firstValue, StringComparer.OrdinalIgnoreCase))
        {
            return SaveInCacheAndReturns(value, values[0], "");
        }

        // In CSS, classValue names cannot start with a digit.
        // The margin/padding syntax is a number followed by a unit (px, em, rem, %, ...)
        var isStyle = char.IsDigit(firstValue[0]) ||
                      firstValue[0] == '-' && firstValue.Length > 1 && char.IsDigit(firstValue[1]) ||
                      firstValue[0] == '+' && firstValue.Length > 1 && char.IsDigit(firstValue[1]);

        if (isStyle)
        {
            return SaveInCacheAndReturns(value, string.Join(' ', AddMissingPixels(values)), "");
        }

        return SaveInCacheAndReturns(value, "", value);
    }

    private static (string Style, string Class) SaveInCacheAndReturns(string? value, string styleValue, string classValue)
    {
        _previousSpacingToStyle = new KeyValuePair<string?, (string, string)>(value, (styleValue, classValue));
        return (styleValue, classValue);
    }

    private static string[] AddMissingPixels(string[] values)
    {
        return values.Select(v => IsNumber(v) ? v + "px" : v).ToArray();
    }

    private static bool IsNumber(string value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? false
            : double.TryParse(value, CultureInfo.InvariantCulture, out _);
    }
}
