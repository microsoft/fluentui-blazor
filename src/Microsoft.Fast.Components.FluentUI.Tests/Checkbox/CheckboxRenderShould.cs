using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.Checkbox
{
    public class CheckboxRenderShould : TestBase
    {
        [Fact]
        public void RenderProperly_DefaultValues()
        {
            // Arrange && Act
            string childContent = "childContent";
            IRenderedComponent<FluentCheckbox> sut = TestContext.RenderComponent<FluentCheckbox>(
                parameters => parameters
                    .AddChildContent(childContent));
            
            // Assert
            sut.MarkupMatches("<fluent-checkbox>" +
                              $"{childContent}" +
                              "</fluent-checkbox>");
        }
        
        [Fact]
        public void RenderProperly_ReadonlyParameter()
        {
            // Arrange && Act
            string childContent = "childContent";
            IRenderedComponent<FluentCheckbox> sut = TestContext.RenderComponent<FluentCheckbox>(
                parameters => parameters
                    .AddChildContent(childContent)
                    .Add(p => p.Readonly, true));
            
            // Assert
            sut.MarkupMatches("<fluent-checkbox " +
                              "readonly=\"\">" +
                              $"{childContent}" +
                              "</fluent-checkbox>");
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("id-parameter")]
        public void RenderProperly_IdParameter(string? idParameter)
        {
            // Arrange && Act
            string childContent = "childContent";
            IRenderedComponent<FluentCheckbox> sut = TestContext.RenderComponent<FluentCheckbox>(
                parameters => parameters
                    .AddChildContent(childContent)
                    .Add(p => p.Id, idParameter));
            
            // Assert
            if (idParameter is null)
            {
                sut.MarkupMatches("<fluent-checkbox>" +
                                  $"{childContent}" +
                                  "</fluent-checkbox>");
            }
            else
            {
                sut.MarkupMatches("<fluent-checkbox " +
                                  $"id=\"{idParameter}\">" +
                                  $"{childContent}" +
                                  "</fluent-checkbox>");
            }
        }
        
        [Fact]
        public void RenderProperly_DisabledParameter()
        {
            // Arrange && Act
            string childContent = "childContent";
            IRenderedComponent<FluentCheckbox> sut = TestContext.RenderComponent<FluentCheckbox>(
                parameters => parameters
                    .AddChildContent(childContent)
                    .Add(p => p.Disabled, true));
            
            // Assert
            sut.MarkupMatches("<fluent-checkbox " +
                              "disabled=\"\">" +
                              $"{childContent}" +
                              "</fluent-checkbox>");
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("name-parameter")]
        public void RenderProperly_NameParameter(string? nameParameter)
        {
            // Arrange && Act
            string childContent = "childContent";
            IRenderedComponent<FluentCheckbox> sut = TestContext.RenderComponent<FluentCheckbox>(
                parameters => parameters
                    .AddChildContent(childContent)
                    .Add(p => p.Name, nameParameter));
            
            // Assert
            if (nameParameter is null)
            {
                sut.MarkupMatches("<fluent-checkbox>" +
                                  $"{childContent}" +
                                  "</fluent-checkbox>");
            }
            else
            {
                sut.MarkupMatches("<fluent-checkbox " +
                                  $"name=\"{nameParameter}\">" +
                                  $"{childContent}" +
                                  "</fluent-checkbox>");
            }
        }
        
        [Fact]
        public void RenderProperly_RequiredParameter()
        {
            // Arrange && Act
            string childContent = "childContent";
            IRenderedComponent<FluentCheckbox> sut = TestContext.RenderComponent<FluentCheckbox>(
                parameters => parameters
                    .AddChildContent(childContent)
                    .Add(p => p.Required, true));
            
            // Assert
            sut.MarkupMatches("<fluent-checkbox " +
                              "required=\"\">" +
                              $"{childContent}" +
                              "</fluent-checkbox>");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void RenderProperly_ValueParameter(bool value)
        {
            // Arrange && Act
            string childContent = "childContent";
            IRenderedComponent<FluentCheckbox> sut = TestContext.RenderComponent<FluentCheckbox>(
                parameters => parameters
                    .AddChildContent(childContent)
                    .Add(p => p.Value, value));
            
            // Assert
            if (value)
            {
                sut.MarkupMatches("<fluent-checkbox " +
                                  "value=\"\" current-value=\"\" current-checked=\"\">" +
                                  $"{childContent}" +
                                  "</fluent-checkbox>");
            }
            else
            {
                sut.MarkupMatches("<fluent-checkbox>" +
                                  $"{childContent}" +
                                  "</fluent-checkbox>");
            }
        }
        
        [Fact]
        public void RenderProperly_ClassParameter()
        {
            // Arrange && Act
            string childContent = "childContent";
            string cssClass = "additional-css-class";
            IRenderedComponent<FluentCheckbox> sut = TestContext.RenderComponent<FluentCheckbox>(
                parameters => parameters
                    .AddChildContent(childContent)
                    .Add(p => p.Class, cssClass));
            
            // Assert
            sut.MarkupMatches("<fluent-checkbox " +
                              $"class=\"{cssClass}\">" +
                              $"{childContent}" +
                              "</fluent-checkbox>");
        }
        
        [Fact]
        public void RenderProperly_StyleParameter()
        {
            // Arrange && Act
            string childContent = "childContent";
            string style = "background-color: red;";
            IRenderedComponent<FluentCheckbox> sut = TestContext.RenderComponent<FluentCheckbox>(
                parameters => parameters
                    .AddChildContent(childContent)
                    .Add(p => p.Style, style));
            
            // Assert
            sut.MarkupMatches("<fluent-checkbox " +
                              $"style=\"{style}\">" +
                              $"{childContent}" +
                              "</fluent-checkbox>");
        }
        
        [Fact]
        public void RenderProperly_AdditionalParameter()
        {
            // Arrange && Act
            string childContent = "childContent";
            string parameterName = "parameterName";
            string parameterValue = "parameterValue";
            IRenderedComponent<FluentCheckbox> sut = TestContext.RenderComponent<FluentCheckbox>(
                parameters => parameters
                    .AddChildContent(childContent)
                    .AddUnmatched(parameterName, parameterValue));
            
            // Assert
            sut.MarkupMatches("<fluent-checkbox " +
                              $"{parameterName}=\"{parameterValue}\">" +
                              $"{childContent}" +
                              "</fluent-checkbox>");
        }
        
        [Fact]
        public void RenderProperly_AdditionalParameters()
        {
            // Arrange && Act
            string childContent = "childContent";
            string parameter1Name = "parameter1Name";
            string parameter1Value = "parameter1Value";
            string parameter2Name = "parameter2Name";
            string parameter2Value = "parameter2Value";
            IRenderedComponent<FluentCheckbox> sut = TestContext.RenderComponent<FluentCheckbox>(
                parameters => parameters
                    .AddChildContent(childContent)
                    .AddUnmatched(parameter1Name, parameter1Value)
                    .AddUnmatched(parameter2Name, parameter2Value));
            
            // Assert
            sut.MarkupMatches("<fluent-checkbox " +
                              $"{parameter1Name}=\"{parameter1Value}\" " +
                              $"{parameter2Name}=\"{parameter2Value}\">" +
                              $"{childContent}" +
                              "</fluent-checkbox>");
        }
    }
}