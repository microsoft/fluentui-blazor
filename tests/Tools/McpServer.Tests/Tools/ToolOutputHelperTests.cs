// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Models;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Tools;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tests.Tools;

/// <summary>
/// Tests for <see cref="ToolOutputHelper"/>.
/// </summary>
public class ToolOutputHelperTests
{
    [Fact]
    public void TruncateSummary_WithNull_ReturnsDash()
    {
        // Act
        var result = ToolOutputHelper.TruncateSummary(null, 50);

        // Assert
        Assert.Equal("-", result);
    }

    [Fact]
    public void TruncateSummary_WithEmpty_ReturnsDash()
    {
        // Act
        var result = ToolOutputHelper.TruncateSummary(string.Empty, 50);

        // Assert
        Assert.Equal("-", result);
    }

    [Fact]
    public void TruncateSummary_WithShortText_ReturnsOriginal()
    {
        // Arrange
        var text = "Short text";

        // Act
        var result = ToolOutputHelper.TruncateSummary(text, 50);

        // Assert
        Assert.Equal(text, result);
    }

    [Fact]
    public void TruncateSummary_WithLongText_TruncatesWithEllipsis()
    {
        // Arrange
        var text = "This is a very long text that should be truncated";

        // Act
        var result = ToolOutputHelper.TruncateSummary(text, 20);

        // Assert
        Assert.Equal(20, result.Length);
        Assert.EndsWith("...", result);
    }

    [Fact]
    public void TruncateSummary_WithExactLength_ReturnsOriginal()
    {
        // Arrange
        var text = "Exact length text";

        // Act
        var result = ToolOutputHelper.TruncateSummary(text, text.Length);

        // Assert
        Assert.Equal(text, result);
    }

    [Theory]
    [InlineData("Id", true)]
    [InlineData("Label", true)]
    [InlineData("Placeholder", true)]
    [InlineData("Value", true)]
    [InlineData("Disabled", true)]
    [InlineData("Appearance", true)]
    [InlineData("Size", true)]
    [InlineData("Color", true)]
    public void IsCommonExampleParam_WithCommonParams_ReturnsTrue(string paramName, bool expected)
    {
        // Act
        var result = ToolOutputHelper.IsCommonExampleParam(paramName);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("CustomProperty")]
    [InlineData("InternalId")]
    [InlineData("SomeRandomParam")]
    public void IsCommonExampleParam_WithUncommonParams_ReturnsFalse(string paramName)
    {
        // Act
        var result = ToolOutputHelper.IsCommonExampleParam(paramName);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsCommonExampleParam_IsCaseInsensitive()
    {
        // Act & Assert
        Assert.True(ToolOutputHelper.IsCommonExampleParam("id"));
        Assert.True(ToolOutputHelper.IsCommonExampleParam("ID"));
        Assert.True(ToolOutputHelper.IsCommonExampleParam("Id"));
    }

    [Fact]
    public void GetExampleValue_WithEnumValues_ReturnsFirstEnum()
    {
        // Arrange
        var param = new PropertyInfo
        {
            Name = "Appearance",
            Type = "ButtonAppearance",
            EnumValues = new[] { "Accent", "Outline", "Stealth" }
        };

        // Act
        var result = ToolOutputHelper.GetExampleValue(param);

        // Assert
        Assert.Contains("Accent", result);
    }

    [Fact]
    public void GetExampleValue_WithStringType_ReturnsPlaceholder()
    {
        // Arrange
        var param = new PropertyInfo
        {
            Name = "Label",
            Type = "string",
            EnumValues = Array.Empty<string>()
        };

        // Act
        var result = ToolOutputHelper.GetExampleValue(param);

        // Assert
        Assert.Contains("label", result.ToLowerInvariant());
    }

    [Fact]
    public void GetExampleValue_WithBoolType_ReturnsTrue()
    {
        // Arrange
        var param = new PropertyInfo
        {
            Name = "Disabled",
            Type = "bool",
            EnumValues = Array.Empty<string>()
        };

        // Act
        var result = ToolOutputHelper.GetExampleValue(param);

        // Assert
        Assert.Equal("true", result);
    }

    [Fact]
    public void GetExampleValue_WithIntType_Returns42()
    {
        // Arrange
        var param = new PropertyInfo
        {
            Name = "Count",
            Type = "int",
            EnumValues = Array.Empty<string>()
        };

        // Act
        var result = ToolOutputHelper.GetExampleValue(param);

        // Assert
        Assert.Equal("42", result);
    }

    [Fact]
    public void ExtractEventType_WithGenericEventCallback_ReturnsTypeParameter()
    {
        // Arrange
        var eventType = "EventCallback<MouseEventArgs>";

        // Act
        var result = ToolOutputHelper.ExtractEventType(eventType);

        // Assert
        Assert.Equal("MouseEventArgs", result);
    }

    [Fact]
    public void ExtractEventType_WithNonGeneric_ReturnsEventArgs()
    {
        // Arrange
        var eventType = "EventCallback";

        // Act
        var result = ToolOutputHelper.ExtractEventType(eventType);

        // Assert
        Assert.Equal("EventArgs", result);
    }

    [Fact]
    public void AppendHeader_AppendsCorrectMarkdown()
    {
        // Arrange
        var sb = new StringBuilder();

        // Act
        ToolOutputHelper.AppendHeader(sb, "Test Title", 2);

        // Assert
        var result = sb.ToString();
        Assert.Contains("## Test Title", result);
    }

    [Fact]
    public void AppendHeader_WithLevel1_UsesSingleHash()
    {
        // Arrange
        var sb = new StringBuilder();

        // Act
        ToolOutputHelper.AppendHeader(sb, "Main Title", 1);

        // Assert
        var result = sb.ToString();
        Assert.StartsWith("# Main Title", result);
    }

    [Fact]
    public void AppendTableHeader_CreatesMarkdownTable()
    {
        // Arrange
        var sb = new StringBuilder();

        // Act
        ToolOutputHelper.AppendTableHeader(sb, "Name", "Type", "Description");

        // Assert
        var result = sb.ToString();
        Assert.Contains("| Name | Type | Description |", result);
        Assert.Contains("|------|------|------|", result);
    }
}
