// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.McpServer.Models;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Models;

/// <summary>
/// Tests for the <see cref="GeneralInfo"/> class.
/// </summary>
public class GeneralInfoTests
{
    [Fact]
    public void DefaultValues_ShouldBeEmpty()
    {
        // Act
        var info = new GeneralInfo();

        // Assert
        Assert.Equal(string.Empty, info.Title);
        Assert.Equal(string.Empty, info.Category);
        Assert.Equal(string.Empty, info.Route);
        Assert.Equal(string.Empty, info.Icon);
        Assert.Equal(string.Empty, info.FileName);
        Assert.Equal(string.Empty, info.Content);
        Assert.Equal(string.Empty, info.Summary);
        Assert.Equal(0, info.Order);
        Assert.False(info.Hidden);
    }

    [Fact]
    public void Properties_ShouldBeSettable()
    {
        // Arrange
        var info = new GeneralInfo
        {
            // Act
            Title = "Test Title",
            Category = "Test Category",
            Route = "/test-route",
            Icon = "TestIcon",
            FileName = "TestFile",
            Content = "Test Content",
            Summary = "Test Summary",
            Order = 100,
            Hidden = true,
        };

        // Assert
        Assert.Equal("Test Title", info.Title);
        Assert.Equal("Test Category", info.Category);
        Assert.Equal("/test-route", info.Route);
        Assert.Equal("TestIcon", info.Icon);
        Assert.Equal("TestFile", info.FileName);
        Assert.Equal("Test Content", info.Content);
        Assert.Equal("Test Summary", info.Summary);
        Assert.Equal(100, info.Order);
        Assert.True(info.Hidden);
    }
}
