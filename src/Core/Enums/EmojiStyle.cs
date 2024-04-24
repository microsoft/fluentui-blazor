using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The variant options for emojis
/// </summary>
public enum EmojiStyle
{
    /// <summary>
    /// Color
    /// </summary>
    [Description("Color")]
    Color,

    /// <summary>
    /// Flat
    /// </summary>  
    [Description("Flat")]
    Flat,

    /// <summary>
    /// High Contrast
    /// </summary>  
    [Description("High-Contrast")]
    HighContrast,

}
