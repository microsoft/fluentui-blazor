// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.McpServer.Models;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Models;

/// <summary>
/// Tests for the <see cref="ComponentInfo"/> record.
/// </summary>
public class ComponentInfoTests
{
    [Fact]
    public void ComponentInfo_WithRequiredProperties_ShouldBeCreatedSuccessfully()
    {
        // Arrange & Act
        var componentInfo = new ComponentInfo
        {
            Name = "FluentButton",
            FullName = "Microsoft.FluentUI.AspNetCore.Components.FluentButton"
        };

        // Assert
        Assert.Equal("FluentButton", componentInfo.Name);
        Assert.Equal("Microsoft.FluentUI.AspNetCore.Components.FluentButton", componentInfo.FullName);
    }

    [Fact]
    public void ComponentInfo_DefaultValues_ShouldBeCorrect()
    {
        // Arrange & Act
        var componentInfo = new ComponentInfo
        {
            Name = "FluentButton",
            FullName = "Microsoft.FluentUI.AspNetCore.Components.FluentButton"
        };

        // Assert
        Assert.Equal(string.Empty, componentInfo.Summary);
        Assert.Equal(string.Empty, componentInfo.Category);
        Assert.False(componentInfo.IsGeneric);
        Assert.Null(componentInfo.BaseClass);
    }

    [Fact]
    public void ComponentInfo_WithAllProperties_ShouldBeCreatedSuccessfully()
    {
        // Arrange & Act
        var componentInfo = new ComponentInfo
        {
            Name = "FluentDataGrid",
            FullName = "Microsoft.FluentUI.AspNetCore.Components.FluentDataGrid`1",
            Summary = "A grid component for displaying data",
            Category = "DataGrid",
            IsGeneric = true,
            BaseClass = "FluentComponentBase"
        };

        // Assert
        Assert.Equal("FluentDataGrid", componentInfo.Name);
        Assert.Equal("Microsoft.FluentUI.AspNetCore.Components.FluentDataGrid`1", componentInfo.FullName);
        Assert.Equal("A grid component for displaying data", componentInfo.Summary);
        Assert.Equal("DataGrid", componentInfo.Category);
        Assert.True(componentInfo.IsGeneric);
        Assert.Equal("FluentComponentBase", componentInfo.BaseClass);
    }

    [Fact]
    public void ComponentInfo_RecordEquality_ShouldWorkCorrectly()
    {
        // Arrange
        var componentInfo1 = new ComponentInfo
        {
            Name = "FluentButton",
            FullName = "Microsoft.FluentUI.AspNetCore.Components.FluentButton",
            Category = "Button"
        };

        var componentInfo2 = new ComponentInfo
        {
            Name = "FluentButton",
            FullName = "Microsoft.FluentUI.AspNetCore.Components.FluentButton",
            Category = "Button"
        };

        // Act & Assert
        Assert.Equal(componentInfo1, componentInfo2);
        Assert.True(componentInfo1 == componentInfo2);
    }

    [Fact]
    public void ComponentInfo_RecordInequality_ShouldWorkCorrectly()
    {
        // Arrange
        var componentInfo1 = new ComponentInfo
        {
            Name = "FluentButton",
            FullName = "Microsoft.FluentUI.AspNetCore.Components.FluentButton"
        };

        var componentInfo2 = new ComponentInfo
        {
            Name = "FluentTextField",
            FullName = "Microsoft.FluentUI.AspNetCore.Components.FluentTextField"
        };

        // Act & Assert
        Assert.NotEqual(componentInfo1, componentInfo2);
        Assert.True(componentInfo1 != componentInfo2);
    }

    [Fact]
    public void ComponentInfo_WithInit_ShouldAllowPropertyModification()
    {
        // Arrange
        var original = new ComponentInfo
        {
            Name = "FluentButton",
            FullName = "Microsoft.FluentUI.AspNetCore.Components.FluentButton",
            Summary = "Original summary"
        };

        // Act
        var modified = original with { Summary = "Modified summary" };

        // Assert
        Assert.Equal("Modified summary", modified.Summary);
        Assert.Equal("FluentButton", modified.Name);
        Assert.Equal("Original summary", original.Summary);
    }
}
