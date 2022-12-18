using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.FluentAccordion
{
    public class FluentAccordionShould : TestBase
    {
        [Fact]
        public void RenderProperly_When_ChildContent_IsNull()
        {
            // Arrange & Act
            IRenderedComponent<FluentUI.FluentAccordion> cut = TestContext.RenderComponent<FluentUI.FluentAccordion>();

            // Assert
            cut.MarkupMatches("<fluent-accordion expand-mode=\"multi\"></fluent-accordion>");
        }

        [Fact]
        public void RenderProperly_TheDefaultExpandMode_WhenExpandMode_IsNotSpecified()
        {
            // Arrange & Act
            IRenderedComponent<FluentUI.FluentAccordion> cut = TestContext.RenderComponent<FluentUI.FluentAccordion>();

            // Assert
            cut.MarkupMatches("<fluent-accordion expand-mode=\"multi\"></fluent-accordion>");
        }

        [Theory]
        [InlineData(AccordionExpandMode.Multi)]
        [InlineData(AccordionExpandMode.Single)]
        public void RenderProperly_WhenExpandMode_IsSpecified(AccordionExpandMode accordionExpandMode)
        {
            // Arrange & Act
            IRenderedComponent<FluentUI.FluentAccordion> cut = TestContext.RenderComponent<FluentUI.FluentAccordion>(
                parameters => parameters.Add(p => p.ExpandMode, accordionExpandMode));

            // Assert
            if (accordionExpandMode == AccordionExpandMode.Multi)
            {
                cut.MarkupMatches("<fluent-accordion expand-mode=\"multi\"></fluent-accordion>");
            }
            else
            {
                cut.MarkupMatches("<fluent-accordion expand-mode=\"single\"></fluent-accordion>");
            }
        }

        [Fact]
        public void RenderProperly_WhenAdditionalCSSClass_IsProvided()
        {
            // Arrange & Act
            IRenderedComponent<FluentUI.FluentAccordion> cut = TestContext.RenderComponent<FluentUI.FluentAccordion>(
                parameters => parameters.Add(p => p.Class, "additional-class"));

            // Assert
            cut.MarkupMatches("<fluent-accordion class=\"additional-class\" expand-mode=\"multi\"></fluent-accordion>");
        }

        [Fact]
        public void RenderProperly_WhenAdditionalStyle_IsProvided()
        {
            // Arrange & Act
            IRenderedComponent<FluentUI.FluentAccordion> cut = TestContext.RenderComponent<FluentUI.FluentAccordion>(
                parameters => parameters.Add(p => p.Style, "background-color: grey"));

            // Assert
            cut.MarkupMatches("<fluent-accordion style=\"background-color: grey\" expand-mode=\"multi\">" +
                              "</fluent-accordion>");
        }

        [Fact]
        public void RenderProperly_WhenAdditionalParameters_AreAdded()
        {
            // Arrange & Act
            IRenderedComponent<FluentUI.FluentAccordion> cut = TestContext.RenderComponent<FluentUI.FluentAccordion>(
                parameters => parameters.AddUnmatched("unmatched1", "unmatched1-value")
                    .AddUnmatched("unmatched2", "unmatched2-value"));

            // Assert
            cut.MarkupMatches("<fluent-accordion expand-mode=\"multi\"" +
                              "unmatched1=\"unmatched1-value\" " +
                              "unmatched2=\"unmatched2-value\">" +
                              "</fluent-accordion>");
        }

        [Fact]
        public void RenderProperly_WhenExpandedModeIsSingle_AndMultipleItemAreExpanded_ByDefault()
        {
            // Arrange & Act
            IRenderedComponent<FluentUI.FluentAccordion> cut = TestContext.RenderComponent<FluentUI.FluentAccordion>(
                parameters => parameters
                    .Add(p => p.ExpandMode, AccordionExpandMode.Single)
                    .AddChildContent<FluentAccordionItem>(itemParams => itemParams.Add(p => p.Expanded, true))
                    .AddChildContent<FluentAccordionItem>(itemParams => itemParams.Add(p => p.Expanded, true)));
            
            // Assert
            cut.MarkupMatches("<fluent-accordion expand-mode=\"single\">" +
                              "<fluent-accordion-item id:ignore expanded=\"\">" +
                              "<span slot=\"heading\"></span>" +
                              "</fluent-accordion-item>" +
                              "<fluent-accordion-item id:ignore expanded=\"\">" +
                              "<span slot=\"heading\"></span>" +
                              "</fluent-accordion-item>" +
                              "</fluent-accordion>");
        }
    }
}