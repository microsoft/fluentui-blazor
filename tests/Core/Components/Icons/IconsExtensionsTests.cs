// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Bunit;
using Microsoft.FluentUI.AspNetCore.Components;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Icons;

public class IconsExtensionsTests : Bunit.TestContext
{
    private static IconInfo CreateMissingIcon()
    {
        // Construct an IconInfo that is very unlikely to exist in the icon assemblies.
        // Rely on public enums for Variant and Size so the extension methods run their logic.
        return new IconInfo
        {
            Name = "Nonexistent_Icon_For_UnitTest",
            Variant = IconVariant.Regular,
            Size = IconSize.Size16
        };
    }

    [Fact]
    public void GetInstance_ThrowsArgumentException_ForMissingIcon_ByDefault()
    {
        var icon = CreateMissingIcon();

        var ex = Assert.Throws<ArgumentException>(() => icon.GetInstance());
        Assert.Contains("Icons.", ex.Message, StringComparison.InvariantCultureIgnoreCase);
    }

    [Fact]
    public void GetInstance_ReturnsNull_WhenThrowOnErrorFalse()
    {
        var icon = CreateMissingIcon();

        var result = icon.GetInstance(throwOnError: false);

        Assert.Null(result);
    }

    [Fact]
    public void TryGetInstance_ReturnsFalse_AndResultNull_ForMissingIcon()
    {
        var icon = CreateMissingIcon();

        var success = icon.TryGetInstance(out var result);

        Assert.False(success);
        Assert.Null(result);
    }

    [Fact]
    public void GetAllIcons_DoesNotThrow_AndReturnsEnumerable()
    {
        IEnumerable<IconInfo> all = null!;
        Exception? thrown = null;

        try
        {
            all = IconsExtensions.GetAllIcons();
        }
        catch (Exception ex)
        {
            thrown = ex;
        }

        Assert.Null(thrown);
        Assert.NotNull(all);
        // Ensure it's an enumerable (deferred) and can be iterated without throwing.
        foreach (var item in all)
        {
            Assert.NotNull(item);
        }
    }

    [Fact]
    public void AllIcons_Property_ReturnsEnumerable_AndMatchesGetAllIcons()
    {
        var fromMethod = IconsExtensions.GetAllIcons();
        var fromProperty = IconsExtensions.AllIcons;

        Assert.NotNull(fromProperty);
        Assert.NotNull(fromMethod);

        // Compare counts without forcing multiple enumerations in pathological cases.
        var listMethod = new List<IconInfo>(fromMethod);
        var listProp = new List<IconInfo>(fromProperty);

        Assert.Equal(listMethod.Count, listProp.Count);
    }
}
