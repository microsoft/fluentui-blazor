// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Tools;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tests.Tools;

/// <summary>
/// Tests for <see cref="ComponentDetailTools"/>.
/// </summary>
public class ComponentDetailToolsTests
{
    private readonly ComponentDetailTools _tools;
    private readonly FluentUIDocumentationService _documentationService;

    public ComponentDetailToolsTests()
    {
        var assembly = typeof(Microsoft.FluentUI.AspNetCore.Components._Imports).Assembly;
        var xmlPath = XmlDocumentationFinder.Find();
        _documentationService = new FluentUIDocumentationService(assembly, xmlPath);
        _tools = new ComponentDetailTools(_documentationService);
    }

    [Theory]
    [InlineData("FluentButton")]
    [InlineData("FluentTextInput")]
    [InlineData("FluentCard")]
    public void GetComponentDetails_ReturnsDetailsForKnownComponents(string componentName)
    {
        // Act
        var result = _tools.GetComponentDetails(componentName);

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(componentName, result);
        Assert.Contains("Parameters", result);
    }

    [Fact]
    public void GetComponentDetails_ReturnsMessageForUnknownComponent()
    {
        // Act
        var result = _tools.GetComponentDetails("NonExistentComponent");

        // Assert
        Assert.Contains("not found", result);
    }

    [Fact]
    public void GetComponentDetails_FluentButton_ContainsAppearanceParameter()
    {
        // Act
        var result = _tools.GetComponentDetails("FluentButton");

        // Assert
        Assert.Contains("Appearance", result);
    }

    [Fact]
    public void GetComponentDetails_IsCaseInsensitive()
    {
        // Act
        var resultLower = _tools.GetComponentDetails("fluentbutton");
        var resultMixed = _tools.GetComponentDetails("FluentButton");

        // Assert
        // Both should find the component (or both should not find it)
        var lowerFound = !resultLower.Contains("not found");
        var mixedFound = !resultMixed.Contains("not found");
        Assert.Equal(lowerFound, mixedFound);
    }

    [Theory]
    [InlineData("FluentButton")]
    [InlineData("FluentTextField")]
    public void GetComponentExample_ReturnsExampleForKnownComponents(string componentName)
    {
        // Act
        var result = _tools.GetComponentExample(componentName);

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(componentName, result);
    }

    [Fact]
    public void GetComponentExample_ReturnsMessageForUnknownComponent()
    {
        // Act
        var result = _tools.GetComponentExample("NonExistentComponent");

        // Assert
        Assert.Contains("not found", result);
    }
}
