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
            var cut = TestContext.RenderComponent<FluentCard>(parameters => parameters
                .AddChildContent("childcontent"));

            // Assert
            cut.Verify();
        }

        [Fact]
        public void RenderProperly_AdditionalCssClass()
        {
            // Arrange && Act
            var cut = TestContext.RenderComponent<FluentCard>(parameters => parameters
                .AddChildContent("childcontent")
                .Add(p => p.Class, "css-class"));

            // Assert
            cut.Verify();
        }

        [Fact]
        public void RenderProperly_AdditionalStyle()
        {
            // Arrange && Act
            var cut = TestContext.RenderComponent<FluentCard>(parameters => parameters
                .AddChildContent("childcontent")
                .Add(p => p.Style, "background-color: red;"));

            // Assert
            cut.Verify();
        }

        [Fact]
        public void RenderProperly_AdditionalParameter()
        {
            // Arrange && Act
            var cut = TestContext.RenderComponent<FluentCard>(parameters => parameters
                .AddChildContent("childcontent")
                .AddUnmatched("additional-parameter-name", "additional-parameter-value"));

            // Assert
            cut.Verify();
        }

        [Fact]
        public void RenderProperly_AdditionalParameters()
        {
            // Arrange && Act
            var cut = TestContext.RenderComponent<FluentCard>(parameters => parameters
                .AddChildContent("childcontent")
                .AddUnmatched("additional-parameter1-name", "additional-parameter1-value")
                .AddUnmatched("additional-parameter2-name", "additional-parameter2-value"));

            // Assert
            cut.Verify();
        }
    }
}