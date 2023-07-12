﻿using System.ComponentModel;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// The skin tones for emojis 
/// </summary>
public enum EmojiSkintone
{
    /// <summary>
    /// Default
    /// </summary>
    [Description("Default")]
    Default,

    /// <summary>
    /// Light
    /// </summary>  
    [Description("Light")]
    Light,

    /// <summary>
    /// Medium Light
    /// </summary>
    [Description("Medium-Light")]
    MediumLight,

    /// <summary>
    /// Medium
    /// </summary>  
    [Description("Medium")]
    Medium,

    /// <summary>
    /// Medium Dark
    /// </summary>  
    [Description("Medium-Dark")]
    MediumDark,

    /// <summary>
    /// Dark
    /// </summary>  
    [Description("Dark")]
    Dark
}