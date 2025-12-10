// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tests.Services;

/// <summary>
/// Tests for <see cref="DocumentationGuideService"/>.
/// </summary>
public class DocumentationGuideServiceTests
{
    private readonly DocumentationGuideService _service;

    public DocumentationGuideServiceTests()
    {
        _service = new DocumentationGuideService();
    }

    [Fact]
    public void GetAllGuides_ReturnsNonEmptyList()
    {
        // Act
        var guides = _service.GetAllGuides();

        // Assert
        Assert.NotEmpty(guides);
    }

    [Theory]
    [InlineData("installation")]
    [InlineData("defaultvalues")]
    [InlineData("migration")]
    [InlineData("localization")]
    [InlineData("styles")]
    public void GetAllGuides_ContainsExpectedGuides(string guideKey)
    {
        // Act
        var guides = _service.GetAllGuides();

        // Assert
        Assert.Contains(guides, g => g.Key.Equals(guideKey, StringComparison.OrdinalIgnoreCase));
    }

    [Theory]
    [InlineData("installation")]
    [InlineData("defaultvalues")]
    [InlineData("migration")]
    [InlineData("localization")]
    [InlineData("styles")]
    public void GetGuideContent_ReturnsContentForKnownGuides(string guideKey)
    {
        // Act
        var content = _service.GetGuideContent(guideKey);

        // Assert
        Assert.NotNull(content);
        Assert.NotEmpty(content);
    }

    [Fact]
    public void GetGuideContent_ReturnsNullForUnknownGuide()
    {
        // Act
        var content = _service.GetGuideContent("nonexistent-guide");

        // Assert
        Assert.Null(content);
    }

    [Fact]
    public void GetGuideContent_IsCaseInsensitive()
    {
        // Act
        var contentLower = _service.GetGuideContent("installation");
        var contentUpper = _service.GetGuideContent("INSTALLATION");

        // Assert
        Assert.Equal(contentLower, contentUpper);
    }

    [Fact]
    public void GetFullMigrationGuide_ReturnsContent()
    {
        // Act
        var content = _service.GetFullMigrationGuide();

        // Assert
        Assert.NotNull(content);
        Assert.NotEmpty(content);
    }

    [Fact]
    public void GetGuide_ReturnsGuideForKnownKey()
    {
        // Act
        var guide = _service.GetGuide("installation");

        // Assert
        Assert.NotNull(guide);
        Assert.Equal("installation", guide.Key);
    }

    [Fact]
    public void GetGuide_ReturnsNullForUnknownKey()
    {
        // Act
        var guide = _service.GetGuide("nonexistent");

        // Assert
        Assert.Null(guide);
    }

    [Fact]
    public void GetGuide_IsCaseInsensitive()
    {
        // Act
        var guideLower = _service.GetGuide("installation");
        var guideUpper = _service.GetGuide("INSTALLATION");

        // Assert
        Assert.NotNull(guideLower);
        Assert.NotNull(guideUpper);
        Assert.Equal(guideLower.Key, guideUpper.Key);
    }
}
