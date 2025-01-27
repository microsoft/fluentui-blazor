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
    private static readonly char[] InvalidCharsInClassName = ['!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '+', '=', '{', '}', '[', ']', '|', '\\', ':', ';', '"', '\'', '<', '>', ',', '?', '/'];

    /// <summary>
    /// Converts a spacing value to a CSS styleValue.
    /// The <paramref name="value"/> can be a string with one or more values separated by spaces, coma or semicolon.
    /// You can use the `padding` or `margin` pattern defined my the W3C
    /// or a classValue name if the value is not a valid CSS keyword like `auto`, `inherit`, `initial`, ...
    /// </summary>
    /// <example>
    ///  - ConvertSpacing("auto")                  => Style = "auto"               Class = ""
    ///  - ConvertSpacing("10px")                  => Style = "10px"               Class = ""
    ///  - ConvertSpacing("10px 20px")             => Style = "10px 20px"          Class = ""
    ///  - ConvertSpacing("10px 20px 30px")        => Style = "10px 20px 30px"     Class = ""
    ///  - ConvertSpacing("mr-0")                  => Style = ""                   Class = "mr-0"
    ///  - ConvertSpacing("my-classValue")         => Style = ""                   Class = "my-classValue"
    ///  - ConvertSpacing("mr-0 my-classValue")    => Style = ""                   Class = "mr-0 my-classValue"
    /// </example>
    /// <param name="value"></param>    
    /// <returns></returns>
    public static (string Style, string Class) ConvertSpacing(this string? value)
    {
        // To optimize multiple calls with the same value
        if (string.Equals(_previousSpacingToStyle.Key, value, StringComparison.Ordinal))
        {
            return _previousSpacingToStyle.Value;
        }

        if (value == null)
        {
            return SaveInCacheAndGet(value, "", "");
        }

        var values = value.Split(Separators, StringSplitOptions.RemoveEmptyEntries)
                          .Select(i => i.Trim())
                          .ToArray();

        // No value
        if (values.Length == 0)
        {
            return SaveInCacheAndGet(value, "", "");
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
            return SaveInCacheAndGet(value, values[0], "");
        }

        // In CSS, classValue names cannot start with a digit.
        // The margin/padding syntax is a number followed by a unit (px, em, rem, %, ...)
        var isStyle = char.IsDigit(firstValue[0]) ||
                      firstValue[0] == '-' && firstValue.Length > 1 && char.IsDigit(firstValue[1]) ||
                      firstValue[0] == '+' && firstValue.Length > 1 && char.IsDigit(firstValue[1]);

        if (isStyle)
        {
            return SaveInCacheAndGet(value, string.Join(' ', AddPxWhenNeeded(values)), "");
        }

        // A `calc` function is a valid style
        if (firstValue.StartsWith("calc(", StringComparison.OrdinalIgnoreCase))
        {
            return SaveInCacheAndGet(value, value, "");
        }

        // Check if value contains invalid characters for a class name
        if (value.IndexOfAny(InvalidCharsInClassName) >= 0)
        {
            return SaveInCacheAndGet(value, "", "");
        }

        // Like a class name
        return SaveInCacheAndGet(value, "", value);
    }

    /// <summary>
    /// Add missing `px` to a value if it is an integer number.
    /// </summary>
    /// <example>
    /// AddMissingPx("10") => "10px"
    /// AddMissingPx("10px") => "10px"
    /// AddMissingPx("10%") => "10%"
    /// AddMissingPx("10.0") => "10.0"
    /// </example>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string? AddMissingPx(this string? value)
    {
        if (int.TryParse(value, CultureInfo.InvariantCulture, out var pixels))
        {
            return $"{pixels}px";
        }

        return value;
    }

    private static (string Style, string Class) SaveInCacheAndGet(string? value, string styleValue, string classValue)
    {
        _previousSpacingToStyle = new KeyValuePair<string?, (string, string)>(value, (styleValue, classValue));
        return (styleValue, classValue);
    }

    private static string[] AddPxWhenNeeded(string[] values)
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
