// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components.Utilities;

/// <summary>
/// MarkupString wrapper that sanitizes the content for safe usage in inline style attributes.
/// </summary>
public readonly partial struct MarkupStringSanitized
{
    private readonly MarkupSanitizedOptions _configuration;
    private const string DeveloperExceptionMessage = $"You can customize the `{nameof(LibraryConfiguration.MarkupSanitized)}` option during the `Services.AddFluentUIComponents()` call.";

    /// <summary>
    /// Default inline style sanitizer function.
    /// </summary>
    internal static readonly Func<string, string> DefaultSanitizeInlineStyle = (value) =>
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
            throw new InvalidOperationException($"The provided CSS inline style contains potentially unsafe content: {value}. {DeveloperExceptionMessage}");
        }

        return string.Empty;
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="MarkupStringSanitized"/> struct.
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="value"></param>
    /// <param name="configuration"></param>
    public MarkupStringSanitized(string tag, string value, LibraryConfiguration? configuration)
    {
        _configuration = configuration?.MarkupSanitized ?? MarkupSanitizedOptions.Default;

        Value = $"<{tag}>{_configuration.SanitizeInlineStyle(value)}</{tag}>";
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MarkupStringSanitized"/> struct,
    /// where the value must be in the format "<tag>content</tag>".
    /// </summary>
    /// <param name="value"></param>
    /// <param name="configuration"></param>
    /// <exception cref="ArgumentException"></exception>
    public MarkupStringSanitized(string value, LibraryConfiguration? configuration)
    {
        _configuration = configuration?.MarkupSanitized ?? MarkupSanitizedOptions.Default;

        var match = RegExWithTag().Match(value);

        if (!match.Success)
        {
            throw new ArgumentException("Value must be in the format '<tag>content</tag>'.", nameof(value));
        }

        var tag = match.Groups[1].Value;
        var content = match.Groups[2].Value;

        Value = $"<{tag}>{_configuration.SanitizeInlineStyle(content)}</{tag}>";
    }

    /// <summary>
    /// Gets the value as a markup string suitable for rendering as HTML content.
    /// </summary>
    public readonly MarkupString Markup => new(Value);

    /// <summary>
    /// Value of the sanitized markup string.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Operator to convert a MarkupStringSanitized to a MarkupString.
    /// </summary>
    /// <param name="value"></param>
    public static implicit operator MarkupString(MarkupStringSanitized value) => new(value.Value);

    /// <summary>
    /// ToString override to return the sanitized string value.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Value ?? string.Empty;

    [GeneratedRegex(@"^<(?<tag>[a-zA-Z][a-zA-Z0-9]*)>(?<content>[\s\S]{0,4096})<\/\k<tag>>$", RegexOptions.Singleline | RegexOptions.ExplicitCapture, matchTimeoutMilliseconds: 300)]
    private static partial System.Text.RegularExpressions.Regex RegExWithTag();
}
