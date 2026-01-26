// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.Extensions.AI;
using Microsoft.FluentUI.AspNetCore.McpServer.Prompts;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Prompts;

/// <summary>
/// Tests for the <see cref="AccessibilityPrompts"/> class.
/// </summary>
public class AccessibilityPromptsTests
{
    [Fact]
    public void ImplementAccessibility_WithComponent_ReturnsValidChatMessage()
    {
        // Act
        var result = AccessibilityPrompts.ImplementAccessibility("form");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ChatRole.User, result.Role);
        Assert.NotNull(result.Text);
        Assert.Contains("form", result.Text, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("form")]
    [InlineData("navigation")]
    [InlineData("data table")]
    [InlineData("dialog")]
    public void ImplementAccessibility_WithComponentOrFeature_IncludesComponent(string componentOrFeature)
    {
        // Act
        var result = AccessibilityPrompts.ImplementAccessibility(componentOrFeature);

        // Assert
        Assert.Contains(componentOrFeature, result.Text, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("A")]
    [InlineData("AA")]
    [InlineData("AAA")]
    public void ImplementAccessibility_WithWcagLevel_IncludesLevel(string wcagLevel)
    {
        // Act
        var result = AccessibilityPrompts.ImplementAccessibility("form", wcagLevel);

        // Assert
        Assert.Contains(wcagLevel, result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void ImplementAccessibility_IncludesHeader()
    {
        // Act
        var result = AccessibilityPrompts.ImplementAccessibility("navigation");

        // Assert
        Assert.Contains("Implement Accessibility in Fluent UI Blazor", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void ImplementAccessibility_IncludesAriaSection()
    {
        // Act
        var result = AccessibilityPrompts.ImplementAccessibility("form");

        // Assert
        Assert.Contains("ARIA", result.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void ImplementAccessibility_IncludesKeyboardSection()
    {
        // Act
        var result = AccessibilityPrompts.ImplementAccessibility("navigation");

        // Assert
        Assert.Contains("keyboard", result.Text, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ImplementAccessibility_IncludesScreenReaderSection()
    {
        // Act
        var result = AccessibilityPrompts.ImplementAccessibility("dialog");

        // Assert
        Assert.Contains("screen reader", result.Text, StringComparison.OrdinalIgnoreCase);
    }
}
