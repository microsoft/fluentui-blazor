using System.ComponentModel.DataAnnotations;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The collection of groups emojis are placed in
/// </summary>
public enum EmojiGroup
{
    /// <summary>
    /// Smileys and Emotion
    /// </summary>
    [Display(Name = "Smileys & Emotion")]
    Smileys_Emotion,

    /// <summary>
    /// People and Body
    /// </summary>
    [Display(Name = "People & Body")]
    People_Body,

    /// <summary>
    /// Animals and Nature
    /// </summary>  
    [Display(Name = "Animals & Nature")]
    Animals_Nature,

    /// <summary>
    /// Food and Drink
    /// </summary>
    [Display(Name = "Food & Drink")]
    Food_Drink,

    /// <summary>
    /// Travel and Places
    /// </summary>
    [Display(Name = "Travel & Places")]
    Travel_Places,

    /// <summary>
    /// Symbols
    /// </summary>  
    [Display(Name = "Symbols")]
    Symbols,

    /// <summary>
    /// Objects
    /// </summary>  
    [Display(Name = "Objects")]
    Objects,

    /// <summary>
    /// Activities
    /// </summary>  
    [Display(Name = "Activities")]
    Activities,

    /// <summary>
    /// Flags
    /// </summary>  
    [Display(Name = "Flags")]
    Flags
}
