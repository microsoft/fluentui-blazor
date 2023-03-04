using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.Checkbox
{
    public class CheckboxRenderShould : TestBase
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void RenderProperly_DefaultValues(bool currentValue)
        {
            // Arrange && Act
            string childContent = "childContent";
            IRenderedComponent<FluentCheckbox> sut = TestContext.RenderComponent<FluentCheckbox>(
                parameters => parameters
                    .AddChildContent(childContent)
                    .Bind(bind => bind.Value, currentValue, newValue => currentValue = false));

            // Assert
            if (currentValue)
            {
                sut.MarkupMatches("<fluent-checkbox " +
                                  "value=\"\" " +
                                  "current-value=\"\" " +
                                  "current-checked=\"\">" +
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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void RenderProperly_ReadonlyParameter(bool currentValue)
        {
            // Arrange && Act
            string childContent = "childContent";
            IRenderedComponent<FluentCheckbox> sut = TestContext.RenderComponent<FluentCheckbox>(
                parameters => parameters
                    .AddChildContent(childContent)
                    .Bind(p => p.Value, currentValue, newValue => currentValue = false)
                    .Add(p => p.Readonly, true));

            // Assert
            if (currentValue)
            {
                sut.MarkupMatches("<fluent-checkbox  " +
                                  "value=\"\" " +
                                  "current-value=\"\" " +
                                  "current-checked=\"\" " +
                                  "readonly=\"\">" +
                                  $"{childContent}" +
                                  "</fluent-checkbox>");
            }
            else
            {
                sut.MarkupMatches("<fluent-checkbox " +
                                  "readonly=\"\">" +
                                  $"{childContent}" +
                                  "</fluent-checkbox>");
            }
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
            bool currentValue = true;
            IRenderedComponent<FluentCheckbox> sut = TestContext.RenderComponent<FluentCheckbox>(
                parameters => parameters
                    .AddChildContent(childContent)
                    .Bind(p => p.Value, currentValue, newValue => currentValue = true)
                    .Add(p => p.Id, idParameter));

            // Assert
            if (idParameter is null)
            {
                sut.MarkupMatches("<fluent-checkbox " +
                                  "value=\"\" " +
                                  "current-value=\"\" " +
                                  "current-checked=\"\">" +
                                  $"{childContent}" +
                                  "</fluent-checkbox>");
            }
            else
            {
                sut.MarkupMatches("<fluent-checkbox " +
                                  $"id=\"{idParameter}\" " +
                                  "value=\"\" " +
                                  "current-value=\"\" " +
                                  "current-checked=\"\">" +
                                  $"{childContent}" +
                                  "</fluent-checkbox>");
            }
        }

        [Fact]
        public void RenderProperly_DisabledParameter()
        {
            // Arrange && Act
            string childContent = "childContent";
            bool currentValue = true;
            IRenderedComponent<FluentCheckbox> sut = TestContext.RenderComponent<FluentCheckbox>(
                parameters => parameters
                    .AddChildContent(childContent)
                    .Bind(p => p.Value, currentValue, newValue => currentValue = false)
                    .Add(p => p.Disabled, true));

            // Assert
            sut.MarkupMatches("<fluent-checkbox " +
                              "value=\"\" " +
                              "current-value=\"\" " +
                              "current-checked=\"\" " +
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
            bool currentValue = true;
            IRenderedComponent<FluentCheckbox> sut = TestContext.RenderComponent<FluentCheckbox>(
                parameters => parameters
                    .AddChildContent(childContent)
                    .Bind(p => p.Value, currentValue, newValue => currentValue = false)
                    .Add(p => p.Name, nameParameter));

            // Assert
            if (nameParameter is null)
            {
                sut.MarkupMatches("<fluent-checkbox  " +
                                  "value=\"\" " +
                                  "current-value=\"\" " +
                                  "current-checked=\"\">" +
                                  $"{childContent}" +
                                  "</fluent-checkbox>");
            }
            else
            {
                sut.MarkupMatches("<fluent-checkbox  " +
                                  "value=\"\" " +
                                  "current-value=\"\" " +
                                  "current-checked=\"\" " +
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
            bool currentValue = true;
            IRenderedComponent<FluentCheckbox> sut = TestContext.RenderComponent<FluentCheckbox>(
                parameters => parameters
                    .AddChildContent(childContent)
                    .Bind(p => p.Value, currentValue, newValue => currentValue = false)
                    .Add(p => p.Required, true));

            // Assert
            sut.MarkupMatches("<fluent-checkbox " +
                              "required=\"\"  " +
                              "value=\"\" " +
                              "current-value=\"\" " +
                              "current-checked=\"\">" +
                              $"{childContent}" +
                              "</fluent-checkbox>");
        }

        [Fact]
        public void RenderProperly_ClassParameter()
        {
            // Arrange && Act
            string childContent = "childContent";
            string cssClass = "additional-css-class";
            bool currentValue = true;
            IRenderedComponent<FluentCheckbox> sut = TestContext.RenderComponent<FluentCheckbox>(
                parameters => parameters
                    .AddChildContent(childContent)
                    .Bind(p => p.Value, currentValue, newValue => currentValue = false)
                    .Add(p => p.Class, cssClass));

            // Assert
            sut.MarkupMatches("<fluent-checkbox " +
                              $"class=\"{cssClass}\"  " +
                              "value=\"\" " +
                              "current-value=\"\" " +
                              "current-checked=\"\">" +
                              $"{childContent}" +
                              "</fluent-checkbox>");
        }

        [Fact]
        public void RenderProperly_StyleParameter()
        {
            // Arrange && Act
            string childContent = "childContent";
            string style = "background-color: red;";
            bool currentValue = true;
            IRenderedComponent<FluentCheckbox> sut = TestContext.RenderComponent<FluentCheckbox>(
                parameters => parameters
                    .AddChildContent(childContent)
                    .Bind(p => p.Value, currentValue, newValue => currentValue = false)
                    .Add(p => p.Style, style));

            // Assert
            sut.MarkupMatches("<fluent-checkbox " +
                              $"style=\"{style}\"  " +
                              "value=\"\" " +
                              "current-value=\"\" " +
                              "current-checked=\"\">" +
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
            bool currentValue = true;
            IRenderedComponent<FluentCheckbox> sut = TestContext.RenderComponent<FluentCheckbox>(
                parameters => parameters
                    .AddChildContent(childContent)
                    .Bind(p => p.Value, currentValue, newValue => currentValue = false)
                    .AddUnmatched(parameterName, parameterValue));

            // Assert
            sut.MarkupMatches("<fluent-checkbox " +
                              $"{parameterName}=\"{parameterValue}\"  " +
                              "value=\"\" " +
                              "current-value=\"\" " +
                              "current-checked=\"\">" +
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
            bool currentValue = true;
            IRenderedComponent<FluentCheckbox> sut = TestContext.RenderComponent<FluentCheckbox>(
                parameters => parameters
                    .AddChildContent(childContent)
                    .Bind(p => p.Value, currentValue, newValue => currentValue = false)
                    .AddUnmatched(parameter1Name, parameter1Value)
                    .AddUnmatched(parameter2Name, parameter2Value));

            // Assert
            sut.MarkupMatches("<fluent-checkbox " +
                              $"{parameter1Name}=\"{parameter1Value}\" " +
                              $"{parameter2Name}=\"{parameter2Value}\" " +
                              "value=\"\" " +
                              "current-value=\"\" " +
                              "current-checked=\"\">" +
                              $"{childContent}" +
                              "</fluent-checkbox>");
        }
    }
}