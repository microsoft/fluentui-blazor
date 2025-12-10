// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Tools;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tests.Tools;

/// <summary>
/// Tests for <see cref="EnumTools"/>.
/// </summary>
public class EnumToolsTests
{
    private readonly EnumTools _tools;
    private readonly FluentUIDocumentationService _documentationService;

    public EnumToolsTests()
    {
        var assembly = typeof(Microsoft.FluentUI.AspNetCore.Components._Imports).Assembly;
        var xmlPath = XmlDocumentationFinder.Find();
        _documentationService = new FluentUIDocumentationService(assembly, xmlPath);
        _tools = new EnumTools(_documentationService);
    }

    [Fact]
    public void ListEnums_ReturnsMarkdownWithEnums()
    {
        // Act
        var result = _tools.ListEnums();

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains("#", result); // Contains markdown headers
    }

    [Fact]
    public void ListEnums_WithComponentFilter_FiltersResults()
    {
        // Act
        var result = _tools.ListEnums("FluentButton");

        // Assert
        Assert.NotEmpty(result);
    }

    [Theory]
    [InlineData("ButtonAppearance")]
    [InlineData("Color")]
    [InlineData("Orientation")]
    public void GetEnumValues_ReturnsValuesForKnownEnums(string enumName)
    {
        // Act
        var result = _tools.GetEnumValues(enumName);

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(enumName, result);
        Assert.Contains("Values", result);
    }

    [Fact]
    public void GetEnumValues_ReturnsMessageForUnknownEnum()
    {
        // Act
        var result = _tools.GetEnumValues("NonExistentEnum");

        // Assert
        Assert.Contains("not found", result);
    }

    [Fact]
    public void GetEnumValues_ButtonAppearance_ContainsExpectedValues()
    {
        // Act
        var result = _tools.GetEnumValues("ButtonAppearance");

        // Assert
        Assert.Contains("Primary", result);
    }

    [Fact]
    public void GetComponentEnums_FluentButton_ReturnsEnums()
    {
        // Act
        var result = _tools.GetComponentEnums("FluentButton");

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains("FluentButton", result);
    }

    [Fact]
    public void GetComponentEnums_ReturnsMessageForUnknownComponent()
    {
        // Act
        var result = _tools.GetComponentEnums("NonExistentComponent");

        // Assert
        Assert.Contains("not found", result);
    }
}
