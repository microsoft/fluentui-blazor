using Bunit;
using FluentAssertions;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Anchor;

public partial class FluentAnchorTests : TestContext
{
    private const string FluentAnchorRazorJs = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/Anchor/FluentAnchor.razor.js";

    public FluentAnchorTests()
    {
        JSInterop.SetupModule(FluentAnchorRazorJs);
    }

    [Fact]
    public void FluentAnchor_AttributesDefaultValues()
    {
        // Arrange
        var cut = RenderComponent<FluentAnchor>(parameters =>
        {
            parameters.AddChildContent("click me!");
        });

        // Assert
        cut.Verify();
    }

    [Theory]
    [InlineData("")]
    [InlineData("something")]
    public void FluentAnchor_DownloadAttribute(string download)
    {
        // Arrange && Act
        var cut = RenderComponent<FluentAnchor>(parameters =>
        {
            parameters.Add(p => p.Href, "https://fast.design");
            parameters.Add(p => p.Download, download);
            parameters.AddChildContent("click me!");
        });

        // Assert
        cut.Verify(suffix: download);
    }

    [Theory]
    [InlineData("https://fast.design", "file1")]
    [InlineData("/something/something", "file2")]
    [InlineData("#/something/something", "file3")]
    public void FluentAnchor_HrefAttribute(string url, string suffix)
    {
        // Arrange && Act
        var cut = RenderComponent<FluentAnchor>(parameters =>
        {
            parameters.Add(p => p.Href, url);
            parameters.AddChildContent("click me!");
        });

        // Assert
        cut.Verify(suffix: suffix);
    }

    [Theory]
    [InlineData("en-GB")]
    [InlineData("fr")]
    public void FluentAnchor_HrefLangAttribute(string lang)
    {
        // Arrange
        var cut = RenderComponent<FluentAnchor>(
            parameters => parameters
                .Add(p => p.Hreflang, lang)
                .Add(p => p.Href, "https://fast.design")
                .AddChildContent("click me!"));

        // Assert
        cut.Verify(suffix: lang);
    }

    [Theory]
    [InlineData("https://fast.design", "file1")]
    [InlineData("https://fast.design https://google.com", "file2")]
    public void FluentAnchor_PingAttribute(string ping, string suffix)
    {
        // Arrange
        var cut = RenderComponent<FluentAnchor>(
            parameters => parameters
                .Add(p => p.Ping, ping)
                .Add(p => p.Href, "https://fast.design")
                .AddChildContent("click me!"));

        // Assert
        cut.Verify(suffix: suffix);
    }

    [Theory]
    [InlineData("no-referrer")]
    [InlineData("no-referrer-when-downgrade")]
    public void FluentAnchor_ReferrerPolicyAttribute(string referrerPolicy)
    {
        // Arrange
        var cut = RenderComponent<FluentAnchor>(
            parameters => parameters
                .Add(p => p.Referrerpolicy, referrerPolicy)
                .Add(p => p.Href, "https://fast.design")
                .AddChildContent("click me!"));

        // Assert
        cut.Verify(suffix: referrerPolicy);
    }

    [Theory]
    [InlineData("alternate")]
    [InlineData("bookmark")]
    public void FluentAnchor_RelAttribute(string rel)
    {
        // Arrange
        var cut = RenderComponent<FluentAnchor>(
            parameters => parameters
                .Add(p => p.Rel, rel)
                .Add(p => p.Href, "https://fast.design")
                .AddChildContent("click me!"));

        // Assert
        cut.Verify(suffix: rel);
    }

    [Theory]
    [InlineData("_blank")]
    [InlineData("_self")]
    [InlineData("_parent")]
    [InlineData("_top")]
    [InlineData("invalid")]
    public void FluentAnchor_TargetAttribute(string target)
    {
        // Arrange
        IRenderedComponent<FluentAnchor>? cut = null;
        Action action = () =>
        {
            cut = RenderComponent<FluentAnchor>(
                parameters => parameters
                    .Add(p => p.Target, target)
                    .Add(p => p.Href, "https://fast.design")
                    .AddChildContent("click me!"));
        };

        // Assert
        if (target == "invalid")
        {
            action.Should().Throw<ArgumentException>();
        }
        else
        {
            action.Should().NotThrow();
            cut!.Verify(suffix: target);
        }
    }

    [Theory]
    [InlineData("image/png", "png")]
    [InlineData("application/pdf", "pdf")]
    public void FluentAnchor_TypeAttribute(string type, string suffix)
    {
        // Arrange
        var cut = RenderComponent<FluentAnchor>(
            parameters => parameters
                .Add(p => p.Type, type)
                .Add(p => p.Href, "https://fast.design")
                .AddChildContent("click me!"));

        // Assert
        cut.Verify(suffix: suffix);
    }

    [Theory]
    [InlineData(Appearance.Accent)]
    [InlineData(Appearance.Filled)]
    [InlineData(Appearance.Hypertext)]
    [InlineData(Appearance.Outline)]
    [InlineData(Appearance.Lightweight)]
    [InlineData(Appearance.Neutral)]
    [InlineData(Appearance.Stealth)]
    public void FluentAnchor_AppearanceAttribute(Appearance appearance)
    {
        // Arrange
        var cut = RenderComponent<FluentAnchor>(
            parameters => parameters
                .Add(p => p.Appearance, appearance)
                .Add(p => p.Href, "https://fast.design")
                .AddChildContent("click me!"));

        // Assert
        cut.Verify(suffix: appearance.ToString());
    }

    [Fact]
    public void FluentAnchor_Without_ChildContent()
    {
        // Arrange
        var cut = RenderComponent<FluentAnchor>();

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentAnchor_WithAdditionalCSSClass()
    {
        // Arrange & Act
        var cut = RenderComponent<FluentAnchor>(
            parameters => parameters
                .Add(p => p.Href, "https://fast.design")
                .Add(p => p.Class, "additional-class")
                .AddChildContent("click me!"));

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentAnchor_WithAdditionalStyle()
    {
        // Arrange & Act
        var cut = RenderComponent<FluentAnchor>(
            parameters => parameters
                .Add(p => p.Href, "https://fast.design")
                .Add(p => p.Style, "background-color: black;")
                .AddChildContent("click me!"));

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentAnchor_WithASingleAdditionalAttribute()
    {
        // Arrange & Act
        var cut = RenderComponent<FluentAnchor>(
            parameters => parameters
                .Add(p => p.Href, "https://fast.design")
                .AddUnmatched("additional", "additional-value")
                .AddChildContent("click me!"));

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentAnchor_WithMultipleAdditionalAttributes()
    {
        // Arrange & Act
        var cut = RenderComponent<FluentAnchor>(
            parameters => parameters
                .Add(p => p.Href, "https://fast.design")
                .AddUnmatched("additional1", "additional1-value")
                .AddUnmatched("additional2", "additional2-value")
                .AddChildContent("click me!"));

        // Assert
        cut.Verify();
    }
}
