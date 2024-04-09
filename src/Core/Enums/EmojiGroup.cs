using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The collection of groups emojis are placed in
/// </summary>
public enum EmojiGroup
{
    /// <summary>
    /// Smileys and Emotion
    /// </summary>
    [Description("Smileys & Emotion")]
    Smileys_Emotion,

    /// <summary>
    /// People and Body
    /// </summary>
    [Description("People & Body")]
    People_Body,

    /// <summary>
    /// Animals and Nature
    /// </summary>  
    [Description("Animals & Nature")]
    Animals_Nature,

    /// <summary>
    /// Food and Drink
    /// </summary>
    [Description("Food & Drink")]
    Food_Drink,

    /// <summary>
    /// Travel and Places
    /// </summary>
    [Description("Travel & Places")]
    Travel_Places,

    /// <summary>
    /// Symbols
    /// </summary>  
    [Description("Symbols")]
    Symbols,

    /// <summary>
    /// Objects
    /// </summary>  
    [Description("Objects")]
    Objects,

    /// <summary>
    /// Activities
    /// </summary>  
    [Description("Activities")]
    Activities,

    /// <summary>
    /// Flags
    /// </summary>  
    [Description("Flags")]
    Flags
}
