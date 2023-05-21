using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.Breadcrumb
{
    public class BreadcrumbTests : TestBase
    {
        [Fact]
        public void Breadcrumb_DefaultAttributeValues()
        {
            // Arrange && Act
            var cut = TestContext.RenderComponent<FluentBreadcrumb>(parameters =>
            {
                parameters.AddChildContent<FluentBreadcrumbItem>(p => p.AddChildContent("child content"));
            });

            // Assert
            cut.Verify();
        }

        [Fact]
        public void Breadcrumb_AdditionalCssClass()
        {
            // Arrange && Act
            var cut = TestContext.RenderComponent<FluentBreadcrumb>(parameters =>
            {
                parameters.Add(p => p.Class, "css-class");
                parameters.AddChildContent<FluentBreadcrumbItem>(p => p.AddChildContent("child content"));
            });

            // Assert
            cut.Verify();
        }

        [Fact]
        public void Breadcrumb_AdditionalStyle()
        {
            // Arrange && Act
            var cut = TestContext.RenderComponent<FluentBreadcrumb>(parameters =>
            {
                parameters.Add(p => p.Style, "background-color: green");
                parameters.AddChildContent<FluentBreadcrumbItem>(p => p.AddChildContent("child content"));
            });

            // Assert
            cut.Verify();
        }

        [Fact]
        public void Breadcrumb_AdditionalAttribute()
        {
            // Arrange && Act
            var cut = TestContext.RenderComponent<FluentBreadcrumb>(parameters =>
            {
                parameters.AddUnmatched("additional-attribute-name", "additional-attribute-value");
                parameters.AddChildContent<FluentBreadcrumbItem>(p => p.AddChildContent("child content"));
            });

            // Assert
            cut.Verify();
        }

        [Fact]
        public void Breadcrumb_AdditionalAttributes()
        {
            // Arrange && Act
            var cut = TestContext.RenderComponent<FluentBreadcrumb>(parameters =>
            {
                parameters.AddUnmatched("additional-attribute1-name", "additional-attribute1-value");
                parameters.AddUnmatched("additional-attribute2-name", "additional-attribute2-value");
                parameters.AddChildContent<FluentBreadcrumbItem>(p => p.AddChildContent("child content"));
            });

            // Assert
            cut.Verify();
        }
    }
}