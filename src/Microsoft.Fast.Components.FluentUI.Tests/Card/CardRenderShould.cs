using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.Card
{
    public class CardRenderShould : TestBase
    {
        [Fact]
        public void RenderProperly_Default()
        {
            // Arrange && Act
            string childContent = "childcontent";
            IRenderedComponent<FluentCard> cut = TestContext.RenderComponent<FluentCard>(parameters => parameters
                .AddChildContent(childContent));
            
            // Assert
            cut.MarkupMatches("<fluent-card>" +
                              $"{childContent}" +
                              "</fluent-card>");
        }
        
        [Fact]
        public void RenderProperly_AdditionalCssClass()
        {
            // Arrange && Act
            string childContent = "childcontent";
            string cssClass = "css-class";
            IRenderedComponent<FluentCard> cut = TestContext.RenderComponent<FluentCard>(parameters => parameters
                .AddChildContent(childContent)
                .Add(p =>p.Class, cssClass));
            
            // Assert
            cut.MarkupMatches("<fluent-card " +
                              $"class=\"{cssClass}\">" +
                              $"{childContent}" +
                              "</fluent-card>");
        }
        
        [Fact]
        public void RenderProperly_AdditionalStyle()
        {
            // Arrange && Act
            string childContent = "childcontent";
            string style = "background-color: red;";
            IRenderedComponent<FluentCard> cut = TestContext.RenderComponent<FluentCard>(parameters => parameters
                .AddChildContent(childContent)
                .Add(p =>p.Style, style));
            
            // Assert
            cut.MarkupMatches("<fluent-card " +
                              $"style=\"{style}\">" +
                              $"{childContent}" +
                              "</fluent-card>");
        }
        
        [Fact]
        public void RenderProperly_AdditionalParameter()
        {
            // Arrange && Act
            string childContent = "childcontent";
            string additionalParameterName = "additional-parameter-name";
            string additionalParameterValue = "additional-parameter-value";
            IRenderedComponent<FluentCard> cut = TestContext.RenderComponent<FluentCard>(parameters => parameters
                .AddChildContent(childContent)
                .AddUnmatched(additionalParameterName, additionalParameterValue));
            
            // Assert
            cut.MarkupMatches("<fluent-card " +
                              $"{additionalParameterName}=\"{additionalParameterValue}\">" +
                              $"{childContent}" +
                              "</fluent-card>");
        }
        
        [Fact]
        public void RenderProperly_AdditionalParameters()
        {
            // Arrange && Act
            string childContent = "childcontent";
            string additionalParameter1Name = "additional-parameter1-name";
            string additionalParameter1Value = "additional-parameter1-value";            
            string additionalParameter2Name = "additional-parameter2-name";
            string additionalParameter2Value = "additional-parameter2-value";
            IRenderedComponent<FluentCard> cut = TestContext.RenderComponent<FluentCard>(parameters => parameters
                .AddChildContent(childContent)
                .AddUnmatched(additionalParameter1Name, additionalParameter1Value)
                .AddUnmatched(additionalParameter2Name, additionalParameter2Value));
            
            // Assert
            cut.MarkupMatches("<fluent-card " +
                              $"{additionalParameter1Name}=\"{additionalParameter1Value}\" " +
                              $"{additionalParameter2Name}=\"{additionalParameter2Value}\">" +
                              $"{childContent}" +
                              "</fluent-card>");
        }
    }
}