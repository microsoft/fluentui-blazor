// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components.Utilities;

/// <summary>
/// MarkupString wrapper that sanitizes the content for safe usage in inline style attributes.
/// </summary>
public readonly struct MarkupStringSanitized
{
    private readonly MarkupSanitizedOptions _configuration;

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
}
