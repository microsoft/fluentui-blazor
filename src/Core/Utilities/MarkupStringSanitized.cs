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
        var pattern = @"^(?!.*(behavior\s*:|expression\s*\(|-moz-binding\s*:|javascript\s*:|url\s*\(\s*['""]?\s*javascript\s*:))[a-zA-Z0-9\s,.:;_%\-()#{}'""pxemremvhsmsdegradturn]+$";
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
    /// Provides the default function for sanitizing tag names by removing invalid characters.
    /// </summary>
    /// <remarks>The default sanitizer allows only ASCII letters (A–Z, a–z), digits (0–9), hyphens (-),
    /// periods (.), colons (:), and underscores (_). Characters outside this set may be removed to ensure
    /// the tag name is valid for its intended use.
    /// </remarks>
    internal static readonly Func<string, string> DefaultSanitizeTagName = (value) =>
    {
        var pattern = @"[^a-zA-Z0-9\-.:_]";
        var regex = new Regex(pattern, RegexOptions.None, matchTimeout: TimeSpan.FromMilliseconds(400));
        var sanitized = regex.Replace(value, string.Empty);

        return sanitized;
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="MarkupStringSanitized"/> struct.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="configuration"></param>
    private MarkupStringSanitized(ParsedContent element, LibraryConfiguration? configuration)
    {
        _configuration = configuration?.MarkupSanitized ?? MarkupSanitizedOptions.Default;

        var sanitizedElement = new ParsedContent(
            MarkupStringSanitized.DefaultSanitizeTagName(element.Tag),
            string.IsNullOrEmpty(element.Attribute) ? null : MarkupStringSanitized.DefaultSanitizeTagName(element.Attribute),
            string.IsNullOrEmpty(element.AttributeValue) ? null : _configuration.SanitizeInlineStyle(element.AttributeValue),
            element.Quote,
            string.IsNullOrEmpty(element.Content) ? null : _configuration.SanitizeInlineStyle(element.Content));

        if (string.IsNullOrEmpty(sanitizedElement.Content))
        {
            if (string.IsNullOrEmpty(sanitizedElement.Attribute) || string.IsNullOrEmpty(sanitizedElement.AttributeValue))
            {
                Value = $"<{sanitizedElement.Tag} />";
            }

            else
            {
                Value = $"<{sanitizedElement.Tag} {sanitizedElement.Attribute}={sanitizedElement.Quote}{sanitizedElement.AttributeValue}{sanitizedElement.Quote} />";
            }
        }
        else
        {
            if (string.IsNullOrEmpty(sanitizedElement.Attribute) || string.IsNullOrEmpty(sanitizedElement.AttributeValue))
            {
                Value = $"<{sanitizedElement.Tag}>{sanitizedElement.Content}</{sanitizedElement.Tag}>";
            }
            else
            {
                Value = $"<{sanitizedElement.Tag} {sanitizedElement.Attribute}={sanitizedElement.Quote}{sanitizedElement.AttributeValue}{sanitizedElement.Quote}>{sanitizedElement.Content}</{sanitizedElement.Tag}>";
            }
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MarkupStringSanitized"/> struct,
    /// where the element must be in the format "<tag>content</tag>" or "<tag style='style-element'>content</tag>".
    /// </summary>
    /// <param name="value"></param>
    /// <param name="configuration"></param>
    /// <exception cref="ArgumentException"></exception>
    public MarkupStringSanitized(string value, LibraryConfiguration? configuration)
        : this(ParseTagAndContent(value), configuration) { }

    /// <summary>
    /// Gets the element as a markup string suitable for rendering as HTML content.
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
    /// ToString override to return the sanitized string element.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Value ?? string.Empty;

    [GeneratedRegex(@"^<(?<tag>[a-zA-Z][a-zA-Z0-9]*?)(?:\s+(?<attribute>[a-zA-Z][a-zA-Z0-9\-]*?)\s*=\s*(?<quote>[""'])(?<attributevalue>.*?)\k<quote>)?(?:\s*/)?>(?<content>[\s\S]*?)(?:</\k<tag>>)?$", RegexOptions.Singleline | RegexOptions.ExplicitCapture, matchTimeoutMilliseconds: 300)]
    private static partial Regex RegExWithTagAndStyle();

    /// <summary />
    internal static ParsedContent ParseTagAndContent(string value)
    {
        // Explanation of capture groups:
        // 1. (?<tag>[a-zA-Z][a-zA-Z0-9]*?)           - Captures the tag name
        // 2. (?<attribute>[a-zA-Z][a-zA-Z0-9\-]*?)   - Captures the attribute name (e.g., "style")
        // 3. (?<quote>[""'])                         - Captures the quote type (single or double)
        // 4. (?<attributevalue>.*?)                  - Captures the attribute element
        // 5. (?<content>[\s\S]*?)                    - Captures the content between tags

        var match = RegExWithTagAndStyle().Match(value);

        // Valid format: <tag>content</tag> or <tag style="style">content</tag>
        if (match.Success)
        {
            var tag = match.Groups["tag"].Value;
            var quote = match.Groups["quote"].Value;
            var attribute = match.Groups["attribute"].Value;
            var attributeValue = match.Groups["attributevalue"].Value;
            var content = match.Groups["content"].Value;

            return new ParsedContent(
                tag,
                string.IsNullOrEmpty(attribute) ? null : attribute,
                string.IsNullOrEmpty(attributeValue) ? null : attributeValue,
                string.IsNullOrEmpty(quote) ? '\'' : quote[0],
                string.IsNullOrEmpty(content) ? null : content);
        }

        throw new ArgumentException("Value must be in the format '<tag>content</tag>' or '<tag attribute=\"attr-value\">content</tag>'.", nameof(value));
    }

    /// <summary />
    internal record ParsedContent(string Tag, string? Attribute, string? AttributeValue, char Quote, string? Content);
}
