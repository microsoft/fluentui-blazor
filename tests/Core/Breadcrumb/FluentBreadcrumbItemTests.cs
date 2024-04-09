using Bunit;
using FluentAssertions;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Breadcrumb;

public class FluentBreadcrumbItemTests : TestBase
{
    [Fact]
    public void FluentBreadcrumbItem_DefaultAttributes()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentBreadcrumbItem>(parameters =>
        {
            parameters.AddChildContent("childContent");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentBreadcrumbItem_DownloadAttribute()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentBreadcrumbItem>(parameters =>
        {
            parameters.Add(p => p.Download, "https://example.org");
            parameters.AddChildContent("childContent");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentBreadcrumbItem_HrefAttribute()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentBreadcrumbItem>(parameters =>
        {
            parameters.Add(p => p.Href, "https://example.org");
            parameters.AddChildContent("childContent");
        });

        // Assert
        cut.Verify();
    }

    [Theory]
    [InlineData("en-GB")]
    [InlineData("fr")]
    public void FluentBreadcrumbItem_HrefLangAttribute(string hrefLang)
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentBreadcrumbItem>(parameters =>
        {
            parameters.Add(p => p.Hreflang, hrefLang);
            parameters.AddChildContent("childContent");
        });

        // Assert
        cut.Verify(suffix: hrefLang);
    }

    [Theory]
    [InlineData("https://fast.design", "file1")]
    [InlineData("https://fast.design https://google.com", "file2")]
    public void FluentBreadcrumbItem_PingAttribute(string pingAttribute, string suffix)
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentBreadcrumbItem>(parameters =>
        {
            parameters.Add(p => p.Ping, pingAttribute);
            parameters.AddChildContent("childContent");
        });

        // Assert
        cut.Verify(suffix: suffix);
    }

    [Theory]
    [InlineData("no-referrer")]
    [InlineData("no-referrer-when-downgrade")]
    public void FluentBreadcrumbItem_ReferrerPolicyAttribute(string referrerPolicy)
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentBreadcrumbItem>(parameters =>
        {
            parameters.Add(p => p.Referrerpolicy, referrerPolicy);
            parameters.AddChildContent("childContent");
        });

        // Assert
        cut.Verify(suffix: referrerPolicy);
    }

    [Theory]
    [InlineData("alternate")]
    [InlineData("bookmark")]
    public void FluentBreadcrumbItem_RelAttribute(string relValue)
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentBreadcrumbItem>(parameters =>
        {
            parameters.Add(p => p.Rel, relValue);
            parameters.AddChildContent("childContent");
        });

        // Assert
        cut.Verify(suffix: relValue);
    }

    [Theory]
    [InlineData("_blank")]
    [InlineData("_self")]
    [InlineData("_parent")]
    [InlineData("_top")]
    [InlineData("invalid")]
    public void FluentBreadcrumbItem_TargetAttribute(string targetAttribute)
    {
        // Arrange && Act
        IRenderedComponent<FluentBreadcrumbItem>? cut = null;
        Action action = () =>
        {
            cut = TestContext.RenderComponent<FluentBreadcrumbItem>(parameters =>
            {
                parameters.Add(p => p.Target, targetAttribute);
                parameters.AddChildContent("childContent");
            });
        };

        // Assert
        if (targetAttribute == "invalid")
        {
            action.Should().Throw<ArgumentException>();
        }
        else
        {
            action.Should().NotThrow();
            cut!.Verify(suffix: targetAttribute);
        }
    }

    [Theory]
    [InlineData("image/png")]
    [InlineData("application/pdf")]
    public void FluentBreadcrumbItem_TypeAttribute(string typeAttribute)
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentBreadcrumbItem>(parameters =>
        {
            parameters.Add(p => p.Type, typeAttribute);
            parameters.AddChildContent("childContent");
        });

        // Assert
        cut.Verify(suffix: typeAttribute);
    }

    [Theory]
    [InlineData(Appearance.Accent)]
    [InlineData(Appearance.Filled)]
    [InlineData(Appearance.Hypertext)]
    [InlineData(Appearance.Outline)]
    [InlineData(Appearance.Lightweight)]
    [InlineData(Appearance.Neutral)]
    [InlineData(Appearance.Stealth)]
    public void FluentBreadcrumbItem_AppearanceAttribute(Appearance appearance)
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentBreadcrumbItem>(parameters =>
        {
            parameters.Add(p => p.Appearance, appearance);
            parameters.AddChildContent("childContent");
        });

        // Assert
        cut.Verify(suffix: appearance.ToString());
    }

    [Fact]
    public void FluentBreadcrumbItem_AdditionalCssClass()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentBreadcrumbItem>(parameters =>
        {
            parameters.Add(p => p.Class, "additional-css-class");
            parameters.AddChildContent("childContent");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentBreadcrumbItem_AdditionalStyle()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentBreadcrumbItem>(parameters =>
        {
            parameters.Add(p => p.Style, "background-color: white");
            parameters.AddChildContent("childContent");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentBreadcrumbItem_AdditionalUnmatchedAttribute()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentBreadcrumbItem>(parameters =>
        {
            parameters.AddUnmatched("additional-attribute-name", "additional-attribute-value");
            parameters.AddChildContent("childContent");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentBreadcrumbItem_AdditionalUnmatchedAttributes()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentBreadcrumbItem>(parameters =>
        {
            parameters.AddUnmatched("additional-attribute1-name", "additional-attribute1-value");
            parameters.AddUnmatched("additional-attribute2-name", "additional-attribute2-value");
            parameters.AddChildContent("childContent");
        });

        // Assert
        cut.Verify();
    }
}
