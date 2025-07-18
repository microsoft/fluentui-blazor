// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.CounterBadge;

public class FluentCounterBadgeTests : TestBase
{
    [Inject]
    public GlobalState GlobalState { get; set; } = new GlobalState();

    public FluentCounterBadgeTests()
    {
        TestContext.Services.AddSingleton(GlobalState);
    }

    [Fact]
    public void FluentCounterBadge_AttributesDefaultValues()
    {
        // Arrange && Act
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
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentCounterBadge>(parameters =>
        {
            parameters.Add(p => p.Appearance, appearance);
            parameters.Add(p => p.Count, 1);
            parameters.AddChildContent("fluent-button");
        });

        // Assert
        cut.Verify(suffix: appearance.ToString());
    }

    [Theory]
    [InlineData(Appearance.Hypertext)]
    public void FluentCounterBadge_AppearanceAttributeInvalid(Appearance appearance)
    {
        // Arrange && Act
        Assert.Throws<ArgumentException>(() => TestContext.RenderComponent<FluentCounterBadge>(parameters =>
        {
            parameters.Add(p => p.Appearance, appearance);
            parameters.Add(p => p.Count, 1);
            parameters.AddChildContent("fluent-button");
        }));

        // Assert
        //Assert.Throws<ArgumentException>("Appearance", () => "CounterBadge Appearance needs to be one of Accent, Lightweight or Neutral.");

    }

    [Fact]
    public void FluentCounterBadge_WithAdditionalCssClass()
    {
        // Arrange && Act
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
        var cut = TestContext.RenderComponent<FluentCounterBadge>(parameters =>
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
    public void FluentCounterBadge_BackgroundLightWeightAndDarkMode()
    {
        // Arrange && Act
        GlobalState.SetLuminance(StandardLuminance.DarkMode);

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
    public void FluentCounterBadge_BackgroundLightWeightAndLightMode()
    {
        // Arrange && Act
        GlobalState.SetLuminance(StandardLuminance.LightMode);

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
    public void FluentCounterBadge_BackgroundColorNullAndColorSet()
    {
        // Arrange, Act && Assert
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
        Assert.Throws<ArgumentException>(() => TestContext.RenderComponent<FluentCounterBadge>(parameters =>
        {
            parameters.Add(p => p.Appearance, Appearance.Outline);
            parameters.Add(p => p.Count, 1);
            parameters.AddChildContent("childcontent");
        }));
    }

    [Fact]
    public void FluentCounterBadge_ShowZeroAttributeWithCount0()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentCounterBadge>(parameters =>
        {
#pragma warning disable CS0618 // Type or member is obsolete
            parameters.Add(p => p.ShowZero, true);
#pragma warning restore CS0618 // Type or member is obsolete
            parameters.Add(p => p.Count, 0);
            parameters.AddChildContent("childcontent");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCounterBadge_ShowZeroAttributeCountNot0()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentCounterBadge>(parameters =>
        {
#pragma warning disable CS0618 // Type or member is obsolete
            parameters.Add(p => p.ShowZero, true);
#pragma warning restore CS0618 // Type or member is obsolete
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
        GlobalState.Luminance = StandardLuminance.DarkMode;

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

    [Fact]
    public void FluentCounterBadge_Dispose()
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentCounterBadge>(parameters =>
        {
            parameters.Add(p => p.Count, 1);
            parameters.AddChildContent("childcontent");
        });

        TestContext.DisposeComponents();

        // Assert
        Assert.True(cut.IsDisposed);
    }

    [Fact]
    public void FluentCounterBadge_NoCount()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentCounterBadge>(parameters =>
        {
            parameters.AddChildContent("childcontent");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCounterBadge_BadgeContent()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentCounterBadge>(parameters =>
        {
#pragma warning disable CS0618 // Type or member is obsolete
            parameters.Add(p => p.BadgeContent, "badgecontent");
#pragma warning restore CS0618 // Type or member is obsolete
            parameters.Add(p => p.Count, 1);
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCounterBadge_BadgeContentWithMax()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentCounterBadge>(parameters =>
        {
#pragma warning disable CS0618 // Type or member is obsolete
            parameters.Add(p => p.BadgeContent, "badgecontent");
#pragma warning restore CS0618 // Type or member is obsolete
            parameters.Add(p => p.Max, 2);
            parameters.Add(p => p.Count, 3);
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCounterBadge_BadgeContentNoCount()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentCounterBadge>(parameters =>
        {
#pragma warning disable CS0618 // Type or member is obsolete
            parameters.Add(p => p.BadgeContent, "badgecontent");
#pragma warning restore CS0618 // Type or member is obsolete
            parameters.Add(p => p.Count, 0);
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCounterBadge_BadgeContentNullWithCount()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentCounterBadge>(parameters =>
        {
#pragma warning disable CS0618 // Type or member is obsolete
            parameters.Add(p => p.BadgeContent, (RenderFragment?)null);
#pragma warning restore CS0618 // Type or member is obsolete
            parameters.Add(p => p.Count, 1);
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCounterBadge_Dot()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentCounterBadge>(parameters =>
        {
            parameters.Add(p => p.Dot, true);
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCounterBadge_DotWithCount()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentCounterBadge>(parameters =>
        {
            parameters.Add(p => p.Dot, true);
            parameters.Add(p => p.Count, 1);
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCounterBadge_DotWithPositioning()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentCounterBadge>(parameters =>
        {
            parameters.Add(p => p.Dot, true);
            parameters.Add(p => p.Count, 1);
            parameters.Add(parameters => parameters.HorizontalPosition, -25);
            parameters.Add(parameters => parameters.VerticalPosition, -25);
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCounterBadge_WithPositioning()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentCounterBadge>(parameters =>
        {
            parameters.Add(p => p.Count, 1);
            parameters.Add(parameters => parameters.HorizontalPosition, -25);
            parameters.Add(parameters => parameters.VerticalPosition, -25);
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCounterBadge_WithAdditionalAttributes()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentCounterBadge>(parameters =>
        {
            parameters.Add(p => p.Count, 1);
            parameters.Add(p => p.AdditionalAttributes, new Dictionary<string, object>
            {
                { "data-test", "test" },
            });
        });

        // Assert
        cut.Verify();
    }
}
