using Bunit;
using FluentAssertions;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Badge;

public partial class FluentBadgeTests : TestContext
{
    private TestContext TestContext => new(); // TODO: To remove and to use the `RenderComponent` inherited method.

    [Fact]
    public void FluentBadge_DefaultAttributes()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentBadge>(parameters =>
        {
            parameters.AddChildContent("childcontent");
        });

        // Assert
        cut.Verify();
    }

    [Theory]
    [InlineData(Appearance.Neutral)]
    [InlineData(Appearance.Accent)]
    [InlineData(Appearance.Lightweight)]
    [InlineData(Appearance.Hypertext)]
    public void FluentBadge_AppearanceAttribute(Appearance appearance)
    {
        // Arrange && Act
        IRenderedComponent<FluentBadge>? cut = null;
        Action action = () =>
        {
            cut = TestContext.RenderComponent<FluentBadge>(parameters =>
            {
                parameters.Add(p => p.Appearance, appearance);
                parameters.AddChildContent("childcontent");
            });
        };

        // Assert
        if (appearance == Appearance.Hypertext)
        {
            action.Should().Throw<ArgumentException>();
        }
        else
        {
            action.Should().NotThrow();
            cut!.Verify(suffix: appearance.ToString());
        }
    }

    [Theory]
    [InlineData(Appearance.Filled)]
    [InlineData(Appearance.Hypertext)]
    [InlineData(Appearance.Outline)]
    [InlineData(Appearance.Stealth)]
    public void ThrowArgumentException_When_AppearanceAttributeValue_IsInvalid(Appearance appearance)
    {
        // Arrange && Act
        Action action = () =>
        {
            TestContext.RenderComponent<FluentBadge>(parameters =>
            {
                parameters.Add(p => p.Appearance, appearance);
                parameters.AddChildContent("childcontent");
            });
        };

        // Assert
        action.Should().ThrowExactly<ArgumentException>();
    }

    [Fact]
    public void FluentBadge_FillAttribute()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentBadge>(parameters =>
        {
            parameters.Add(p => p.Fill, "red");
            parameters.AddChildContent("childcontent");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentBadge_ColorAttribute()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentBadge>(parameters =>
        {
            parameters.Add(p => p.Color, "red");
            parameters.AddChildContent("childcontent");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentBadge_CircularAttribute()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentBadge>(parameters =>
        {
            parameters.Add(p => p.Circular, true);
            parameters.AddChildContent("childcontent");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentBadge_WithAdditionalCssClass()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentBadge>(parameters =>
        {
            parameters.Add(p => p.Class, "additional_class");
            parameters.AddChildContent("childcontent");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentBadge_WithAdditionalStyle()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentBadge>(parameters =>
        {
            parameters.Add(p => p.Style, "background-color:red; width:100px");
            parameters.AddChildContent("childcontent");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentBadge_WithAnAdditionalAttribute()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentBadge>(parameters =>
        {
            parameters.AddUnmatched("additional-attribute-name", "additional-attribute-value");
            parameters.AddChildContent("childcontent");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentBadge_WithMultipleAdditionalAttributes()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentBadge>(parameters =>
        {
            parameters.AddUnmatched("additional-attribute1-name", "additional-attribute1-value");
            parameters.AddUnmatched("additional-attribute2-name", "additional-attribute2-value");
            parameters.AddChildContent("childcontent");
        });

        // Assert
        cut.Verify();
    }
}
