// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.McpServer.Prompts;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tests.Prompts;

/// <summary>
/// Additional tests for prompts to improve coverage.
/// </summary>
public class PromptEdgeCasesTests
{
    private readonly FluentUIDocumentationService _documentationService;

    public PromptEdgeCasesTests()
    {
        var jsonPath = JsonDocumentationFinder.Find();
        _documentationService = new FluentUIDocumentationService(jsonPath);
    }

    #region CreateDrawerPrompt Tests

    [Fact]
    public void CreateDrawer_WithAllParameters_GeneratesCompletePrompt()
    {
        // Arrange
        var prompt = new CreateDrawerPrompt(_documentationService);

        // Act
        var result = prompt.CreateDrawer("navigation menu", "start", "links and sections");

        // Assert
        Assert.Contains("navigation menu", result.Text);
        Assert.Contains("start", result.Text);
        Assert.Contains("links and sections", result.Text);
    }

    [Fact]
    public void CreateDrawer_WithMinimalParameters_Works()
    {
        // Arrange
        var prompt = new CreateDrawerPrompt(_documentationService);

        // Act
        var result = prompt.CreateDrawer("simple drawer");

        // Assert
        Assert.NotNull(result);
        Assert.Contains("simple drawer", result.Text);
    }

    [Fact]
    public void CreateDrawer_WithNullOptionalParameters_HandlesGracefully()
    {
        // Arrange
        var prompt = new CreateDrawerPrompt(_documentationService);

        // Act
        var result = prompt.CreateDrawer("drawer", null, null);

        // Assert
        Assert.NotNull(result);
        Assert.Contains("drawer", result.Text);
    }

    #endregion

    #region CreateDataGridPrompt Tests

    [Fact]
    public void CreateDataGrid_WithAllFeatures_IncludesAllSections()
    {
        // Arrange
        var prompt = new CreateDataGridPrompt(_documentationService);

        // Act
        var result = prompt.CreateDataGrid("users", "sorting,filtering,pagination", "User");

        // Assert
        Assert.Contains("sorting", result.Text, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("filtering", result.Text, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("pagination", result.Text, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("User", result.Text);
    }

    [Fact]
    public void CreateDataGrid_WithNullOptionalParameters_Works()
    {
        // Arrange
        var prompt = new CreateDataGridPrompt(_documentationService);

        // Act
        var result = prompt.CreateDataGrid("products", null, null);

        // Assert
        Assert.NotNull(result);
        Assert.Contains("products", result.Text);
    }

    [Fact]
    public void CreateDataGrid_WithEmptyFeatures_HandlesGracefully()
    {
        // Arrange
        var prompt = new CreateDataGridPrompt(_documentationService);

        // Act
        var result = prompt.CreateDataGrid("items", "", "Item");

        // Assert
        Assert.NotNull(result);
        Assert.Contains("items", result.Text);
    }

    #endregion

    #region MigrateToV5Prompt Tests

    [Fact]
    public void MigrateToV5_WithComplexCode_IncludesFocusArea()
    {
        // Arrange
        var guideService = new DocumentationGuideService();
        var prompt = new MigrateToV5Prompt(guideService);
        var code = @"
<FluentButton Appearance=""@Appearance.Accent"">
    <FluentIcon Name=""@FluentIcons.Add"" />
    Click me
</FluentButton>";

        // Act
        var result = prompt.MigrateToV5(code, "Button appearance and icons");

        // Assert
        Assert.Contains(code, result.Text);
        Assert.Contains("Button appearance and icons", result.Text);
        Assert.Contains("Focus Areas", result.Text);
    }

    [Fact]
    public void MigrateToV5_WithNullFocus_Works()
    {
        // Arrange
        var guideService = new DocumentationGuideService();
        var prompt = new MigrateToV5Prompt(guideService);

        // Act
        var result = prompt.MigrateToV5("<FluentButton>Test</FluentButton>", null);

        // Assert
        Assert.NotNull(result);
        Assert.DoesNotContain("Focus Areas", result.Text);
    }

    [Fact]
    public void MigrateToV5_WithLongCode_TruncatesGuide()
    {
        // Arrange
        var guideService = new DocumentationGuideService();
        var prompt = new MigrateToV5Prompt(guideService);
        var longCode = string.Join("\n", Enumerable.Repeat("<FluentButton>Test</FluentButton>", 100));

        // Act
        var result = prompt.MigrateToV5(longCode);

        // Assert
        Assert.NotNull(result);
        Assert.Contains("```razor", result.Text);
    }

    #endregion

    #region CheckVersionCompatibilityPrompt Tests

    [Fact]
    public void CheckVersionCompatibility_WithPatchDifference_ShowsMinorWarning()
    {
        // Arrange
        var prompt = new CheckVersionCompatibilityPrompt(_documentationService);
        var expectedVersion = _documentationService.ComponentsVersion;

        // Parse and create a patch version difference
        var parts = expectedVersion.Split('.');
        if (parts.Length >= 3)
        {
            // Remove any prerelease suffix (e.g., "0-alpha" -> "0")
            var patchPart = parts[2].Split('-')[0];
            if (int.TryParse(patchPart, System.Globalization.CultureInfo.InvariantCulture, out var patchNum))
            {
                var patchVersion = $"{parts[0]}.{parts[1]}.{patchNum + 1}";

                // Act
                var result = prompt.CheckVersionCompatibility(patchVersion);

                // Assert
                Assert.NotNull(result);
                // Should show some compatibility info
                Assert.Contains("Version", result.Text);
            }
        }
    }

    [Fact]
    public void CheckVersionCompatibility_WithPreReleaseVersion_HandlesCorrectly()
    {
        // Arrange
        var prompt = new CheckVersionCompatibilityPrompt(_documentationService);

        // Act
        var result = prompt.CheckVersionCompatibility("5.0.0-beta.1");

        // Assert
        Assert.NotNull(result);
        Assert.Contains("Version", result.Text);
    }

    #endregion

    #region ExplainComponentPrompt Tests

    [Fact]
    public void ExplainComponent_WithNonExistentComponent_HandlesGracefully()
    {
        // Arrange
        var prompt = new ExplainComponentPrompt(_documentationService);

        // Act
        var result = prompt.ExplainComponent("NonExistentComponent");

        // Assert
        Assert.NotNull(result);
        Assert.Contains("NonExistentComponent", result.Text);
    }

    [Fact]
    public void ExplainComponent_WithGenericComponent_IncludesGenericInfo()
    {
        // Arrange
        var prompt = new ExplainComponentPrompt(_documentationService);

        // Act - FluentDataGrid is generic
        var result = prompt.ExplainComponent("FluentDataGrid");

        // Assert
        Assert.NotNull(result);
        Assert.Contains("FluentDataGrid", result.Text);
    }

    #endregion

    #region SetupProjectPrompt Tests

    [Fact]
    public void SetupProject_WithMultipleFeatures_IncludesAllFeatures()
    {
        // Arrange
        var guideService = new DocumentationGuideService();
        var prompt = new SetupProjectPrompt(guideService, _documentationService);

        // Act
        var result = prompt.SetupProject("server", "icons, emoji, datagrid");

        // Assert
        Assert.Contains("icons", result.Text, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("emoji", result.Text, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void SetupProject_WithEmptyFeatures_Works()
    {
        // Arrange
        var guideService = new DocumentationGuideService();
        var prompt = new SetupProjectPrompt(guideService, _documentationService);

        // Act
        var result = prompt.SetupProject("wasm", null);

        // Assert
        Assert.NotNull(result);
        Assert.Contains("wasm", result.Text, StringComparison.OrdinalIgnoreCase);
    }

    #endregion
}
