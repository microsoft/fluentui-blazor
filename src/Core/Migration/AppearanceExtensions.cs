// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components.Migration;

/// <summary />
public static class AppearanceExtensions
{
    /// <summary>
    /// Converts an obsolete <see cref="Appearance"/> enum value to a <see cref="ButtonAppearance"/>.
    /// </summary>
    /// <param name="appearance"></param>
    /// <returns></returns>
    public static ButtonAppearance? ToButtonAppearance(this Appearance appearance)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        return appearance switch
        {
            Appearance.Neutral => ButtonAppearance.Default,
            Appearance.Accent => ButtonAppearance.Primary,
            Appearance.Hypertext => ButtonAppearance.Default,
            Appearance.Lightweight => ButtonAppearance.Transparent,
            Appearance.Outline => ButtonAppearance.Outline,
            Appearance.Stealth => ButtonAppearance.Subtle,
            Appearance.Filled => ButtonAppearance.Default,
            _ => ButtonAppearance.Default
        };
#pragma warning restore CS0618 // Type or member is obsolete
    }

    /// <summary>
    /// Converts an obsolete <see cref="Appearance"/> enum value to a <see cref="BadgeAppearance"/>.
    /// </summary>
    /// <param name="appearance"></param>
    /// <returns></returns>
    public static BadgeAppearance ToBadgeAppearance(this Appearance appearance)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        return appearance switch
        {
            Appearance.Neutral => BadgeAppearance.Filled,
            Appearance.Accent => BadgeAppearance.Filled,
            Appearance.Lightweight => BadgeAppearance.Ghost,

            _ => BadgeAppearance.Filled
        };
#pragma warning restore CS0618 // Type or member is obsolete
    }
}
