// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components.Migration;

/// <summary>
/// 
/// </summary>
public static class AppearanceExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="appearance"></param>
    /// <returns></returns>
    public static ButtonAppearance ToButtonAppearance(this Appearance appearance)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        return appearance switch
        {
            Appearance.Neutral => ButtonAppearance.Default,
            Appearance.Accent => ButtonAppearance.Primary,
            Appearance.Hypertext => ButtonAppearance.Default,
            Appearance.Lightweight => ButtonAppearance.Transparent,
            Appearance.Outline => ButtonAppearance.Outline,
            Appearance.Stealth => ButtonAppearance.Default,
            Appearance.Filled => ButtonAppearance.Default,
            _ => ButtonAppearance.Default
        };
#pragma warning restore CS0618 // Type or member is obsolete
    }
}
