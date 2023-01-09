using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.Breadcrumb
{
    public class BreadcrumbRenderShould : TestBase
    {
        [Fact]
        public void RenderProperly_DefaultAttributeValues()
        {
            // Arrange && Act
            string childContent = "child content";
            IRenderedComponent<FluentBreadcrumb> cut = TestContext.RenderComponent<FluentBreadcrumb>(
                parameters => parameters
                    .AddChildContent<FluentBreadcrumbItem>(p => p.AddChildContent(childContent)));

            // Assert
            cut.MarkupMatches("<fluent-breadcrumb>" +
                              "<fluent-breadcrumb-item>" +
                              $"{childContent}" +
                              "</fluent-breadcrumb-item>" +
                              "</fluent-breadcrumb>");
        }
        
        [Fact]
        public void RenderProperly_AdditionalCssClass()
        {
            // Arrange && Act
            string childContent = "child content";
            string cssClass = "css-class";
            IRenderedComponent<FluentBreadcrumb> cut = TestContext.RenderComponent<FluentBreadcrumb>(
                parameters => parameters
                    .Add(p => p.Class, cssClass)
                    .AddChildContent<FluentBreadcrumbItem>(p => p.AddChildContent(childContent)));

            // Assert
            cut.MarkupMatches("<fluent-breadcrumb " +
                              $"class=\"{cssClass}\">" +
                              "<fluent-breadcrumb-item>" +
                              $"{childContent}" +
                              "</fluent-breadcrumb-item>" +
                              "</fluent-breadcrumb>");
        }
        
        [Fact]
        public void RenderProperly_AdditionalStyle()
        {
            // Arrange && Act
            string childContent = "child content";
            string style = "background-color: green;";
            IRenderedComponent<FluentBreadcrumb> cut = TestContext.RenderComponent<FluentBreadcrumb>(
                parameters => parameters
                    .Add(p => p.Style, style)
                    .AddChildContent<FluentBreadcrumbItem>(p => p.AddChildContent(childContent)));

            // Assert
            cut.MarkupMatches("<fluent-breadcrumb " +
                              $"style=\"{style}\">" +
                              "<fluent-breadcrumb-item>" +
                              $"{childContent}" +
                              "</fluent-breadcrumb-item>" +
                              "</fluent-breadcrumb>");
        }
        
        [Fact]
        public void RenderProperly_AdditionalAttribute()
        {
            // Arrange && Act
            string childContent = "child content";
            string additionalAttributeName = "additional-attribute-name";
            string additionalAttributeValue = "additional-attribute-value";
            IRenderedComponent<FluentBreadcrumb> cut = TestContext.RenderComponent<FluentBreadcrumb>(
                parameters => parameters
                    .AddUnmatched(additionalAttributeName, additionalAttributeValue)
                    .AddChildContent<FluentBreadcrumbItem>(p => p.AddChildContent(childContent)));

            // Assert
            cut.MarkupMatches("<fluent-breadcrumb " +
                              $"{additionalAttributeName}=\"{additionalAttributeValue}\">" +
                              "<fluent-breadcrumb-item>" +
                              $"{childContent}" +
                              "</fluent-breadcrumb-item>" +
                              "</fluent-breadcrumb>");
        }
        
        [Fact]
        public void RenderProperly_AdditionalAttributes()
        {
            // Arrange && Act
            string childContent = "child content";
            string additionalAttribute1Name = "additional-attribute1-name";
            string additionalAttribute1Value = "additional-attribute1-value";
            string additionalAttribute2Name = "additional-attribute2-name";
            string additionalAttribute2Value = "additional-attribute2-value";
            IRenderedComponent<FluentBreadcrumb> cut = TestContext.RenderComponent<FluentBreadcrumb>(
                parameters => parameters
                    .AddUnmatched(additionalAttribute1Name, additionalAttribute1Value)
                    .AddUnmatched(additionalAttribute2Name, additionalAttribute2Value)
                    .AddChildContent<FluentBreadcrumbItem>(p => p.AddChildContent(childContent)));

            // Assert
            cut.MarkupMatches("<fluent-breadcrumb " +
                              $"{additionalAttribute1Name}=\"{additionalAttribute1Value}\" " +
                              $"{additionalAttribute2Name}=\"{additionalAttribute2Value}\">" +
                              "<fluent-breadcrumb-item>" +
                              $"{childContent}" +
                              "</fluent-breadcrumb-item>" +
                              "</fluent-breadcrumb>");
        }
    }
}