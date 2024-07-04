using System.ComponentModel.DataAnnotations;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The skin tones for emojis 
/// </summary>
public enum EmojiSkintone
{
    /// <summary>
    /// Default
    /// </summary>
    [Display(Name = "Default")]
    Default,

    /// <summary>
    /// Light
    /// </summary>  
    [Display(Name = "Light")]
    Light,

    /// <summary>
    /// Medium Light
    /// </summary>
    [Display(Name = "Medium-Light")]
    MediumLight,

    /// <summary>
    /// Medium
    /// </summary>  
    [Display(Name = "Medium")]
    Medium,

    /// <summary>
    /// Medium Dark
    /// </summary>  
    [Display(Name = "Medium-Dark")]
    MediumDark,

    /// <summary>
    /// Dark
    /// </summary>  
    [Display(Name = "Dark")]
    Dark
}
