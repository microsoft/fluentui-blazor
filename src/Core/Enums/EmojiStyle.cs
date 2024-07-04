using System.ComponentModel.DataAnnotations;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The variant options for emojis
/// </summary>
public enum EmojiStyle
{
    /// <summary>
    /// Color
    /// </summary>
    [Display(Name = "Color")]
    Color,

    /// <summary>
    /// Flat
    /// </summary>  
    [Display(Name = "Flat")]
    Flat,

    /// <summary>
    /// High Contrast
    /// </summary>  
    [Display(Name = "High-Contrast")]
    HighContrast,

}
