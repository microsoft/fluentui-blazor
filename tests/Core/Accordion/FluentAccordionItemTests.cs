using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Accordion;

public class FluentAccordionItemTests : TestBase
{
    [Fact]
    public void FluentAccordionItem_WithChildContent_IsNull()
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentAccordionItem>();

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentAccordionItem_WithProvided_Content()
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentAccordionItem>(parameters =>
        {
            parameters.AddChildContent("child content");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentAccordionItem_WithCustomHeaderValue()
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentAccordionItem>(parameters =>
        {
            parameters.Add(p => p.Heading, "custom heading value");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentAccordionItem_WithHeadingTemplateAndHeading_IsProvidedBoth()
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentAccordionItem>(parameters =>
        {
            parameters.Add(p => p.HeadingTemplate, context =>
            {
                context.AddContent(0, "custom heading content");
            });

            parameters.Add(p => p.Heading, "Heading Value First");
        });

        // Assert
        cut.Verify();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void FluentAccordionItem_WithProvidedExpanded_Parameter(bool expanded)
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentAccordionItem>(parameters =>
        {
            parameters.Add(p => p.Expanded, expanded);
        });

        // Assert
        cut.Verify(suffix: expanded.ToString());
    }

    [Fact]
    public void FluentAccordionItem_WithAnAdditionalAttribute()
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentAccordionItem>(parameters =>
        {
            parameters.AddUnmatched("unknown", "unknowns-value");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentAccordionItem_WithMultipleAdditionalAttributes()
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentAccordionItem>(parameters =>
        {
            parameters.AddUnmatched("unknown1", "unknown1s-value");
            parameters.AddUnmatched("unknown2", "unknown2s-value");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentAccordionItem_WhenAllParamsAdded_AndAdditionalAttributes_AndContent()
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentAccordionItem>(parameters =>
        {
            parameters.Add(p => p.Expanded, true);
            parameters.Add(p => p.Heading, "custom heading value");
            parameters.AddUnmatched("unknown1", "unknown1s-value");
            parameters.AddUnmatched("unknown2", "unknown2s-value");
            parameters.AddChildContent("child content");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentAccordionItem_WhenAdditionalCSSClass_IsProvided()
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentAccordionItem>(parameters =>
        {
            parameters.Add(p => p.Class, "additional-class");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentAccordionItem_WhenAdditionalStyle_IsProvided()
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentAccordionItem>(parameters =>
        {
            parameters.Add(p => p.Style, "background-color: grey");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentAccordionItem_WithHeadingTemplate_IsNull()
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentAccordionItem>(parameters =>
        {
            parameters.Add(p => p.HeadingTemplate, context => { });
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentAccordionItem_WithHeadingTemplate_IsProvided()
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentAccordionItem>(parameters =>
        {
            parameters.Add(p => p.HeadingTemplate, context =>
            {
                context.AddContent(0, "custom heading content");
            });
        });

        // Assert
        cut.Verify();
    }
}
