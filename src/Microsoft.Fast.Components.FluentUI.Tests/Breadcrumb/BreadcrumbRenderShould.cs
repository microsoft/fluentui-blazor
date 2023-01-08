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
    }
}