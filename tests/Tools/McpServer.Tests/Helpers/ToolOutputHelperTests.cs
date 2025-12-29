// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text;
using Microsoft.FluentUI.AspNetCore.McpServer.Helpers;
using Microsoft.FluentUI.AspNetCore.McpServer.Models;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Helpers;

/// <summary>
/// Tests for the <see cref="ToolOutputHelper"/> class.
/// </summary>
public class ToolOutputHelperTests
{
    #region TruncateSummary Tests

    [Fact]
    public void TruncateSummary_WithNullSummary_ShouldReturnDash()
    {
        // Arrange
        string? summary = null;

        // Act
        var result = ToolOutputHelper.TruncateSummary(summary, 50);

        // Assert
        Assert.Equal("-", result);
    }

    [Fact]
    public void TruncateSummary_WithEmptySummary_ShouldReturnDash()
    {
        // Arrange
        var summary = string.Empty;

        // Act
        var result = ToolOutputHelper.TruncateSummary(summary, 50);

        // Assert
        Assert.Equal("-", result);
    }

    [Fact]
    public void TruncateSummary_WithShortSummary_ShouldReturnUnchanged()
    {
        // Arrange
        var summary = "Short summary";

        // Act
        var result = ToolOutputHelper.TruncateSummary(summary, 50);

        // Assert
        Assert.Equal("Short summary", result);
    }

    [Fact]
    public void TruncateSummary_WithExactLengthSummary_ShouldReturnUnchanged()
    {
        // Arrange
        var summary = "12345678901234567890"; // 20 characters

        // Act
        var result = ToolOutputHelper.TruncateSummary(summary, 20);

        // Assert
        Assert.Equal("12345678901234567890", result);
        Assert.Equal(20, result.Length);
    }

    [Fact]
    public void TruncateSummary_WithLongSummary_ShouldTruncateWithEllipsis()
    {
        // Arrange
        var summary = "This is a very long summary that needs to be truncated";

        // Act
        var result = ToolOutputHelper.TruncateSummary(summary, 20);

        // Assert
        Assert.EndsWith("...", result, StringComparison.OrdinalIgnoreCase);
        Assert.Equal(20, result.Length);
        Assert.Equal("This is a very lo...", result);
    }

    [Fact]
    public void TruncateSummary_WithMaxLengthOfThree_ShouldReturnEllipsis()
    {
        // Arrange
        var summary = "Long text here";

        // Act
        var result = ToolOutputHelper.TruncateSummary(summary, 3);

        // Assert
        Assert.Equal("...", result);
    }

    #endregion

    #region IsCommonExampleParam Tests

    [Theory]
    [InlineData("Id")]
    [InlineData("Label")]
    [InlineData("Placeholder")]
    [InlineData("Value")]
    [InlineData("Disabled")]
    [InlineData("ReadOnly")]
    [InlineData("Appearance")]
    [InlineData("Size")]
    [InlineData("Color")]
    [InlineData("IconStart")]
    [InlineData("IconEnd")]
    public void IsCommonExampleParam_WithCommonParams_ShouldReturnTrue(string paramName)
    {
        // Act
        var result = ToolOutputHelper.IsCommonExampleParam(paramName);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData("id")]
    [InlineData("LABEL")]
    [InlineData("placeholder")]
    [InlineData("VALUE")]
    public void IsCommonExampleParam_WithCaseInsensitiveCommonParams_ShouldReturnTrue(string paramName)
    {
        // Act
        var result = ToolOutputHelper.IsCommonExampleParam(paramName);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData("CustomProperty")]
    [InlineData("ChildContent")]
    [InlineData("OnClick")]
    [InlineData("Items")]
    [InlineData("")]
    public void IsCommonExampleParam_WithNonCommonParams_ShouldReturnFalse(string paramName)
    {
        // Act
        var result = ToolOutputHelper.IsCommonExampleParam(paramName);

        // Assert
        Assert.False(result);
    }

    #endregion

    #region GetExampleValue Tests

    [Fact]
    public void GetExampleValue_WithStringType_ShouldReturnPlaceholder()
    {
        // Arrange
        var param = new PropertyInfo
        {
            Name = "Label",
            Type = "string",
        };

        // Act
        var result = ToolOutputHelper.GetExampleValue(param);

        // Assert
        Assert.Equal("your-label", result);
    }

    [Fact]
    public void GetExampleValue_WithBoolType_ShouldReturnTrue()
    {
        // Arrange
        var param = new PropertyInfo
        {
            Name = "Disabled",
            Type = "bool",
        };

        // Act
        var result = ToolOutputHelper.GetExampleValue(param);

        // Assert
        Assert.Equal("true", result);
    }

    [Fact]
    public void GetExampleValue_WithIntType_ShouldReturn42()
    {
        // Arrange
        var param = new PropertyInfo
        {
            Name = "MaxLength",
            Type = "int",
        };

        // Act
        var result = ToolOutputHelper.GetExampleValue(param);

        // Assert
        Assert.Equal("42", result);
    }

    [Fact]
    public void GetExampleValue_WithEnumValues_ShouldReturnFirstEnumValue()
    {
        // Arrange
        var param = new PropertyInfo
        {
            Name = "Appearance",
            Type = "Appearance",
            EnumValues = ["Accent", "Lightweight", "Neutral"],
        };

        // Act
        var result = ToolOutputHelper.GetExampleValue(param);

        // Assert
        Assert.Equal("@Appearance.Accent", result);
    }

    [Fact]
    public void GetExampleValue_WithUnknownType_ShouldReturnEllipsis()
    {
        // Arrange
        var param = new PropertyInfo
        {
            Name = "Data",
            Type = "CustomType",
        };

        // Act
        var result = ToolOutputHelper.GetExampleValue(param);

        // Assert
        Assert.Equal("...", result);
    }

    [Fact]
    public void GetExampleValue_WithStringName_ShouldUseLowercaseName()
    {
        // Arrange
        var param = new PropertyInfo
        {
            Name = "Placeholder",
            Type = "string",
        };

        // Act
        var result = ToolOutputHelper.GetExampleValue(param);

        // Assert
        Assert.Equal("your-placeholder", result);
    }

    #endregion

    #region AppendHeader Tests

    [Fact]
    public void AppendHeader_WithLevel1_ShouldAppendSingleHash()
    {
        // Arrange
        var sb = new StringBuilder();

        // Act
        ToolOutputHelper.AppendHeader(sb, "Title", 1);

        // Assert
        Assert.Contains("# Title", sb.ToString(), StringComparison.OrdinalIgnoreCase);
        Assert.EndsWith(Environment.NewLine, sb.ToString(), StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void AppendHeader_WithLevel2_ShouldAppendDoubleHash()
    {
        // Arrange
        var sb = new StringBuilder();

        // Act
        ToolOutputHelper.AppendHeader(sb, "Subtitle", 2);

        // Assert
        Assert.Contains("## Subtitle", sb.ToString(), StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void AppendHeader_WithLevel3_ShouldAppendTripleHash()
    {
        // Arrange
        var sb = new StringBuilder();

        // Act
        ToolOutputHelper.AppendHeader(sb, "Section", 3);

        // Assert
        Assert.Contains("### Section", sb.ToString(), StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void AppendHeader_DefaultLevel_ShouldBeLevel1()
    {
        // Arrange
        var sb = new StringBuilder();

        // Act
        ToolOutputHelper.AppendHeader(sb, "Default Title");

        // Assert
        Assert.StartsWith("# Default Title", sb.ToString(), StringComparison.OrdinalIgnoreCase);
    }

    #endregion

    #region AppendTableHeader Tests

    [Fact]
    public void AppendTableHeader_WithTwoColumns_ShouldFormatCorrectly()
    {
        // Arrange
        var sb = new StringBuilder();

        // Act
        ToolOutputHelper.AppendTableHeader(sb, "Name", "Value");

        // Assert
        var result = sb.ToString();
        Assert.Contains("| Name | Value |", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("|------|------|", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void AppendTableHeader_WithThreeColumns_ShouldFormatCorrectly()
    {
        // Arrange
        var sb = new StringBuilder();

        // Act
        ToolOutputHelper.AppendTableHeader(sb, "Name", "Type", "Description");

        // Assert
        var result = sb.ToString();
        Assert.Contains("| Name | Type | Description |", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("|------|------|------|", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void AppendTableHeader_WithSingleColumn_ShouldFormatCorrectly()
    {
        // Arrange
        var sb = new StringBuilder();

        // Act
        ToolOutputHelper.AppendTableHeader(sb, "Name");

        // Assert
        var result = sb.ToString();
        Assert.Contains("| Name |", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("|------|", result, StringComparison.OrdinalIgnoreCase);
    }

    #endregion
}
