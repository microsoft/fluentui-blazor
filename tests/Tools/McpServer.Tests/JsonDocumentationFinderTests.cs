// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests;

/// <summary>
/// Tests for the <see cref="JsonDocumentationFinder"/> class.
/// </summary>
public class JsonDocumentationFinderTests
{
    [Fact]
    public void Find_WhenJsonFileExists_ShouldReturnPath()
    {
        // Arrange
        var testJsonPath = Path.Combine(AppContext.BaseDirectory, "FluentUIComponentsDocumentation.json");

        // Only run this test if the file exists (copied during build)
        if (File.Exists(testJsonPath))
        {
            // Act
            var result = JsonDocumentationFinder.Find();

            // Assert
            Assert.NotNull(result);
            Assert.True(File.Exists(result));
        }
    }

    [Fact]
    public void Find_ShouldReturnNullOrValidPath()
    {
        // Act
        var result = JsonDocumentationFinder.Find();

        // Assert
        // Either null (will use embedded resource) or a valid path
        if (result != null)
        {
            Assert.True(File.Exists(result));
        }
    }

    [Fact]
    public void Find_WhenCalled_ShouldNotThrow()
    {
        // Act
        var exception = Record.Exception(() => JsonDocumentationFinder.Find());

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Find_MultipleCalls_ShouldReturnConsistentResult()
    {
        // Act
        var result1 = JsonDocumentationFinder.Find();
        var result2 = JsonDocumentationFinder.Find();

        // Assert
        Assert.Equal(result1, result2);
    }
}
