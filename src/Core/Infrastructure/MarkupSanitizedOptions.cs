// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.RegularExpressions;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Markup sanitization configuration options.
/// </summary>
public class MarkupSanitizedOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether an exception is thrown when an unsafe operation is detected.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0069:Non-constant static fields should not be visible", Justification = "The developer could modify this value globally.")]
    public static bool ThrowOnUnsafe = true;

    /// <summary />
    internal static readonly MarkupSanitizedOptions Default = new();

    /// <summary>
    /// Gets or sets the function used to sanitize inline CSS style attribute values before rendering.
    /// </summary>
    public Func<string, string> SanitizeInlineStyle { get; set; } = (value) =>
    {
        // A very string pattern to allow only safe CSS values.
        var pattern = @"^(?!.*(behavior\s*:|expression\s*\(|-moz-binding\s*:|javascript\s*:|url\s*\(\s*['""]?\s*javascript\s*:))[a-zA-Z0-9\s,.:;_%\-()#{}pxemremvhsmsdegradturn]+$";
        var regex = new Regex(pattern, RegexOptions.IgnoreCase, matchTimeout: TimeSpan.FromMilliseconds(400));
        var isValid = regex.IsMatch(value);

        if (isValid)
        {
            return value;
        }

        if (MarkupSanitizedOptions.ThrowOnUnsafe)
        {
            throw new InvalidOperationException($"The provided CSS style contains potentially unsafe content: {value}");
        }

        return string.Empty;
    };
}
