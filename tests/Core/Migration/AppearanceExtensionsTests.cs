// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.Migration;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Migration;

public partial class AppearanceExtensionsTests
{
#pragma warning disable CS0618 // Type or member is obsolete

    [Theory]
    [InlineData(Appearance.Neutral, ButtonAppearance.Outline)]
    [InlineData(Appearance.Accent, ButtonAppearance.Primary)]
    [InlineData(Appearance.Hypertext, ButtonAppearance.Outline)]
    [InlineData(Appearance.Lightweight, ButtonAppearance.Transparent)]
    [InlineData(Appearance.Outline, ButtonAppearance.Outline)]
    [InlineData(Appearance.Stealth, ButtonAppearance.Subtle)]
    [InlineData(Appearance.Filled, ButtonAppearance.Outline)]
    [InlineData(Appearance.Filled | Appearance.Accent, ButtonAppearance.Outline)]
    [InlineData((Appearance)999, ButtonAppearance.Outline)]
    public void Appearance_ToButtonAppearance(Appearance appearance, ButtonAppearance expected)
    {
        Assert.Equal(expected, appearance.ToButtonAppearance());
    }

    [Theory]
    [InlineData(Appearance.Neutral, BadgeAppearance.Filled)]
    [InlineData(Appearance.Accent, BadgeAppearance.Filled)]
    [InlineData(Appearance.Hypertext, BadgeAppearance.Filled)]
    [InlineData(Appearance.Lightweight, BadgeAppearance.Ghost)]
    [InlineData(Appearance.Outline, BadgeAppearance.Filled)]
    [InlineData(Appearance.Stealth, BadgeAppearance.Filled)]
    [InlineData(Appearance.Filled, BadgeAppearance.Filled)]
    [InlineData(Appearance.Filled | Appearance.Accent, BadgeAppearance.Filled)]
    [InlineData((Appearance)999, BadgeAppearance.Filled)]
    public void Appearance_ToBadgeAppearance(Appearance appearance, BadgeAppearance expected)
    {
        Assert.Equal(expected, appearance.ToBadgeAppearance());
    }

#pragma warning restore CS0618 // Type or member is obsolete
}
