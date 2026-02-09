// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Services;

/// <summary>
/// Tests for the <see cref="ComponentDocumentationService"/> class.
/// </summary>
public class ComponentDocumentationServiceTests
{
    #region Constructor Tests

    [Fact]
    public void Constructor_ShouldNotThrow()
    {
        // Act
        var exception = Record.Exception(() => new ComponentDocumentationService());

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Constructor_ShouldLoadResources()
    {
        // Act
        var service = new ComponentDocumentationService();

        // Assert
        var components = service.GetAvailableComponents();
        Assert.NotNull(components);
    }

    #endregion

    #region GetComponentDocumentation Tests

    [Fact]
    public void GetComponentDocumentation_WithNonExistentName_ShouldReturnNull()
    {
        // Arrange
        var service = new ComponentDocumentationService();

        // Act
        var result = service.GetComponentDocumentation("NonExistentComponent");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetComponentDocumentation_WithExistingComponent_ShouldReturnContent()
    {
        // Arrange
        var service = new ComponentDocumentationService();
        var components = service.GetAvailableComponents();

        if (components.Count == 0)
        {
            return; // No embedded docs available in test context
        }

        // Act
        var result = service.GetComponentDocumentation(components[0]);

        // Assert
        Assert.NotNull(result);
        Assert.False(string.IsNullOrWhiteSpace(result));
    }

    [Fact]
    public void GetComponentDocumentation_ShouldNotContainApiDirectives()
    {
        // Arrange
        var service = new ComponentDocumentationService();
        var components = service.GetAvailableComponents();

        // Act & Assert
        foreach (var componentName in components)
        {
            var result = service.GetComponentDocumentation(componentName);
            if (result != null)
            {
                Assert.DoesNotContain("{{ API ", result, StringComparison.OrdinalIgnoreCase);
            }
        }
    }

    [Fact]
    public void GetComponentDocumentation_ShouldNotContainFrontMatter()
    {
        // Arrange
        var service = new ComponentDocumentationService();
        var components = service.GetAvailableComponents();

        // Act & Assert
        foreach (var componentName in components)
        {
            var result = service.GetComponentDocumentation(componentName);
            if (result != null)
            {
                Assert.DoesNotMatch(@"^---\s*\n", result);
            }
        }
    }

    #endregion

    #region SearchDocumentation Tests

    [Fact]
    public void SearchDocumentation_WithEmptyTerm_ShouldReturnEmpty()
    {
        // Arrange
        var service = new ComponentDocumentationService();

        // Act
        var result = service.SearchDocumentation("");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void SearchDocumentation_WithWhitespaceTerm_ShouldReturnEmpty()
    {
        // Arrange
        var service = new ComponentDocumentationService();

        // Act
        var result = service.SearchDocumentation("   ");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void SearchDocumentation_WithNonMatchingTerm_ShouldReturnEmpty()
    {
        // Arrange
        var service = new ComponentDocumentationService();

        // Act
        var result = service.SearchDocumentation("XyzNonExistentTerm98765");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void SearchDocumentation_ShouldReturnSortedResults()
    {
        // Arrange
        var service = new ComponentDocumentationService();
        var components = service.GetAvailableComponents();

        if (components.Count == 0)
        {
            return; // No embedded docs available
        }

        // Use a common term that should match multiple docs
        var result = service.SearchDocumentation("component");

        // Assert - results should be sorted
        var sorted = result.OrderBy(r => r, StringComparer.OrdinalIgnoreCase).ToList();
        Assert.Equal(sorted, result);
    }

    #endregion

    #region GetAvailableComponents Tests

    [Fact]
    public void GetAvailableComponents_ShouldReturnSortedList()
    {
        // Arrange
        var service = new ComponentDocumentationService();

        // Act
        var result = service.GetAvailableComponents();

        // Assert
        Assert.NotNull(result);
        var sorted = result.OrderBy(r => r, StringComparer.OrdinalIgnoreCase).ToList();
        Assert.Equal(sorted, result);
    }

    #endregion

    #region ResolveDirectives Tests

    [Fact]
    public void ResolveDirectives_WithNoDirectives_ShouldReturnSameContent()
    {
        // Arrange
        var service = new ComponentDocumentationService();
        var content = "# My Component\n\nThis is a description.";

        // Act
        var result = service.ResolveDirectives(content);

        // Assert
        Assert.Equal(content, result);
    }

    [Fact]
    public void ResolveDirectives_WithApiDirective_ShouldStripIt()
    {
        // Arrange
        var service = new ComponentDocumentationService();
        var content = "# My Component\n\n{{ API Type=FluentButton }}\n\nSome text.";

        // Act
        var result = service.ResolveDirectives(content);

        // Assert
        Assert.DoesNotContain("{{ API", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Some text", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ResolveDirectives_WithApiPropertiesDirective_ShouldStripIt()
    {
        // Arrange
        var service = new ComponentDocumentationService();
        var content = "# Options\n\n{{ API Type=DialogOptions Properties=All }}\n\nEnd.";

        // Act
        var result = service.ResolveDirectives(content);

        // Assert
        Assert.DoesNotContain("{{ API", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("End", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ResolveDirectives_WithUnknownExample_ShouldLeaveComment()
    {
        // Arrange
        var service = new ComponentDocumentationService();
        var content = "# Test\n\n{{ UnknownExampleXyz123 }}";

        // Act
        var result = service.ResolveDirectives(content);

        // Assert
        Assert.Contains("<!-- Example 'UnknownExampleXyz123' not found -->", result, StringComparison.OrdinalIgnoreCase);
    }

    #endregion
}
