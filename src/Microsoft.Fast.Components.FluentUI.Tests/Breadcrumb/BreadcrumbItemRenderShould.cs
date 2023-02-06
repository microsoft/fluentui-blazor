using Bunit;
using FluentAssertions;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.Breadcrumb
{
    public class BreadcrumbItemRenderShould : TestBase
    {
        [Fact]
        public void RenderProperly_DefaultAttributes()
        {
            // Arrange && Act
            string childContent = "childContent";
            IRenderedComponent<FluentBreadcrumbItem> cut = TestContext.RenderComponent<FluentBreadcrumbItem>(
                attributes => attributes
                    .AddChildContent(childContent));
            
            // Assert
            cut.MarkupMatches("<fluent-breadcrumb-item>" +
                              $"{childContent}" +
                              "</fluent-breadcrumb-item>");
        }
        
        [Fact]
        public void RenderProperly_DownloadAttribute()
        {
            // Arrange && Act
            string childContent = "childContent";
            string downloadAttributeValue = "https://example.org";
            IRenderedComponent<FluentBreadcrumbItem> cut = TestContext.RenderComponent<FluentBreadcrumbItem>(
                attributes => attributes
                    .Add(p => p.Download, downloadAttributeValue)
                    .AddChildContent(childContent));
            
            // Assert
            cut.MarkupMatches("<fluent-breadcrumb-item " +
                              $"download=\"{downloadAttributeValue}\">" +
                              $"{childContent}" +
                              "</fluent-breadcrumb-item>");
        }
        
        [Fact]
        public void RenderProperly_HrefAttribute()
        {
            // Arrange && Act
            string childContent = "childContent";
            string hrefAttributeValue = "https://example.org";
            IRenderedComponent<FluentBreadcrumbItem> cut = TestContext.RenderComponent<FluentBreadcrumbItem>(
                attributes => attributes
                    .Add(p => p.Href, hrefAttributeValue)
                    .AddChildContent(childContent));
            
            // Assert
            cut.MarkupMatches("<fluent-breadcrumb-item " +
                              $"href=\"{hrefAttributeValue}\">" +
                              $"{childContent}" +
                              "</fluent-breadcrumb-item>");
        }
        
        [Theory]
        [InlineData("en-GB")]
        [InlineData("fr")]
        public void RenderProperly_HrefLangAttribute(string hrefLang)
        {
            // Arrange && Act
            string childContent = "childContent";
            IRenderedComponent<FluentBreadcrumbItem> cut = TestContext.RenderComponent<FluentBreadcrumbItem>(
                attributes => attributes
                    .Add(p => p.Hreflang, hrefLang)
                    .AddChildContent(childContent));
            
            // Assert
            cut.MarkupMatches("<fluent-breadcrumb-item " +
                              $"hreflang=\"{hrefLang}\">" +
                              $"{childContent}" +
                              "</fluent-breadcrumb-item>");
        }
        
        [Theory]
        [InlineData("https://fast.design")]
        [InlineData("https://fast.design https://google.com")]
        public void RenderProperly_PingAttribute(string pingAttribute)
        {
            // Arrange && Act
            string childContent = "childContent";
            IRenderedComponent<FluentBreadcrumbItem> cut = TestContext.RenderComponent<FluentBreadcrumbItem>(
                attributes => attributes
                    .Add(p => p.Ping, pingAttribute)
                    .AddChildContent(childContent));
            
            // Assert
            cut.MarkupMatches("<fluent-breadcrumb-item " +
                              $"ping=\"{pingAttribute}\">" +
                              $"{childContent}" +
                              "</fluent-breadcrumb-item>");
        }
        
        [Theory]
        [InlineData("no-referrer")]
        [InlineData("no-referrer-when-downgrade")]
        public void RenderProperly_ReferrerPolicyAttribute(string referrerPolicy)
        {
            // Arrange && Act
            string childContent = "childContent";
            IRenderedComponent<FluentBreadcrumbItem> cut = TestContext.RenderComponent<FluentBreadcrumbItem>(
                attributes => attributes
                    .Add(p => p.Referrerpolicy, referrerPolicy)
                    .AddChildContent(childContent));
            
            // Assert
            cut.MarkupMatches("<fluent-breadcrumb-item " +
                              $"referrerpolicy=\"{referrerPolicy}\">" +
                              $"{childContent}" +
                              "</fluent-breadcrumb-item>");
        }
        
        [Theory]
        [InlineData("alternate")]
        [InlineData("bookmark")]
        public void RenderProperly_RelAttribute(string relValue)
        {
            // Arrange && Act
            string childContent = "childContent";
            IRenderedComponent<FluentBreadcrumbItem> cut = TestContext.RenderComponent<FluentBreadcrumbItem>(
                attributes => attributes
                    .Add(p => p.Rel, relValue)
                    .AddChildContent(childContent));
            
            // Assert
            cut.MarkupMatches("<fluent-breadcrumb-item " +
                              $"rel=\"{relValue}\">" +
                              $"{childContent}" +
                              "</fluent-breadcrumb-item>");
        }
        
        [Theory]
        [InlineData("_blank")]
        [InlineData("_self")]
        [InlineData("_parent")]
        [InlineData("_top")]
        [InlineData("invalid")]
        public void RenderProperly_TargetAttribute(string targetAttribute)
        {
            // Arrange && Act
            string childContent = "childContent";
            IRenderedComponent<FluentBreadcrumbItem>? cut = null;
            Action action = () =>
            {
                cut = TestContext.RenderComponent<FluentBreadcrumbItem>(
                    attributes => attributes
                        .Add(p => p.Target, targetAttribute)
                        .AddChildContent(childContent));
            };
            
            // Assert
            if (targetAttribute == "invalid")
            {
                action.Should().Throw<ArgumentException>();
            }
            else
            {
                action.Should().NotThrow();
                cut!.MarkupMatches("<fluent-breadcrumb-item " +
                                  $"target=\"{targetAttribute}\">" +
                                  $"{childContent}" +
                                  "</fluent-breadcrumb-item>");
            }
        }
        
        [Theory]
        [InlineData("image/png")]
        [InlineData("application/pdf")]
        public void RenderProperly_TypeAttribute(string typeAttribute)
        {
            // Arrange && Act
            string childContent = "childContent";
            IRenderedComponent<FluentBreadcrumbItem> cut = TestContext.RenderComponent<FluentBreadcrumbItem>(
                attributes => attributes
                    .Add(p => p.Type, typeAttribute)
                    .AddChildContent(childContent));
            
            // Assert
            cut.MarkupMatches("<fluent-breadcrumb-item " +
                              $"type=\"{typeAttribute}\">" +
                              $"{childContent}" +
                              "</fluent-breadcrumb-item>");
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
            // Arrange && Act
            string childContent = "childContent";
            IRenderedComponent<FluentBreadcrumbItem> cut = TestContext.RenderComponent<FluentBreadcrumbItem>(
                attributes => attributes
                    .Add(p => p.Appearance, appearance)
                    .AddChildContent(childContent));
            
            // Assert
            cut.MarkupMatches("<fluent-breadcrumb-item " +
                              $"appearance=\"{appearance.ToAttributeValue()}\">" +
                              $"{childContent}" +
                              "</fluent-breadcrumb-item>");
        }

        [Fact]
        public void RenderProperly_AdditionalCssClass()
        {
            // Arrange && Act
            string childContent = "childContent";
            string cssClass = "additional-css-class";
            IRenderedComponent<FluentBreadcrumbItem> cut = TestContext.RenderComponent<FluentBreadcrumbItem>(
                attributes => attributes
                    .Add(p => p.Class, cssClass)
                    .AddChildContent(childContent));
            
            // Assert
            cut.MarkupMatches("<fluent-breadcrumb-item " +
                              $"class=\"{cssClass}\">" +
                              $"{childContent}" +
                              "</fluent-breadcrumb-item>");
        }
        
        [Fact]
        public void RenderProperly_AdditionalStyle()
        {
            // Arrange && Act
            string childContent = "childContent";
            string style = "background-color: white;";
            IRenderedComponent<FluentBreadcrumbItem> cut = TestContext.RenderComponent<FluentBreadcrumbItem>(
                attributes => attributes
                    .Add(p => p.Style, style)
                    .AddChildContent(childContent));
            
            // Assert
            cut.MarkupMatches("<fluent-breadcrumb-item " +
                              $"style=\"{style}\">" +
                              $"{childContent}" +
                              "</fluent-breadcrumb-item>");
        }
        
        [Fact]
        public void RenderProperly_AdditionalUnmatchedAttribute()
        {
            // Arrange && Act
            string childContent = "childContent";
            string additionalAttributeName = "additional-attribute-name";
            string additionalAttributeValue = "additional-attribute-value";
            IRenderedComponent<FluentBreadcrumbItem> cut = TestContext.RenderComponent<FluentBreadcrumbItem>(
                attributes => attributes
                    .AddUnmatched(additionalAttributeName, additionalAttributeValue)
                    .AddChildContent(childContent));
            
            // Assert
            cut.MarkupMatches("<fluent-breadcrumb-item " +
                              $"{additionalAttributeName}=\"{additionalAttributeValue}\">" +
                              $"{childContent}" +
                              "</fluent-breadcrumb-item>");
        }
        
        [Fact]
        public void RenderProperly_AdditionalUnmatchedAttributes()
        {
            // Arrange && Act
            string childContent = "childContent";
            string additionalAttribute1Name = "additional-attribute1-name";
            string additionalAttribute1Value = "additional-attribute1-value";
            string additionalAttribute2Name = "additional-attribute2-name";
            string additionalAttribute2Value = "additional-attribute2-value";
            IRenderedComponent<FluentBreadcrumbItem> cut = TestContext.RenderComponent<FluentBreadcrumbItem>(
                attributes => attributes
                    .AddUnmatched(additionalAttribute1Name, additionalAttribute1Value)
                    .AddUnmatched(additionalAttribute2Name, additionalAttribute2Value)
                    .AddChildContent(childContent));
            
            // Assert
            cut.MarkupMatches("<fluent-breadcrumb-item " +
                              $"{additionalAttribute1Name}=\"{additionalAttribute1Value}\" " +
                              $"{additionalAttribute2Name}=\"{additionalAttribute2Value}\">" +
                              $"{childContent}" +
                              "</fluent-breadcrumb-item>");
        }
    }
}