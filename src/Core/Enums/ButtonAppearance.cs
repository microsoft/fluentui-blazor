// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The visual appearance of the FluentButton.
/// </summary>
public enum ButtonAppearance
{
    /// <summary>
    /// The button appears with the default style
    /// </summary>
    Default,

    /// <summary>
    /// Emphasizes the button as a primary action.
    /// </summary>
    Primary,

    /// <summary>
    /// Removes background styling.
    /// </summary>
    Outline,

    /// <summary>
    /// Minimizes emphasis to blend into the background until hovered or focused
    /// </summary>
    Subtle,

    /// <summary>
    /// Removes background and border styling.
    /// </summary>
    Transparent,

    /*
       #region Obsoletes

       /// <summary />
       [Obsolete("This value is obsolete. Use the Default value instead.")]
       Neutral,

       /// <summary />
       [Obsolete("This value is obsolete. Use the Primary value instead.")]
       Accent,

       /// <summary />
       [Obsolete("This value is obsolete. Use the Transparent value instead.")]
       Lightweight,

       /// <summary />
       [Obsolete("This value is obsolete. Use the Default value instead.")]
       Stealth,

       /// <summary />
       [Obsolete("This value is obsolete. Use the Default value instead.")]
       Filled

       #endregion
    */
}

