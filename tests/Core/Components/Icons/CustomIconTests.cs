// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Icons;

public class CustomIconTests : Bunit.TestContext
{
    [Fact]
    public void DefaultConstructor_SetsExpectedDefaults()
    {
        // Arrange & Act
        var icon = new CustomIcon();

        // Assert
        Assert.Equal(string.Empty, icon.Name);
        Assert.Equal(IconVariant.Regular, icon.Variant);
        Assert.Equal(IconSize.Size24, icon.Size);
        Assert.Equal(string.Empty, icon.Content);
    }

    [Fact]
    public void Constructor_WithIcon_CopiesValues()
    {
        // Arrange
        var source = new Icon("my-icon", IconVariant.Filled, IconSize.Size16, "<svg></svg>");

        // Act
        var icon = new CustomIcon(source);

        // Assert
        Assert.Equal("my-icon", icon.Name);
        Assert.Equal(IconVariant.Filled, icon.Variant);
        Assert.Equal(IconSize.Size16, icon.Size);
        Assert.Equal("<svg></svg>", icon.Content);
    }
}
