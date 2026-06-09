// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Label;

public class LabelInfoTests
{
    [Fact]
    public void LabelInfo_DefaultConstructor_HasDefaults()
    {
        // Act
        var info = new LabelInfo();

        // Assert
        Assert.Null(info.InfoText);
        Assert.Null(info.InfoActionLink);
        Assert.Null(info.InfoActionText);
        Assert.Equal(LinkTarget.Blank, info.InfoActionTarget);
    }

    [Fact]
    public void LabelInfo_TextConstructor_SetsTextOnly()
    {
        // Act
        var info = new LabelInfo("hello");

        // Assert
        Assert.Equal("hello", info.InfoText);
        Assert.Null(info.InfoActionLink);
        Assert.Null(info.InfoActionText);
        Assert.Equal(LinkTarget.Blank, info.InfoActionTarget);
    }

    [Fact]
    public void LabelInfo_FullConstructor_SetsAllValues()
    {
        // Act
        var info = new LabelInfo(
            text: "hello",
            actionLink: "https://example.com",
            actionText: "Learn",
            actionTarget: LinkTarget.Self);

        // Assert
        Assert.Equal("hello", info.InfoText);
        Assert.Equal("https://example.com", info.InfoActionLink);
        Assert.Equal("Learn", info.InfoActionText);
        Assert.Equal(LinkTarget.Self, info.InfoActionTarget);
    }

    [Fact]
    public void LabelInfo_ImplementsILabelInfo()
    {
        // Act
        ILabelInfo info = new LabelInfo
        {
            InfoText = "t",
            InfoActionLink = "l",
            InfoActionText = "a",
            InfoActionTarget = LinkTarget.Parent,
        };

        // Assert
        Assert.Equal("t", info.InfoText);
        Assert.Equal("l", info.InfoActionLink);
        Assert.Equal("a", info.InfoActionText);
        Assert.Equal(LinkTarget.Parent, info.InfoActionTarget);
    }

    [Fact]
    public void LabelInfoBuilder_WithText_CreatesInstance()
    {
        // Act
        var info = LabelInfo.WithText("hello");

        // Assert
        Assert.Equal("hello", info.InfoText);
        Assert.Null(info.InfoActionLink);
        Assert.Null(info.InfoActionText);
        Assert.Equal(LinkTarget.Blank, info.InfoActionTarget);
    }

    [Fact]
    public void LabelInfoBuilder_WithActionLink_SetsValueAndReturnsSameInstance()
    {
        // Arrange
        var info = LabelInfo.WithText("hello");

        // Act
        var result = info.WithActionLink("https://example.com");

        // Assert
        Assert.Same(info, result);
        Assert.Equal("https://example.com", info.InfoActionLink);
    }

    [Fact]
    public void LabelInfoBuilder_WithActionText_SetsValueAndReturnsSameInstance()
    {
        // Arrange
        var info = LabelInfo.WithText("hello");

        // Act
        var result = info.WithActionText("Learn more");

        // Assert
        Assert.Same(info, result);
        Assert.Equal("Learn more", info.InfoActionText);
    }

    [Fact]
    public void LabelInfoBuilder_WithActionTarget_SetsValueAndReturnsSameInstance()
    {
        // Arrange
        var info = LabelInfo.WithText("hello");

        // Act
        var result = info.WithActionTarget(LinkTarget.Top);

        // Assert
        Assert.Same(info, result);
        Assert.Equal(LinkTarget.Top, info.InfoActionTarget);
    }

    [Fact]
    public void LabelInfoBuilder_Chain_AppliesAllValues()
    {
        // Act
        var info = LabelInfo.WithText("hello")
            .WithActionLink("https://example.com")
            .WithActionText("Read")
            .WithActionTarget(LinkTarget.Self);

        // Assert
        Assert.Equal("hello", info.InfoText);
        Assert.Equal("https://example.com", info.InfoActionLink);
        Assert.Equal("Read", info.InfoActionText);
        Assert.Equal(LinkTarget.Self, info.InfoActionTarget);
    }

     [Fact]
    public void LabelInfo_MaxWidthProperty_CanBeSet()
    {
        // Arrange & Act
        var labelInfo = new LabelInfo { MaxWidth = "300px" };

        // Assert
        Assert.Equal("300px", labelInfo.MaxWidth);
    }

    [Fact]
    public void LabelInfo_Constructor_SetsMaxWidth()
    {
        // Arrange & Act
        var labelInfo = new LabelInfo("Info text", maxWidth: "150px");

        // Assert
        Assert.Equal("150px", labelInfo.MaxWidth);
    }

    [Fact]
    public void LabelInfo_ConstructorWithoutMaxWidth_MaxWidthIsNull()
    {
        // Arrange & Act
        var labelInfo = new LabelInfo("Info text");

        // Assert
        Assert.Null(labelInfo.MaxWidth);
    }

    [Fact]
    public void LabelInfo_WithMaxWidth_SetsMaxWidth()
    {
        // Arrange & Act
        var labelInfo = LabelInfo.WithText("Info text").WithMaxWidth("250px");

        // Assert
        Assert.Equal("250px", labelInfo.MaxWidth);
    }

    [Fact]
    public void LabelInfo_WithMaxWidth_ReturnsSameInstance()
    {
        // Arrange
        var labelInfo = LabelInfo.WithText("Info text");

        // Act
        var result = labelInfo.WithMaxWidth("250px");

        // Assert
        Assert.Same(labelInfo, result);
    }

    [Fact]
    public void LabelInfo_WithMaxWidthNull_SetsMaxWidthToNull()
    {
        // Arrange & Act
        var labelInfo = LabelInfo.WithText("Info text").WithMaxWidth(null);

        // Assert
        Assert.Null(labelInfo.MaxWidth);
    }
}
