using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Accordion;

public class FluentAccordionTests : TestBase
{
    [Fact]
    public void FluentAccordion_When_ChildContent_IsNull()
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentAccordion>();

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentAccordion_TheDefaultExpandMode_WhenExpandMode_IsNotSpecified()
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentAccordion>();

        // Assert
        cut.Verify();
    }

    [Theory]
    [InlineData(AccordionExpandMode.Multi)]
    [InlineData(AccordionExpandMode.Single)]
    public void FluentAccordion_WhenExpandMode_IsSpecified(AccordionExpandMode accordionExpandMode)
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentAccordion>(parameters =>
        {
            parameters.Add(p => p.ExpandMode, accordionExpandMode);
        });

        // Assert
        cut.Verify(suffix: accordionExpandMode.ToString());
    }

    [Fact]
    public void FluentAccordion_WhenAdditionalCSSClass_IsProvided()
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentAccordion>(parameters =>
        {
            parameters.Add(p => p.Class, "additional-class");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentAccordion_WhenAdditionalStyle_IsProvided()
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentAccordion>(parameters =>
        {
            parameters.Add(p => p.Style, "background-color: grey");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentAccordion_WhenAdditionalParameters_AreAdded()
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentAccordion>(parameters =>
        {
            parameters.AddUnmatched("unmatched1", "unmatched1-value");
            parameters.AddUnmatched("unmatched2", "unmatched2-value");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentAccordion_WhenExpandedModeIsSingle_AndMultipleItemAreExpanded_ByDefault()
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentAccordion>(parameters =>
        {
            parameters.Add(p => p.ExpandMode, AccordionExpandMode.Single);
            parameters.AddChildContent<FluentAccordionItem>(itemParams => itemParams.Add(p => p.Expanded, true));
            parameters.AddChildContent<FluentAccordionItem>(itemParams => itemParams.Add(p => p.Expanded, true));
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentAccordion_Dispose()
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentAccordion>(parameters =>
        {
            parameters.Add(p => p.ExpandMode, AccordionExpandMode.Single);
            parameters.AddChildContent<FluentAccordionItem>(itemParams => itemParams.Add(p => p.Expanded, true));
            parameters.AddChildContent<FluentAccordionItem>(itemParams => itemParams.Add(p => p.Expanded, true));
        });

        TestContext.DisposeComponents();

        // Assert
        Assert.True(cut.IsDisposed);
    }
}
