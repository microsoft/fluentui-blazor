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
    [InlineData(Appearance.Neutral, ButtonAppearance.Default)]
    [InlineData(Appearance.Accent, ButtonAppearance.Primary)]
    [InlineData(Appearance.Hypertext, ButtonAppearance.Default)]
    [InlineData(Appearance.Lightweight, ButtonAppearance.Transparent)]
    [InlineData(Appearance.Outline, ButtonAppearance.Outline)]
    [InlineData(Appearance.Stealth, ButtonAppearance.Default)]
    [InlineData(Appearance.Filled, ButtonAppearance.Default)]
    [InlineData(Appearance.Filled | Appearance.Accent, ButtonAppearance.Default)]
    public void Appearance_ToButtonAppearance(Appearance appearance, ButtonAppearance expected)
    {
        Assert.Equal(expected, appearance.ToButtonAppearance());
    }

#pragma warning restore CS0618 // Type or member is obsolete
}
