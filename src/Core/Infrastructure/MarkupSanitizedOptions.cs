// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.Utilities;

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
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "The developer could modify this value globally.")]
    public static bool ThrowOnUnsafe = true;

    /// <summary />
    internal static readonly MarkupSanitizedOptions Default = new();

    /// <summary>
    /// Gets or sets the function used to sanitize inline CSS style attribute values before rendering.
    /// See <see cref="MarkupStringSanitized.DefaultSanitizeInlineStyle"/> for more details.
    /// </summary>
    public Func<string, string> SanitizeInlineStyle { get; set; } = MarkupStringSanitized.DefaultSanitizeInlineStyle;

    /// <summary>
    /// Gets or sets the function used to sanitize HTML code values before rendering.
    /// See <see cref="MarkupStringSanitized.DefaultSanitizeHtml"/> for more details.
    /// </summary>
    public Func<string, string> DefaultSanitizeHtml { get; set; } = MarkupStringSanitized.DefaultSanitizeHtml;
}
