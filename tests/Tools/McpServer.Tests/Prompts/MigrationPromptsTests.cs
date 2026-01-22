// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.Extensions.AI;
using Microsoft.FluentUI.AspNetCore.McpServer.Prompts;
using Microsoft.FluentUI.AspNetCore.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Prompts;

/// <summary>
/// Tests for the <see cref="MigrationPrompts"/> class.
/// </summary>
public class MigrationPromptsTests
{
    [Fact]
    public void MigrateToV5_WithDefaultParameters_ReturnsValidChatMessage()
    {
        // Arrange
        var service = new DocumentationService();
        var prompts = new MigrationPrompts(service);

        // Act
        var result = prompts.MigrateToV5();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ChatRole.User, result.Role);
        Assert.NotNull(result.Text);
    }

    [Fact]
    public void MigrateToV5_WithComponentName_IncludesComponent()
    {
        // Arrange
        var service = new DocumentationService();
        var prompts = new MigrationPrompts(service);

        // Act
        var result = prompts.MigrateToV5("DataGrid");

        // Assert
        Assert.Contains("DataGrid", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void MigrateToV5_IncludesMigrationHeader()
    {
        // Arrange
        var service = new DocumentationService();
        var prompts = new MigrationPrompts(service);

        // Act
        var result = prompts.MigrateToV5();

        // Assert
        Assert.Contains("Migrate", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void MigrateToV5_IncludesVersionInfo()
    {
        // Arrange
        var service = new DocumentationService();
        var prompts = new MigrationPrompts(service);

        // Act
        var result = prompts.MigrateToV5();

        // Assert
        Assert.Contains("v5", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void MigrateToV5_WithBreakingChanges_IncludesBreakingChangesSection()
    {
        // Arrange
        var service = new DocumentationService();
        var prompts = new MigrationPrompts(service);

        // Act
        var result = prompts.MigrateToV5(includeBreakingChanges: true);

        // Assert
        Assert.Contains("breaking", result.Text, StringComparison.OrdinalIgnoreCase);
    }
}
