using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Indicates how string values are compared.
/// </summary>
public enum DataFilterCaseSensitivity
{
    /// <summary>
    /// Text is compared using <see cref="StringComparison.Ordinal"/>.
    /// </summary>
    /// <remarks>
    /// When using this value, casing of text matters.
    /// </remarks>
    Default,

    /// <summary>
    /// Text is compared using <see cref="StringComparison.OrdinalIgnoreCase"/>.
    /// </summary>
    /// <remarks>
    /// When using this value, casing of text does not matter.
    /// </remarks>
    [Description("Case Insensitive")]
    CaseInsensitive,

    /// <summary>
    /// Excludes any <see cref="StringComparison" /> value for text comparisons.
    /// </summary>
    /// <remarks>
    /// This is typically used for Entity Framework expressions, which do not support string comparisons.
    /// </remarks>
    Ignore
}
