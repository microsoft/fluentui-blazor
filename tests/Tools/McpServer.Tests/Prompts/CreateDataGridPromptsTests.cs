// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.Extensions.AI;
using Microsoft.FluentUI.AspNetCore.McpServer.Prompts;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Prompts;

/// <summary>
/// Tests for the <see cref="CreateDataGridPrompts"/> class.
/// </summary>
public class CreateDataGridPromptsTests
{
    [Fact]
    public void CreateDataGrid_WithDefaultParameters_ReturnsValidChatMessage()
    {
        // Act
        var result = CreateDataGridPrompts.CreateDataGrid("Product list");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ChatRole.User, result.Role);
        Assert.NotNull(result.Text);
        Assert.Contains("Product list", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void CreateDataGrid_IncludesDataGridComponent()
    {
        // Act
        var result = CreateDataGridPrompts.CreateDataGrid("Order list");

        // Assert
        Assert.Contains("FluentDataGrid", result.Text, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("sorting,pagination")]
    [InlineData("sorting,selection")]
    [InlineData("virtualization")]
    public void CreateDataGrid_WithFeatures_IncludesFeatures(string features)
    {
        // Act
        var result = CreateDataGridPrompts.CreateDataGrid("Item list", features);

        // Assert
        foreach (var feature in features.Split(','))
        {
            Assert.Contains(feature, result.Text, StringComparison.OrdinalIgnoreCase);
        }
    }

    [Theory]
    [InlineData("grid")]
    [InlineData("table")]
    public void CreateDataGrid_WithDisplayMode_IncludesDisplayMode(string displayMode)
    {
        // Act
        var result = CreateDataGridPrompts.CreateDataGrid("Data list", displayMode: displayMode);

        // Assert
        Assert.Contains(displayMode, result.Text, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void CreateDataGrid_IncludesPropertyColumn()
    {
        // Act
        var result = CreateDataGridPrompts.CreateDataGrid("Entity list");

        // Assert
        Assert.Contains("PropertyColumn", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void CreateDataGrid_IncludesTemplateColumn()
    {
        // Act
        var result = CreateDataGridPrompts.CreateDataGrid("Data list");

        // Assert
        Assert.Contains("TemplateColumn", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void CreateDataGrid_IncludesHeader()
    {
        // Act
        var result = CreateDataGridPrompts.CreateDataGrid("Record list");

        // Assert
        Assert.Contains("Create a Fluent UI Blazor DataGrid", result.Text, StringComparison.Ordinal);
    }
}
