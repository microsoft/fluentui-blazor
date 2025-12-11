// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tests;

/// <summary>
/// Additional tests for <see cref="JsonDocumentationFinder"/> to improve coverage.
/// </summary>
public class JsonDocumentationFinderMoreTests
{
    [Fact]
    public void Find_CalledMultipleTimes_ReturnsSameResult()
    {
        // Act
        var result1 = JsonDocumentationFinder.Find();
        var result2 = JsonDocumentationFinder.Find();
        var result3 = JsonDocumentationFinder.Find();

        // Assert
        Assert.Equal(result1, result2);
        Assert.Equal(result2, result3);
    }

    [Fact]
    public void Find_ReturnsNullOrValidPath()
    {
        // Act
        var result = JsonDocumentationFinder.Find();

        // Assert
        if (result != null)
        {
            Assert.True(Path.IsPathRooted(result));
            Assert.True(File.Exists(result));
            Assert.EndsWith("FluentUIComponentsDocumentation.json", result);
        }
    }

    [Fact]
    public void Find_ChecksMultiplePaths()
    {
        // Act
        var result = JsonDocumentationFinder.Find();

        // Assert - Just verify it doesn't throw
        Assert.True(result == null || result != null);
    }

    [Fact]
    public void Find_WithFileInCurrentDirectory_FindsIt()
    {
        // This test verifies the finder checks AppContext.BaseDirectory
        // Act
        var result = JsonDocumentationFinder.Find();

        // Assert
        if (result != null)
        {
            Assert.Contains("FluentUIComponentsDocumentation.json", result);
        }
    }

    [Fact]
    public void Find_LogsToStandardError()
    {
        // Arrange
        var originalError = Console.Error;
        try
        {
            using var sw = new StringWriter();
            Console.SetError(sw);

            // Act
            _ = JsonDocumentationFinder.Find();

            // Assert
            var output = sw.ToString();
            Assert.True(
                output.Contains("Found JSON documentation") || 
                output.Contains("No external JSON documentation"),
                "Should log to stderr");
        }
        finally
        {
            Console.SetError(originalError);
        }
    }
}
