// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace Microsoft.FluentUI.AspNetCore.Components;

internal static class FluentButtonExtensions
{
    public static string? ToAttributeValue(this ButtonAppearance appearance)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        return appearance switch
        {
            // Obsolete cases
            ButtonAppearance.Neutral => EnumExtensions.ToAttributeValue(ButtonAppearance.Default),
            ButtonAppearance.Accent => EnumExtensions.ToAttributeValue(ButtonAppearance.Primary),
            ButtonAppearance.Lightweight => EnumExtensions.ToAttributeValue(ButtonAppearance.Transparent),
            ButtonAppearance.Stealth => EnumExtensions.ToAttributeValue(ButtonAppearance.Default),
            ButtonAppearance.Filled => EnumExtensions.ToAttributeValue(ButtonAppearance.Default),

            // Default cases
            _ => EnumExtensions.ToAttributeValue(appearance),
        };
#pragma warning restore CS0618 // Type or member is obsolete
    }
}
