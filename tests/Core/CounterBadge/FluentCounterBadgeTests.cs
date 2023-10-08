using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.CounterBadge;

public class FluentCounterBadgeTests : TestBase
{
    [Inject]
    public GlobalState GlobalState { get; set; } = default!;

    [Fact]
    public void FluentCounterBadge_AttributesDefaultValues()
    {
        // Arrange && Act
        TestContext.Services.AddSingleton(new GlobalState());
        var cut = TestContext.RenderComponent<FluentCounterBadge>(parameters =>
        {
            parameters.Add(p => p.Count, 1);
            parameters.AddChildContent("childcontent");
        });

        // Assert
        cut.Verify();
    }

    [Theory]
    [InlineData(Appearance.Accent)]
    [InlineData(Appearance.Lightweight)]
    [InlineData(Appearance.Neutral)]
    public void FluentCounterBadge_AppearanceAttribute(Appearance appearance)
    {
        // Arrange &&
        TestContext.Services.AddSingleton(new GlobalState());
        var cut = TestContext.RenderComponent<FluentCounterBadge>(parameters =>
        {
            parameters.Add(p => p.Appearance, appearance);
            parameters.Add(p => p.Count, 1);
            parameters.AddChildContent("fluent-button");
        });

        // Assert
        cut.Verify(suffix: appearance.ToString());
    }


    [Fact]
    public void FluentCounterBadge_WithAdditionalCssClass()
    {
        // Arrange && Act
        TestContext.Services.AddSingleton(new GlobalState());
        var cut = TestContext.RenderComponent<FluentCounterBadge>(parameters =>
        {
            parameters.Add(p => p.Class, "additional_class");
            parameters.Add(p => p.Count, 10);
            parameters.AddChildContent("childcontent");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCounterBadge_WithAdditionalStyle()
    {
        // Arrange && Act
        TestContext.Services.AddSingleton(new GlobalState());
        var cut = TestContext.RenderComponent<FluentCounterBadge>(parameters =>
        {
            parameters.Add(p => p.Style, "background-color: red");
            parameters.Add(p => p.Count, 10);
            parameters.AddChildContent("childcontent");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCounterBadge_ShowOverflowAttribute()
    {
        // Arrange && Act
        TestContext.Services.AddSingleton(new GlobalState());

        var cut = TestContext.RenderComponent<FluentCounterBadge>(parameters =>
        {
            parameters.Add(p => p.ShowOverflow, true);
            parameters.Add(p => p.Max, 9);
            parameters.Add(p => p.Count, 10);
            parameters.AddChildContent("childcontent");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCounterBadge_BackgroundColorAndColorAttribute()
    {
        // Arrange && Act
        TestContext.Services.AddSingleton(new GlobalState());
        IRenderedComponent<FluentCounterBadge>? cut = TestContext.RenderComponent<FluentCounterBadge>(parameters =>
        {
            parameters.Add(p => p.BackgroundColor, Color.Accent);
            parameters.Add(p => p.Color, Color.Fill);
            parameters.Add(p => p.Count, 1);
            parameters.AddChildContent("childcontent");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCounterBadge_BackgroundColorNullAndColorSet()
    {
        // Arrange, Act && Assert
        TestContext.Services.AddSingleton(new GlobalState());
        Assert.Throws<ArgumentException>(() => TestContext.RenderComponent<FluentCounterBadge>(parameters =>
        {
            parameters.Add(p => p.BackgroundColor, null);
            parameters.Add(p => p.Color, Color.Fill);
            parameters.Add(p => p.Count, 1);
            parameters.AddChildContent("childcontent");
        }));
    }

    [Fact]
    public void FluentCounterBadge_BackgroundColorSetAndColorNull()
    {
        // Arrange, Act && Assert
        TestContext.Services.AddSingleton(new GlobalState());
        Assert.Throws<ArgumentException>(() => TestContext.RenderComponent<FluentCounterBadge>(parameters =>
        {
            parameters.Add(p => p.BackgroundColor, Color.Accent);
            parameters.Add(p => p.Color, null);
            parameters.Add(p => p.Count, 1);
            parameters.AddChildContent("childcontent");
        }));
    }

    [Fact]
    public void FluentCounterBadge_BackgroundColorCustom()
    {
        // Arrange, Act && Assert
        TestContext.Services.AddSingleton(new GlobalState());
        Assert.Throws<ArgumentException>(() => TestContext.RenderComponent<FluentCounterBadge>(parameters =>
        {
            parameters.Add(p => p.BackgroundColor, Color.Custom);
            parameters.Add(p => p.Color, Color.Custom);
            parameters.Add(p => p.Count, 1);
            parameters.AddChildContent("childcontent");
        }));
    }

    [Fact]
    public void FluentCounterBadge_AppearanceOutline()
    {
        // Arrange, Act && Assert
        TestContext.Services.AddSingleton(new GlobalState());
        Assert.Throws<ArgumentException>(() => TestContext.RenderComponent<FluentCounterBadge>(parameters =>
        {
            parameters.Add(p => p.Appearance, Appearance.Outline);
            parameters.Add(p => p.Count, 1);
            parameters.AddChildContent("childcontent");
        }));
    }

    [Fact]
    public void FluentCounterBadge_ShowZeroAttribute()
    {
        // Arrange && Act
        TestContext.Services.AddSingleton(new GlobalState());
        var cut = TestContext.RenderComponent<FluentCounterBadge>(parameters =>
        {
            parameters.Add(p => p.ShowZero, true);
            parameters.Add(p => p.Count, 10);
            parameters.AddChildContent("childcontent");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCounterBadge_BackgroundColorLightweightLuminanceDark()
    {
        // Arrange && Act
        TestContext.Services.AddSingleton(new GlobalState());

        var cut = TestContext.RenderComponent<FluentCounterBadge>(parameters =>
        {
            parameters.Add(p => p.BackgroundColor, Color.Lightweight);
            parameters.Add(p => p.Color, Color.Fill);
            parameters.Add(p => p.Count, 1);
            parameters.AddChildContent("childcontent");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCounterBadge_BackgroundColorLightweightLuminanceLight()
    {
        // Arrange && Act
        TestContext.Services.AddSingleton(new GlobalState());

        var cut = TestContext.RenderComponent<FluentCounterBadge>(parameters =>
        {
            parameters.Add(p => p.BackgroundColor, Color.Lightweight);
            parameters.Add(p => p.Color, Color.Fill);
            parameters.Add(p => p.Count, 1);
            parameters.AddChildContent("childcontent");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCounterBadge_BackgroundColorErrorLuminanceDark()
    {
        // Arrange && Act
        TestContext.Services.AddSingleton(new GlobalState());

        var cut = TestContext.RenderComponent<FluentCounterBadge>(parameters =>
        {
            parameters.Add(p => p.BackgroundColor, Color.Error);
            parameters.Add(p => p.Color, Color.Fill);
            parameters.Add(p => p.Count, 1);
            parameters.AddChildContent("childcontent");
        });

        // Assert
        cut.Verify();
    }
}
