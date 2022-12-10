using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.FluentAccordion
{
    public class FluentAccordionItem_Render_Should : RenderTestBase
    {
        [Fact]
        public void RenderProperly_WithoutAnyProvided_Content()
        {
            // Arrange
            IRenderedComponent<FluentAccordionItem> cut = TestContext.RenderComponent<FluentAccordionItem>();

            // Assert
            cut.MarkupMatches("<fluent-accordion-item id:ignore>" +
                              "<span slot=\"heading\"></span>" +
                              "</fluent-accordion-item>");
        }

        [Fact]
        public void RenderProperly_WithProvided_Content()
        {
            // Arrange
            IRenderedComponent<FluentAccordionItem> cut = TestContext.RenderComponent<FluentAccordionItem>(
                parameters => parameters.AddChildContent("child content"));

            // Assert
            cut.MarkupMatches("<fluent-accordion-item id:ignore>" +
                              "<span slot=\"heading\"></span>" +
                              "child content" +
                              "</fluent-accordion-item>");
        }

        [Fact]
        public void RenderProperly_WithCustomHeaderValue()
        {
            // Arrange
            IRenderedComponent<FluentAccordionItem> cut = TestContext.RenderComponent<FluentAccordionItem>(
                parameters => parameters.Add(p => p.Heading, "custom heading value"));

            // Assert
            cut.MarkupMatches("<fluent-accordion-item id:ignore>" +
                              "<span slot=\"heading\">custom heading value</span>" +
                              "</fluent-accordion-item>");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void RenderProperly_WithProvidedExpanded_Parameter(bool expanded)
        {
            // Arrange
            IRenderedComponent<FluentAccordionItem> cut = TestContext.RenderComponent<FluentAccordionItem>(
                parameters => parameters.Add(p => p.Expanded, expanded));

            // Assert
            if (expanded)
            {
                cut.MarkupMatches("<fluent-accordion-item id:ignore expanded=\"\">" +
                                  "<span slot=\"heading\"></span>" +
                                  "</fluent-accordion-item>");
            }
            else
            {
                cut.MarkupMatches("<fluent-accordion-item id:ignore>" +
                                  "<span slot=\"heading\"></span>" +
                                  "</fluent-accordion-item>");
            }
        }

        [Fact]
        public void RenderProperly_WithAdditionalAttribute()
        {
            // Arrange
            IRenderedComponent<FluentAccordionItem> cut = TestContext.RenderComponent<FluentAccordionItem>(
                parameters => parameters.AddUnmatched("unknown", "unknowns-value"));

            // Assert
            cut.MarkupMatches("<fluent-accordion-item id:ignore unknown=\"unknowns-value\">" +
                              "<span slot=\"heading\"></span>" +
                              "</fluent-accordion-item>");
        }

        [Fact]
        public void RenderProperly_WithMultipleAdditionalAttributes()
        {
            // Arrange
            IRenderedComponent<FluentAccordionItem> cut = TestContext.RenderComponent<FluentAccordionItem>(
                parameters => parameters
                    .AddUnmatched("unknown1", "unknown1s-value")
                    .AddUnmatched("unknown2", "unknown2s-value"));

            // Assert
            cut.MarkupMatches("<fluent-accordion-item id:ignore " +
                              "unknown1=\"unknown1s-value\"" +
                              "unknown2=\"unknown2s-value\">" +
                              "<span slot=\"heading\"></span>" +
                              "</fluent-accordion-item>");
        }

        [Fact]
        public void RenderProperly_WhenAllParamsAdded_AndAdditionalAttributes_AndContent()
        {
            // Arrange
            IRenderedComponent<FluentAccordionItem> cut = TestContext.RenderComponent<FluentAccordionItem>(
                parameters => parameters
                    .Add(p => p.Expanded, true)
                    .Add(p => p.Heading, "custom heading value")
                    .AddUnmatched("unknown1", "unknown1s-value")
                    .AddUnmatched("unknown2", "unknown2s-value")
                    .AddChildContent("child content"));

            // Assert
            cut.MarkupMatches("<fluent-accordion-item id:ignore " +
                              "expanded=\"\"" +
                              "unknown1=\"unknown1s-value\"" +
                              "unknown2=\"unknown2s-value\">" +
                              "<span slot=\"heading\">custom heading value</span>" +
                              "child content" +
                              "</fluent-accordion-item>");
        }
    }
}