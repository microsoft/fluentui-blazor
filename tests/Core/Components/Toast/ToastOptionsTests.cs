// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Toast;

public class ToastOptionsTests
{
    [Fact]
    public void ToastOptions_DefaultValues()
    {
        // Act
        var options = new ToastOptions();

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
        Assert.Null(options.Lifetime);
        Assert.Null(options.Position);
        Assert.Null(options.VerticalOffset);
        Assert.Null(options.HorizontalOffset);
        Assert.Null(options.Inverted);
        Assert.Null(options.Intent);
        Assert.Null(options.Politeness);
        Assert.Null(options.Title);
        Assert.Null(options.Message);
        Assert.Null(options.Subtitle);
        Assert.Null(options.PauseOnHover);
        Assert.Null(options.PauseOnWindowBlur);
        Assert.Null(options.AllowDismiss);
        Assert.Null(options.Icon);
        Assert.Null(options.Width);
        Assert.Null(options.OnStatusChange);
        Assert.NotNull(options.DismissAction);
        Assert.NotNull(options.QuickAction1);
        Assert.NotNull(options.QuickAction2);
        Assert.Equal(ToastResultTiming.Closed, options.ResultTiming);
    }

    [Fact]
    public void ToastOptions_FactoryCtor_InvokesFactory()
    {
        // Act
        var options = new ToastOptions(o =>
        {
            o.Title = "Hello";
            o.Message = "World";
            o.Intent = ToastIntent.Success;
        });

        // Assert
        Assert.Equal("Hello", options.Title);
        Assert.Equal("World", options.Message);
        Assert.Equal(ToastIntent.Success, options.Intent);
    }

    [Fact]
    public void ToastOptions_Properties_CanBeSet()
    {
        // Act
        var options = new ToastOptions
        {
            Id = "id1",
            Class = "my-class",
            Style = "color: red;",
            Data = 42,
            Lifetime = TimeSpan.FromSeconds(3),
            Position = ToastPosition.TopEnd,
            VerticalOffset = 16,
            HorizontalOffset = 20,
            Inverted = true,
            Intent = ToastIntent.Warning,
            Politeness = ToastPoliteness.Assertive,
            Title = "Title",
            Message = "Message",
            Subtitle = "Subtitle",
            PauseOnHover = true,
            PauseOnWindowBlur = true,
            AllowDismiss = true,
            Width = "300px",
            ResultTiming = ToastResultTiming.Visible,
        };

        // Assert
        Assert.Equal("id1", options.Id);
        Assert.Equal("my-class", options.Class);
        Assert.Equal("color: red;", options.Style);
        Assert.Equal(42, options.Data);
        Assert.Equal(TimeSpan.FromSeconds(3), options.Lifetime);
        Assert.Equal(ToastPosition.TopEnd, options.Position);
        Assert.Equal(16, options.VerticalOffset);
        Assert.Equal(20, options.HorizontalOffset);
        Assert.True(options.Inverted);
        Assert.Equal(ToastIntent.Warning, options.Intent);
        Assert.Equal(ToastPoliteness.Assertive, options.Politeness);
        Assert.Equal("Title", options.Title);
        Assert.Equal("Message", options.Message);
        Assert.Equal("Subtitle", options.Subtitle);
        Assert.True(options.PauseOnHover);
        Assert.True(options.PauseOnWindowBlur);
        Assert.True(options.AllowDismiss);
        Assert.Equal("300px", options.Width);
        Assert.Equal(ToastResultTiming.Visible, options.ResultTiming);
    }

    [Fact]
    public void ToastOptions_ClassValue_IncludesClassAndSpacing()
    {
        // Arrange
        var options = new ToastOptions
        {
            Class = "my-class",
            Margin = "10px",
            Padding = "20px",
        };

        // Act
        var classValue = options.ClassValue;

        // Assert
        Assert.NotNull(classValue);
        Assert.Contains("my-class", classValue);
    }

    [Fact]
    public void ToastOptions_StyleValue_IncludesMarginAndPadding()
    {
        // Arrange
        var options = new ToastOptions
        {
            Style = "color: red;",
            Margin = "10px",
            Padding = "20px",
        };

        // Act
        var styleValue = options.StyleValue;

        // Assert
        Assert.NotNull(styleValue);
        Assert.Contains("color: red", styleValue);
        Assert.Contains("margin: 10px", styleValue);
        Assert.Contains("padding: 20px", styleValue);
    }

    [Fact]
    public void ToastOptions_SetParameters_Accepts()
    {
        // Arrange
        var options = new ToastOptions();

        // Act
        options.Parameters["Name"] = "John";

        // Assert
        Assert.Single(options.Parameters);
        Assert.Equal("John", options.Parameters["Name"]);
    }

    [Fact]
    public void ToastOptions_Actions_AreIndependentInstances()
    {
        // Arrange
        var options = new ToastOptions();

        // Act
        options.DismissAction.Label = "Dismiss";
        options.QuickAction1.Label = "One";
        options.QuickAction2.Label = "Two";

        // Assert
        Assert.Equal("Dismiss", options.DismissAction.Label);
        Assert.Equal("One", options.QuickAction1.Label);
        Assert.Equal("Two", options.QuickAction2.Label);
    }
}
