using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.Anchor
{
    public class FluentAnchorRenderShould : TestBase
    {
        [Fact]
        public void RenderProperly_AttributesDefaultValues()
        {
            // Arrange
            TestContext.JSInterop.SetupModule(
                "./_content/Microsoft.Fast.Components.FluentUI/Components/Anchor/FluentAnchor.razor.js");
            IRenderedComponent<FluentUI.FluentAnchor> cut = TestContext.RenderComponent<FluentUI.FluentAnchor>(
                parameters => parameters
                    .AddChildContent("click me!"));

            // Assert
            cut.MarkupMatches("<fluent-anchor " +
                              "appearance=\"neutral\"> " +
                              "click me!" +
                              "</fluent-anchor>");
        }
        
        [Theory]
        [InlineData("")]
        [InlineData("something")]
        public void RenderProperly_DownloadAttribute(string download)
        {
            // Arrange && Act
            TestContext.JSInterop.SetupModule(
                "./_content/Microsoft.Fast.Components.FluentUI/Components/Anchor/FluentAnchor.razor.js");
            IRenderedComponent<FluentUI.FluentAnchor> cut = TestContext.RenderComponent<FluentUI.FluentAnchor>(
                parameters => parameters
                    .Add(p => p.Href, "https://fast.design")
                    .Add(p => p.Download, download)
                    .AddChildContent("click me!"));

            // Assert
            cut.MarkupMatches("<fluent-anchor " +
                              "appearance=\"neutral\" " +
                              "href=\"https://fast.design\" " +
                              $"download=\"{download}\">" +
                              "click me!" +
                              "</fluent-anchor>");
        }

        [Theory]
        [InlineData("https://fast.design")]
        [InlineData("/something/something")]
        [InlineData("#/something/something")]
        public void RenderProperly_HrefAttribute(string url)
        {
            // Arrange && Act
            TestContext.JSInterop.SetupModule(
                "./_content/Microsoft.Fast.Components.FluentUI/Components/Anchor/FluentAnchor.razor.js");
            IRenderedComponent<FluentUI.FluentAnchor> cut = TestContext.RenderComponent<FluentUI.FluentAnchor>(
                parameters => parameters
                    .Add(p => p.Href, url)
                    .AddChildContent("click me!"));

            // Assert
            cut.MarkupMatches("<fluent-anchor " +
                              "appearance=\"neutral\" " +
                              $"href=\"{url}\">" +
                              "click me!" +
                              "</fluent-anchor>");
        }

        [Theory]
        [InlineData("en-GB")]
        [InlineData("fr")]
        public void RenderProperly_HrefLangAttribute(string lang)
        {
            // Arrange
            TestContext.JSInterop.SetupModule(
                "./_content/Microsoft.Fast.Components.FluentUI/Components/Anchor/FluentAnchor.razor.js");
            IRenderedComponent<FluentUI.FluentAnchor> cut = TestContext.RenderComponent<FluentUI.FluentAnchor>(
                parameters => parameters
                    .Add(p => p.Hreflang, lang)
                    .Add(p => p.Href, "https://fast.design")
                    .AddChildContent("click me!"));

            // Assert
            cut.MarkupMatches("<fluent-anchor " +
                              "href=\"https://fast.design\" " +
                              "appearance=\"neutral\" " +
                              $"hreflang=\"{lang}\">" +
                              "click me!" +
                              "</fluent-anchor>");
        }

        [Theory]
        [InlineData("https://fast.design")]
        [InlineData("https://fast.design https://google.com")]
        public void RenderProperly_PingAttribute(string ping)
        {
            // Arrange
            TestContext.JSInterop.SetupModule(
                "./_content/Microsoft.Fast.Components.FluentUI/Components/Anchor/FluentAnchor.razor.js");
            IRenderedComponent<FluentUI.FluentAnchor> cut = TestContext.RenderComponent<FluentUI.FluentAnchor>(
                parameters => parameters
                    .Add(p => p.Ping, ping)
                    .Add(p => p.Href, "https://fast.design")
                    .AddChildContent("click me!"));

            // Assert
            cut.MarkupMatches("<fluent-anchor " +
                              "href=\"https://fast.design\" " +
                              "appearance=\"neutral\" " +
                              $"ping=\"{ping}\">" +
                              "click me!" +
                              "</fluent-anchor>");
        }

        [Theory]
        [InlineData("no-referrer")]
        [InlineData("no-referrer-when-downgrade")]
        public void RenderProperly_ReferrerPolicyAttribute(string referrerPolicy)
        {
            // Arrange
            TestContext.JSInterop.SetupModule(
                "./_content/Microsoft.Fast.Components.FluentUI/Components/Anchor/FluentAnchor.razor.js");
            IRenderedComponent<FluentUI.FluentAnchor> cut = TestContext.RenderComponent<FluentUI.FluentAnchor>(
                parameters => parameters
                    .Add(p => p.Referrerpolicy, referrerPolicy)
                    .Add(p => p.Href, "https://fast.design")
                    .AddChildContent("click me!"));

            // Assert
            cut.MarkupMatches("<fluent-anchor " +
                              "href=\"https://fast.design\" " +
                              "appearance=\"neutral\" " +
                              $"referrerPolicy=\"{referrerPolicy}\">" +
                              "click me!" +
                              "</fluent-anchor>");
        }

        [Theory]
        [InlineData("alternate")]
        [InlineData("bookmark")]
        public void RenderProperly_RelAttribute(string rel)
        {
            // Arrange
            TestContext.JSInterop.SetupModule(
                "./_content/Microsoft.Fast.Components.FluentUI/Components/Anchor/FluentAnchor.razor.js");
            IRenderedComponent<FluentUI.FluentAnchor> cut = TestContext.RenderComponent<FluentUI.FluentAnchor>(
                parameters => parameters
                    .Add(p => p.Rel, rel)
                    .Add(p => p.Href, "https://fast.design")
                    .AddChildContent("click me!"));

            // Assert
            cut.MarkupMatches("<fluent-anchor " +
                              "href=\"https://fast.design\" " +
                              "appearance=\"neutral\" " +
                              $"rel=\"{rel}\">" +
                              "click me!" +
                              "</fluent-anchor>");
        }

        [Theory]
        [InlineData("_blank")]
        [InlineData("_self")]
        [InlineData("_parent")]
        [InlineData("_top")]
        public void RenderProperly_TargetAttribute(string target)
        {
            // Arrange
            TestContext.JSInterop.SetupModule(
                "./_content/Microsoft.Fast.Components.FluentUI/Components/Anchor/FluentAnchor.razor.js");
            IRenderedComponent<FluentUI.FluentAnchor> cut = TestContext.RenderComponent<FluentUI.FluentAnchor>(
                parameters => parameters
                    .Add(p => p.Target, target)
                    .Add(p => p.Href, "https://fast.design")
                    .AddChildContent("click me!"));

            // Assert
            cut.MarkupMatches("<fluent-anchor " +
                              "href=\"https://fast.design\" " +
                              "appearance=\"neutral\" " +
                              $"target=\"{target}\">" +
                              "click me!" +
                              "</fluent-anchor>");
        }

        [Theory]
        [InlineData("image/png")]
        [InlineData("application/pdf")]
        public void RenderProperly_TypeAttribute(string type)
        {
            // Arrange
            TestContext.JSInterop.SetupModule(
                "./_content/Microsoft.Fast.Components.FluentUI/Components/Anchor/FluentAnchor.razor.js");
            IRenderedComponent<FluentUI.FluentAnchor> cut = TestContext.RenderComponent<FluentUI.FluentAnchor>(
                parameters => parameters
                    .Add(p => p.Type, type)
                    .Add(p => p.Href, "https://fast.design")
                    .AddChildContent("click me!"));

            // Assert
            cut.MarkupMatches("<fluent-anchor " +
                              "href=\"https://fast.design\" " +
                              "appearance=\"neutral\" " +
                              $"type=\"{type}\"> " +
                              "click me!" +
                              "</fluent-anchor>");
        }

        [Theory]
        [InlineData(Appearance.Accent)]
        [InlineData(Appearance.Filled)]
        [InlineData(Appearance.Hypertext)]
        [InlineData(Appearance.Outline)]
        [InlineData(Appearance.Lightweight)]
        [InlineData(Appearance.Neutral)]
        [InlineData(Appearance.Stealth)]
        public void RenderProperly_AppearanceAttribute(Appearance appearance)
        {
            // Arrange
            TestContext.JSInterop.SetupModule(
                "./_content/Microsoft.Fast.Components.FluentUI/Components/Anchor/FluentAnchor.razor.js");
            IRenderedComponent<FluentUI.FluentAnchor> cut = TestContext.RenderComponent<FluentUI.FluentAnchor>(
                parameters => parameters
                    .Add(p => p.Appearance, appearance)
                    .Add(p => p.Href, "https://fast.design")
                    .AddChildContent("click me!"));

            // Assert
            cut.MarkupMatches("<fluent-anchor " +
                              "href=\"https://fast.design\" " +
                              $"appearance=\"{appearance.ToAttributeValue()}\"> " +
                              "click me!" +
                              "</fluent-anchor>");
        }

        [Fact]
        public void RenderProperly_Without_ChildContent()
        {
            // Arrange
            TestContext.JSInterop.SetupModule(
                "./_content/Microsoft.Fast.Components.FluentUI/Components/Anchor/FluentAnchor.razor.js");
            IRenderedComponent<FluentUI.FluentAnchor> cut = TestContext.RenderComponent<FluentUI.FluentAnchor>();

            // Assert
            cut.MarkupMatches("<fluent-anchor appearance=\"neutral\"></fluent-anchor>");
        }

        [Fact]
        public void RenderProperly_WithAdditionalCSSClass()
        {
            // Arrange & Act
            TestContext.JSInterop.SetupModule(
                "./_content/Microsoft.Fast.Components.FluentUI/Components/Anchor/FluentAnchor.razor.js");
            IRenderedComponent<FluentUI.FluentAnchor> cut = TestContext.RenderComponent<FluentUI.FluentAnchor>(
                parameters => parameters
                    .Add(p => p.Href, "https://fast.design")
                    .Add(p => p.Class, "additional-class")
                    .AddChildContent("click me!"));

            // Assert
            cut.MarkupMatches("<fluent-anchor " +
                              "href=\"https://fast.design\" " +
                              "class=\"additional-class\" " +
                              "appearance=\"neutral\"> " +
                              "click me!" +
                              "</fluent-anchor>");
        }

        [Fact]
        public void RenderProperly_WithAdditionalStyle()
        {
            // Arrange & Act
            TestContext.JSInterop.SetupModule(
                "./_content/Microsoft.Fast.Components.FluentUI/Components/Anchor/FluentAnchor.razor.js");
            IRenderedComponent<FluentUI.FluentAnchor> cut = TestContext.RenderComponent<FluentUI.FluentAnchor>(
                parameters => parameters
                    .Add(p => p.Href, "https://fast.design")
                    .Add(p => p.Style, "background-color: black;")
                    .AddChildContent("click me!"));

            // Assert
            cut.MarkupMatches("<fluent-anchor " +
                              "href=\"https://fast.design\" " +
                              "style=\"background-color: black;\" " +
                              "appearance=\"neutral\"> " +
                              "click me!" +
                              "</fluent-anchor>");
        }

        [Fact]
        public void RenderProperly_WithASingleAdditionalAttribute()
        {
            // Arrange & Act
            string additionalAttributeName = "additional";
            string additionalAttributeValue = "additional-value";
            TestContext.JSInterop.SetupModule(
                "./_content/Microsoft.Fast.Components.FluentUI/Components/Anchor/FluentAnchor.razor.js");
            IRenderedComponent<FluentUI.FluentAnchor> cut = TestContext.RenderComponent<FluentUI.FluentAnchor>(
                parameters => parameters
                    .Add(p => p.Href, "https://fast.design")
                    .AddUnmatched(additionalAttributeName, additionalAttributeValue)
                    .AddChildContent("click me!"));

            // Assert
            cut.MarkupMatches("<fluent-anchor " +
                              "href=\"https://fast.design\" " +
                              "appearance=\"neutral\" " +
                              $"{additionalAttributeName}=\"{additionalAttributeValue}\"> " +
                              "click me!" +
                              "</fluent-anchor>");
        }

        [Fact]
        public void RenderProperly_WithMultipleAdditionalAttributes()
        {
            // Arrange & Act
            string additionalAttribute1Name = "additional1";
            string additionalAttribute1Value = "additional1-value";
            string additionalAttribute2Name = "additional2";
            string additionalAttribute2Value = "additional2-value";
            TestContext.JSInterop.SetupModule(
                "./_content/Microsoft.Fast.Components.FluentUI/Components/Anchor/FluentAnchor.razor.js");
            IRenderedComponent<FluentUI.FluentAnchor> cut = TestContext.RenderComponent<FluentUI.FluentAnchor>(
                parameters => parameters
                    .Add(p => p.Href, "https://fast.design")
                    .AddUnmatched(additionalAttribute1Name, additionalAttribute1Value)
                    .AddUnmatched(additionalAttribute2Name, additionalAttribute2Value)
                    .AddChildContent("click me!"));

            // Assert
            cut.MarkupMatches("<fluent-anchor " +
                              "href=\"https://fast.design\" " +
                              "appearance=\"neutral\" " +
                              $"{additionalAttribute1Name}=\"{additionalAttribute1Value}\" " +
                              $"{additionalAttribute2Name}=\"{additionalAttribute2Value}\"> " +
                              "click me!" +
                              "</fluent-anchor>");
        }
    }
}