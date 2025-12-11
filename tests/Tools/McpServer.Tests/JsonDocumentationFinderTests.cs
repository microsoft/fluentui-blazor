// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tests;

/// <summary>
/// Tests for <see cref="JsonDocumentationFinder"/>.
/// </summary>
public class JsonDocumentationFinderTests
{
    [Fact]
    public void Find_ReturnsPathOrNull()
    {
        // Act
        var result = JsonDocumentationFinder.Find();

        // Assert
        // Should either return a valid path or null (for embedded resource)
        if (result != null)
        {
            Assert.True(File.Exists(result), $"Returned path should exist: {result}");
        }
    }

    [Fact]
    public void Find_WhenFileExists_ReturnsFullPath()
    {
        // Act
        var result = JsonDocumentationFinder.Find();

        // Assert
        if (result != null)
        {
            Assert.True(Path.IsPathRooted(result), "Should return a full path");
        }
    }

    [Fact]
    public void Find_ReturnsConsistentResult()
    {
        // Act
        var result1 = JsonDocumentationFinder.Find();
        var result2 = JsonDocumentationFinder.Find();

        // Assert
        Assert.Equal(result1, result2);
    }

    [Fact]
    public void Find_WhenFileExists_ContainsExpectedFileName()
    {
        // Act
        var result = JsonDocumentationFinder.Find();

        // Assert
        if (result != null)
        {
            Assert.Contains("FluentUIComponentsDocumentation.json", result);
        }
    }
}
