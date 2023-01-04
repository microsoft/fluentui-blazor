using Bunit;
using FluentAssertions;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.Badge
{
    public class FluentBadgeRenderShould : TestBase
    {
        
        [Fact]
        public void RenderProperly_DefaultAttributes()
        {
            // Arrange && Act
            IRenderedComponent<FluentBadge> cut = TestContext.RenderComponent<FluentBadge>(
                parameters => parameters
                    .AddChildContent("childcontent"));
            
            // Assert
            cut.MarkupMatches("<fluent-badge " +
                              "appearance=\"accent\">" +
                              "childcontent" +
                              "</fluent-badge>");
        }
        
        [Theory]
        [InlineData(Appearance.Neutral)]
        [InlineData(Appearance.Accent)]
        [InlineData(Appearance.Lightweight)]
        public void RenderProperly_AppearanceAttribute(Appearance appearance)
        {
            // Arrange && Act
            IRenderedComponent<FluentBadge> cut = TestContext.RenderComponent<FluentBadge>(
                parameters => parameters
                    .Add(p => p.Appearance, appearance)
                    .AddChildContent("childcontent"));
            
            // Assert
            cut.MarkupMatches("<fluent-badge " +
                              $"appearance=\"{appearance.ToAttributeValue()}\">" +
                              "childcontent" +
                              "</fluent-badge>");
        }
        
        [Theory]
        [InlineData(Appearance.Filled)]
        [InlineData(Appearance.Hypertext)]
        [InlineData(Appearance.Outline)]
        [InlineData(Appearance.Stealth)]
        public void ThrowArgumentException_When_AppearanceAttributeValue_IsInvalid(Appearance appearance)
        {
            // Arrange && Act
            Action action = () =>
            {
                TestContext.RenderComponent<FluentBadge>(
                    parameters => parameters
                        .Add(p => p.Appearance, appearance)
                        .AddChildContent("childcontent"));
            };

            // Assert
            action.Should().ThrowExactly<ArgumentException>();
        }
        
        [Fact]
        public void RenderProperly_FillAttribute()
        {
            // Arrange && Act
            string backgroundColor = "red";
            IRenderedComponent<FluentBadge> cut = TestContext.RenderComponent<FluentBadge>(
                parameters => parameters
                    .Add(p => p.Fill, backgroundColor)
                    .AddChildContent("childcontent"));
            
            // Assert
            cut.MarkupMatches("<fluent-badge " +
                              "appearance=\"accent\" " +
                              $"fill=\"{backgroundColor}\">" +
                              "childcontent" +
                              "</fluent-badge>");
        }
        
        [Fact]
        public void RenderProperly_ColorAttribute()
        {
            // Arrange && Act
            string color = "red";
            IRenderedComponent<FluentBadge> cut = TestContext.RenderComponent<FluentBadge>(
                parameters => parameters
                    .Add(p => p.Color, color)
                    .AddChildContent("childcontent"));
            
            // Assert
            cut.MarkupMatches("<fluent-badge " +
                              "appearance=\"accent\" " +
                              $"color=\"{color}\">" +
                              "childcontent" +
                              "</fluent-badge>");
        }
        
        [Fact]
        public void RenderProperly_CircularAttribute()
        {
            // Arrange && Act
            IRenderedComponent<FluentBadge> cut = TestContext.RenderComponent<FluentBadge>(
                parameters => parameters
                    .Add(p => p.Circular, true)
                    .AddChildContent("childcontent"));
            
            // Assert
            cut.MarkupMatches("<fluent-badge " +
                              "appearance=\"accent\" " +
                              "circular=\"\">" +
                              "childcontent" +
                              "</fluent-badge>");
        }
        
        [Fact]
        public void RenderProperly_WithAdditionalCssClass()
        {
            // Arrange && Act
            string cssClass = "additional_class";
            IRenderedComponent<FluentBadge> cut = TestContext.RenderComponent<FluentBadge>(
                parameters => parameters
                    .Add(p => p.Class, cssClass)
                    .AddChildContent("childcontent"));
            
            // Assert
            cut.MarkupMatches("<fluent-badge " +
                              $"class=\"{cssClass}\" " +
                              "appearance=\"accent\">" +
                              "childcontent" +
                              "</fluent-badge>");
        }

        [Fact]
        public void RenderProperly_WithAdditionalStyle()
        {
            // Arrange && Act
            string style = "background-color:red; width:100px;";
            IRenderedComponent<FluentBadge> cut = TestContext.RenderComponent<FluentBadge>(
                parameters => parameters
                    .Add(p => p.Style, style)
                    .AddChildContent("childcontent"));
            
            // Assert
            cut.MarkupMatches("<fluent-badge " +
                              $"style=\"{style}\" " +
                              "appearance=\"accent\">" +
                              "childcontent" +
                              "</fluent-badge>");
        }

        [Fact]
        public void RenderProperly_WithAnAdditionalAttribute()
        {
            // Arrange && Act
            string additionalAttributeName = "additional-attribute-name";
            string additionalAttributeValue = "additional-attribute-value";
            IRenderedComponent<FluentBadge> cut = TestContext.RenderComponent<FluentBadge>(
                parameters => parameters
                    .AddUnmatched(additionalAttributeName, additionalAttributeValue)
                    .AddChildContent("childcontent"));
            
            // Assert
            cut.MarkupMatches("<fluent-badge " +
                              "appearance=\"accent\" " +
                              $"{additionalAttributeName}=\"{additionalAttributeValue}\">" +
                              "childcontent" +
                              "</fluent-badge>");
        }

        [Fact]
        public void RenderProperly_WithMultipleAdditionalAttributes()
        {
            // Arrange && Act
            string additionalAttribute1Name = "additional-attribute1-name";
            string additionalAttribute1Value = "additional-attribute1-value";
            string additionalAttribute2Name = "additional-attribute2-name";
            string additionalAttribute2Value = "additional-attribute2-value";
            IRenderedComponent<FluentBadge> cut = TestContext.RenderComponent<FluentBadge>(
                parameters => parameters
                    .AddUnmatched(additionalAttribute1Name, additionalAttribute1Value)
                    .AddUnmatched(additionalAttribute2Name, additionalAttribute2Value)
                    .AddChildContent("childcontent"));
            
            // Assert
            cut.MarkupMatches("<fluent-badge " +
                              "appearance=\"accent\" " +
                              $"{additionalAttribute1Name}=\"{additionalAttribute1Value}\" " +
                              $"{additionalAttribute2Name}=\"{additionalAttribute2Value}\">" +
                              "childcontent" +
                              "</fluent-badge>");
        }
    }
}