using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.FluentAccordion
{
    public class FluentAccordion_Render_Should : RenderTestBase
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
        public void RenderProperly_WithMultiSelect_WhenExpandMode_IsNotSpecified()
        {
            // Arrange & Act
            IRenderedComponent<FluentUI.FluentAccordion> cut = TestContext.RenderComponent<FluentUI.FluentAccordion>();

            // Assert
            cut.MarkupMatches("<fluent-accordion expand-mode=\"multi\"></fluent-accordion>");
        }

        [Theory]
        [InlineData(AccordionExpandMode.Multi)]
        [InlineData(AccordionExpandMode.Single)]
        public void RenderProperly_When_ExpandMode_IsSpecified(AccordionExpandMode accordionExpandMode)
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
            cut.MarkupMatches("<fluent-accordion " +
                              "style=\"background-color: grey\" " +
                              "expand-mode=\"multi\"></fluent-accordion>");
        }
    }
}