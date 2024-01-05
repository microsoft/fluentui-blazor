using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The visual appearance of a card
/// </summary>
public enum CardAppearance
{
    /// <summary>
    /// The card will have a shadow, border and background color.
    /// </summary>
    [Description("filled")]
    Filled,

    /// <summary>
    /// This appearance is similar to <see cref="Filled"/>, but the background color will be a little darker.
    /// </summary>
    [Description("filled-alternative")]
    FilledAlternative,

    /// <summary>
    /// This appearance is similar to <see cref="Filled"/>, but the background color will be transparent and no shadow applied.
    /// </summary>
    [Description("outline")]
    Outline,

    /// <summary>
    /// This appearance is similar to <see cref="FilledAlternative"/>, but no border is applied.
    /// </summary>
    [Description("subtle")]
    Subtle
}