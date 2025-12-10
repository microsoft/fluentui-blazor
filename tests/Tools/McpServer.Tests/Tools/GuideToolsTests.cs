// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Tools;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tests.Tools;

/// <summary>
/// Tests for <see cref="GuideTools"/>.
/// </summary>
public class GuideToolsTests
{
    private readonly GuideTools _tools;
    private readonly DocumentationGuideService _guideService;

    public GuideToolsTests()
    {
        _guideService = new DocumentationGuideService();
        _tools = new GuideTools(_guideService);
    }

    [Fact]
    public void ListGuides_ReturnsMarkdownWithGuides()
    {
        // Act
        var result = _tools.ListGuides();

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains("#", result); // Contains markdown headers
        Assert.Contains("installation", result, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData("installation")]
    [InlineData("defaultvalues")]
    [InlineData("localization")]
    [InlineData("styles")]
    public void GetGuide_ReturnsContentForKnownGuides(string guideKey)
    {
        // Act
        var result = _tools.GetGuide(guideKey);

        // Assert
        Assert.NotEmpty(result);
        // The content should not start with "Guide 'X' not found"
        Assert.False(result.StartsWith($"Guide '{guideKey}' not found"), $"Expected content, but got 'not found' message for {guideKey}");
    }

    [Fact]
    public void GetGuide_ReturnsMessageForUnknownGuide()
    {
        // Act
        var result = _tools.GetGuide("nonexistent-guide");

        // Assert
        Assert.Contains("not found", result, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData("install")]
    [InlineData("nuget")]
    [InlineData("package")]
    public void SearchGuides_FindsMatchingGuides(string searchTerm)
    {
        // Act
        var result = _tools.SearchGuides(searchTerm);

        // Assert
        Assert.NotEmpty(result);
    }

    [Fact]
    public void SearchGuides_ReturnsMessageForNoMatches()
    {
        // Act
        var result = _tools.SearchGuides("xyz123nonexistent");

        // Assert
        Assert.Contains("No matches found", result);
    }
}
