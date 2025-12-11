// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Tools;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tests.Tools;

/// <summary>
/// Additional tests for Tools to improve code coverage.
/// </summary>
public class ToolsEdgeCasesTests
{
    private readonly FluentUIDocumentationService _documentationService;

    public ToolsEdgeCasesTests()
    {
        var jsonPath = JsonDocumentationFinder.Find();
        _documentationService = new FluentUIDocumentationService(jsonPath);
    }

    [Fact]
    public void ComponentListTools_SearchComponents_WithEmptyTerm_ReturnsMessage()
    {
        // Arrange
        var tools = new ComponentListTools(_documentationService);

        // Act
        var result = tools.SearchComponents("");

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void ComponentDetailTools_GetComponentDetails_CaseInsensitive()
    {
        // Arrange
        var tools = new ComponentDetailTools(_documentationService);

        // Act
        var resultLower = tools.GetComponentDetails("fluentbutton");
        var resultUpper = tools.GetComponentDetails("FLUENTBUTTON");
        var resultMixed = tools.GetComponentDetails("FluentButton");

        // Assert
        Assert.NotNull(resultLower);
        Assert.NotNull(resultUpper);
        Assert.NotNull(resultMixed);
        Assert.Contains("FluentButton", resultLower);
    }

    [Fact]
    public void EnumTools_GetEnumValues_WithInvalidEnum_ReturnsNotFound()
    {
        // Arrange
        var tools = new EnumTools(_documentationService);

        // Act
        var result = tools.GetEnumValues("NonExistentEnum");

        // Assert
        Assert.Contains("not found", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GuideTools_GetGuide_WithValidTopic_ReturnsContent()
    {
        // Arrange
        var guideService = new DocumentationGuideService();
        var tools = new GuideTools(guideService);

        // Act
        var result = tools.GetGuide("installation");

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void GuideTools_GetGuide_WithInvalidTopic_ReturnsNotFound()
    {
        // Arrange
        var guideService = new DocumentationGuideService();
        var tools = new GuideTools(guideService);

        // Act
        var result = tools.GetGuide("invalid-topic-that-does-not-exist");

        // Assert
        Assert.Contains("not found", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GuideTools_ListGuides_ReturnsNonEmptyList()
    {
        // Arrange
        var guideService = new DocumentationGuideService();
        var tools = new GuideTools(guideService);

        // Act
        var result = tools.ListGuides();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void VersionTools_GetVersionInfo_ContainsAllSections()
    {
        // Arrange
        var tools = new VersionTools(_documentationService);

        // Act
        var result = tools.GetVersionInfo();

        // Assert
        Assert.Contains("MCP Server Version", result);
        Assert.Contains("Components Version", result);
        Assert.Contains("Documentation Generated", result);
        Assert.Contains("Documentation Available", result);
        Assert.Contains("Documentation Statistics", result);
        Assert.Contains("Compatibility", result);
    }

    [Fact]
    public void VersionTools_CheckVersionCompatibility_WithEmptyVersion_ReturnsError()
    {
        // Arrange
        var tools = new VersionTools(_documentationService);

        // Act
        var result = tools.CheckVersionCompatibility("invalid-version");

        // Assert
        Assert.Contains("Unable to parse", result);
    }

    [Fact]
    public void ComponentListTools_ListComponents_WithNullCategory_ReturnsAllComponents()
    {
        // Arrange
        var tools = new ComponentListTools(_documentationService);

        // Act
        var result = tools.ListComponents(null);

        // Assert
        Assert.NotNull(result);
        Assert.Contains("FluentButton", result);
    }
}
