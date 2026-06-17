// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.MessageBar;

public class MessageBarOptionsTests
{
    [Fact]
    public void MessageBarOptions_DefaultValues()
    {
        // Act
        var options = new MessageBarOptions();

        // Assert
        Assert.Null(options.Id);
        Assert.Null(options.Class);
        Assert.Null(options.Style);
        Assert.Null(options.Margin);
        Assert.Null(options.Padding);
        Assert.Null(options.Data);
        Assert.Null(options.AdditionalAttributes);
        Assert.NotNull(options.Parameters);
        Assert.Empty(options.Parameters);
        Assert.Equal(string.Empty, options.Section);
        Assert.Null(options.Intent);
        Assert.Null(options.Layout);
        Assert.Null(options.Shape);
        Assert.Null(options.Animation);
        Assert.Null(options.AriaLive);
        Assert.Null(options.Icon);
        Assert.Null(options.Title);
        Assert.Null(options.Message);
        Assert.True(options.AllowDismiss);
        Assert.Null(options.TimeStamp);
        Assert.Null(options.Lifetime);
        Assert.Null(options.OnStatusChange);
    }

    [Fact]
    public void MessageBarOptions_FactoryCtor_InvokesFactory()
    {
        // Act
        var options = new MessageBarOptions(o =>
        {
            o.Section = "main";
            o.Title = "Hello";
            o.Message = "World";
            o.Intent = MessageBarIntent.Success;
        });

        // Assert
        Assert.Equal("main", options.Section);
        Assert.Equal("Hello", options.Title);
        Assert.Equal("World", options.Message);
        Assert.Equal(MessageBarIntent.Success, options.Intent);
    }

    [Fact]
    public void MessageBarOptions_ClassValue_IncludesClassAndSpacing()
    {
        // Arrange
        var options = new MessageBarOptions
        {
            Class = "my-class",
            Margin = "10px",
            Padding = "20px",
        };

        // Act
        var classValue = typeof(MessageBarOptions)
            .GetProperty("ClassValue", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!
            .GetValue(options) as string;

        // Assert
        Assert.NotNull(classValue);
        Assert.Contains("my-class", classValue);
    }

    [Fact]
    public void MessageBarOptions_StyleValue_IncludesMarginAndPadding()
    {
        // Arrange
        var options = new MessageBarOptions
        {
            Style = "color: red;",
            Margin = "10px",
            Padding = "20px",
        };

        // Act
        var styleValue = typeof(MessageBarOptions)
            .GetProperty("StyleValue", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!
            .GetValue(options) as string;

        // Assert
        Assert.NotNull(styleValue);
        Assert.Contains("color: red", styleValue);
        Assert.Contains("margin: 10px", styleValue);
        Assert.Contains("padding: 20px", styleValue);
    }

    [Fact]
    public void MessageBarOptions_SetParameters_Accepts()
    {
        // Arrange
        var options = new MessageBarOptions();

        // Act
        options.Parameters["Name"] = "John";

        // Assert
        Assert.Single(options.Parameters);
        Assert.Equal("John", options.Parameters["Name"]);
    }

    [Fact]
    public void MessageBarOptions_AdditionalAttributes_IsAssignable()
    {
        // Arrange
        var options = new MessageBarOptions();
        var attributes = new Dictionary<string, object> { ["data-test"] = "value" };

        // Act
        options.AdditionalAttributes = attributes;

        // Assert
        Assert.NotNull(options.AdditionalAttributes);
        Assert.Equal("value", options.AdditionalAttributes["data-test"]);
    }
}
