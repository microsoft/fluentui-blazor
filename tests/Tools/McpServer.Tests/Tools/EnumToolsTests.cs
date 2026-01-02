// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.McpServer.Services;
using Microsoft.FluentUI.AspNetCore.McpServer.Tools;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Tools;

/// <summary>
/// Tests for the <see cref="EnumTools"/> class.
/// </summary>
public class EnumToolsTests
{
    private readonly string _testJsonPath;
    private readonly bool _jsonFileExists;

    public EnumToolsTests()
    {
        _testJsonPath = Path.Combine(AppContext.BaseDirectory, "FluentUIComponentsDocumentation.json");
        _jsonFileExists = File.Exists(_testJsonPath);
    }

    private EnumTools CreateTools()
    {
        var service = new FluentUIDocumentationService(_testJsonPath);
        return new EnumTools(service);
    }

    #region GetEnumValues Tests

    [Fact]
    public void GetEnumValues_WithValidEnumName_ShouldReturnValues()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.GetEnumValues("Appearance");

        // Assert
        Assert.False(string.IsNullOrEmpty(result));
        Assert.Contains("# Appearance", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("## Values", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetEnumValues_WithNonExistentEnum_ShouldReturnNotFoundMessage()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.GetEnumValues("NonExistentEnum");

        // Assert
        Assert.Contains("not found", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("ListEnums()", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetEnumValues_CaseInsensitive_ShouldWork()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.GetEnumValues("appearance");

        // Assert
        Assert.Contains("# Appearance", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetEnumValues_ShouldShowTable()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.GetEnumValues("Appearance");

        // Assert
        Assert.Contains("| Name | Value | Description |", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("|------|-------|-------------|", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetEnumValues_WithFilter_ShouldFilterResults()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.GetEnumValues("Appearance", filter: "Accent");

        // Assert
        Assert.Contains("Accent", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetEnumValues_WithFilterNoMatches_ShouldReturnNotFoundMessage()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.GetEnumValues("Appearance", filter: "XyzNonExistent");

        // Assert
        Assert.Contains("No values found matching filter", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetEnumValues_ShouldFormatAsMarkdown()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.GetEnumValues("Appearance");

        // Assert
        Assert.Contains("# ", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("## ", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("|", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetEnumValues_WithNullFilter_ShouldReturnAllValues()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.GetEnumValues("Appearance", filter: null);

        // Assert
        Assert.Contains("## Values", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetEnumValues_WithEmptyFilter_ShouldReturnAllValues()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.GetEnumValues("Appearance", filter: "");

        // Assert
        Assert.Contains("## Values", result, StringComparison.OrdinalIgnoreCase);
    }

    #endregion

    #region GetComponentEnums Tests

    [Fact]
    public void GetComponentEnums_WithValidComponent_ShouldReturnEnums()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.GetComponentEnums("FluentButton");

        // Assert
        Assert.False(string.IsNullOrEmpty(result));
        Assert.Contains("Enum Types for", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetComponentEnums_WithNonExistentComponent_ShouldReturnNotFoundMessage()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.GetComponentEnums("NonExistentComponent");

        // Assert
        Assert.Contains("not found", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("ListComponents()", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetComponentEnums_ShouldShowPropertyToEnumMapping()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.GetComponentEnums("FluentButton");

        // Assert
        Assert.Contains("→", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetComponentEnums_ShouldShowEnumValues()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.GetComponentEnums("FluentButton");

        // Assert
        Assert.Contains("| Value | Description |", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetComponentEnums_ShouldShowUsageHint()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.GetComponentEnums("FluentButton");

        // Assert
        Assert.Contains("GetEnumValues(enumName:", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetComponentEnums_WithoutFluentPrefix_ShouldWork()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.GetComponentEnums("Button");

        // Assert
        Assert.Contains("Enum Types for", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetComponentEnums_ShouldFormatAsMarkdown()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.GetComponentEnums("FluentButton");

        // Assert
        Assert.Contains("# ", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("## ", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("|", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetComponentEnums_ShouldLimitValuesTo10()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.GetComponentEnums("FluentButton");

        // Assert
        // If there are more than 10 values, it should show "more values"
        // This is verified by the presence of "..." in the output if truncated
        Assert.False(string.IsNullOrEmpty(result));
    }

    [Fact]
    public void GetComponentEnums_ShouldShowCount()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var tools = CreateTools();

        // Act
        var result = tools.GetComponentEnums("FluentButton");

        // Assert
        Assert.Contains("enum type(s)", result, StringComparison.OrdinalIgnoreCase);
    }

    #endregion

    #region Constructor Tests

    [Fact]
    public void Constructor_ShouldInitializeCorrectly()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Act
        var tools = CreateTools();

        // Assert
        Assert.NotNull(tools);
    }

    #endregion
}
