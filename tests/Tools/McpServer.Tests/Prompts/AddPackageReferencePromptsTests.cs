// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.Extensions.AI;
using Microsoft.FluentUI.AspNetCore.McpServer.Prompts;
using Microsoft.FluentUI.AspNetCore.McpServer.Tools;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Prompts;

/// <summary>
/// Tests for the <see cref="AddPackageReferencePrompts"/> class.
/// </summary>
public class AddPackageReferencePromptsTests
{
    private const string TestProjectPath = "src/MyApp/MyApp.csproj";

    [Fact]
    public void AddPackageReference_WithDefaultParameters_ReturnsValidChatMessage()
    {
        // Act
        var result = AddPackageReferencePrompts.AddPackageReference(TestProjectPath);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ChatRole.User, result.Role);
        Assert.NotNull(result.Text);
        Assert.Contains("Add Fluent UI Blazor Package References", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void AddPackageReference_IncludesProjectPath()
    {
        // Act
        var result = AddPackageReferencePrompts.AddPackageReference(TestProjectPath);

        // Assert
        Assert.Contains(TestProjectPath, result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void AddPackageReference_IncludesMcpServerVersion()
    {
        // Arrange
        var expectedVersion = VersionTools.GetMcpSemanticVersion();

        // Act
        var result = AddPackageReferencePrompts.AddPackageReference(TestProjectPath);

        // Assert
        Assert.Contains(expectedVersion, result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void AddPackageReference_AlwaysIncludesComponentsPackage()
    {
        // Act
        var result = AddPackageReferencePrompts.AddPackageReference(TestProjectPath, includeIcons: false, includeEmoji: false);

        // Assert
        Assert.Contains("Microsoft.FluentUI.AspNetCore.Components", result.Text, StringComparison.Ordinal);
        Assert.Contains("Components (required)", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void AddPackageReference_WithIconsEnabled_IncludesIconsPackage()
    {
        // Act
        var result = AddPackageReferencePrompts.AddPackageReference(TestProjectPath, includeIcons: true);

        // Assert
        Assert.Contains("Microsoft.FluentUI.AspNetCore.Components.Icons", result.Text, StringComparison.Ordinal);
        Assert.Contains("Icons (recommended)", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void AddPackageReference_WithIconsDisabled_ExcludesIconsPackage()
    {
        // Act
        var result = AddPackageReferencePrompts.AddPackageReference(TestProjectPath, includeIcons: false);

        // Assert
        Assert.DoesNotContain("Microsoft.FluentUI.AspNetCore.Components.Icons", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void AddPackageReference_WithEmojiEnabled_IncludesEmojiPackage()
    {
        // Act
        var result = AddPackageReferencePrompts.AddPackageReference(TestProjectPath, includeEmoji: true);

        // Assert
        Assert.Contains("Microsoft.FluentUI.AspNetCore.Components.Emoji", result.Text, StringComparison.Ordinal);
        Assert.Contains("Emoji (optional)", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void AddPackageReference_WithEmojiDisabled_ExcludesEmojiPackage()
    {
        // Act
        var result = AddPackageReferencePrompts.AddPackageReference(TestProjectPath, includeEmoji: false);

        // Assert
        Assert.DoesNotContain("Microsoft.FluentUI.AspNetCore.Components.Emoji", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void AddPackageReference_IncludesDotnetAddCommand()
    {
        // Arrange
        var expectedVersion = VersionTools.GetMcpSemanticVersion();

        // Act
        var result = AddPackageReferencePrompts.AddPackageReference(TestProjectPath);

        // Assert
        Assert.Contains(
            $"dotnet add \"{TestProjectPath}\" package Microsoft.FluentUI.AspNetCore.Components --version {expectedVersion}",
            result.Text,
            StringComparison.Ordinal);
    }

    [Fact]
    public void AddPackageReference_IncludesPackageReferenceXml()
    {
        // Arrange
        var expectedVersion = VersionTools.GetMcpSemanticVersion();

        // Act
        var result = AddPackageReferencePrompts.AddPackageReference(TestProjectPath);

        // Assert
        Assert.Contains(
            $"<PackageReference Include=\"Microsoft.FluentUI.AspNetCore.Components\" Version=\"{expectedVersion}\" />",
            result.Text,
            StringComparison.Ordinal);
    }

    [Fact]
    public void AddPackageReference_IncludesVerificationStep()
    {
        // Act
        var result = AddPackageReferencePrompts.AddPackageReference(TestProjectPath);

        // Assert
        Assert.Contains("Verify the Project File", result.Text, StringComparison.Ordinal);
        Assert.Contains("Verify the Package References", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void AddPackageReference_MentionsCheckProjectVersionTool()
    {
        // Act
        var result = AddPackageReferencePrompts.AddPackageReference(TestProjectPath);

        // Assert
        Assert.Contains("CheckProjectVersion", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void AddPackageReference_WithAllPackages_IncludesAllThree()
    {
        // Act
        var result = AddPackageReferencePrompts.AddPackageReference(TestProjectPath, includeIcons: true, includeEmoji: true);

        // Assert
        Assert.Contains("Microsoft.FluentUI.AspNetCore.Components\" Version=", result.Text, StringComparison.Ordinal);
        Assert.Contains("Microsoft.FluentUI.AspNetCore.Components.Icons\" Version=", result.Text, StringComparison.Ordinal);
        Assert.Contains("Microsoft.FluentUI.AspNetCore.Components.Emoji\" Version=", result.Text, StringComparison.Ordinal);
    }
}
