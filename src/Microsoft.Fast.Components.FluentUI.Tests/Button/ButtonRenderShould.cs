using Bunit;
using FluentAssertions;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.Button
{
    public class ButtonRenderShould : TestBase
    {
        [Fact]
        public void RenderProperly_Default()
        {
            // Arrange && Act
            string childContent = "fluent-button";
            IRenderedComponent<FluentButton> cut = TestContext.RenderComponent<FluentButton>(
                parameters => parameters
                    .AddChildContent(childContent));

            // Assert
            cut.MarkupMatches("<fluent-button " +
                              "type=\"button\" " +
                              "appearance=\"neutral\"> " +
                              $"{childContent}" +
                              "</fluent-button>");
        }

        [Fact]
        public void RenderProperly_AutofocusAttribute()
        {
            // Arrange && Act
            string childContent = "fluent-button";
            IRenderedComponent<FluentButton> cut = TestContext.RenderComponent<FluentButton>(
                parameters => parameters
                    .Add(p => p.Autofocus, true)
                    .AddChildContent(childContent));

            // Assert
            cut.MarkupMatches("<fluent-button " +
                              "type=\"button\" " +
                              "appearance=\"neutral\" " +
                              "autofocus=\"\"> " +
                              $"{childContent}" +
                              "</fluent-button>");
        }

        [Theory]
        [InlineData("form-id-attribute")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void RenderProperly_FormIdAttribute(string? formId)
        {
            // Arrange && Act
            string childContent = "fluent-button";
            IRenderedComponent<FluentButton> cut = TestContext.RenderComponent<FluentButton>(
                parameters => parameters
                    .Add(p => p.FormId, formId)
                    .AddChildContent(childContent));

            // Assert
            if (formId is not null)
            {
                cut.MarkupMatches("<fluent-button " +
                                  "type=\"button\" " +
                                  "appearance=\"neutral\" " +
                                  $"form=\"{formId}\"> " +
                                  $"{childContent}" +
                                  "</fluent-button>");
            }
            else
            {
                cut.MarkupMatches("<fluent-button " +
                                  "type=\"button\" " +
                                  "appearance=\"neutral\">" +
                                  $"{childContent}" +
                                  "</fluent-button>");
            }
        }

        [Theory]
        [InlineData("submit")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void RenderProperly_FormActionAttribute(string? formAction)
        {
            // Arrange && Act
            string childContent = "fluent-button";
            IRenderedComponent<FluentButton> cut = TestContext.RenderComponent<FluentButton>(
                parameters => parameters
                    .Add(p => p.Action, formAction)
                    .AddChildContent(childContent));

            // Assert
            if (formAction is not null)
            {
                cut.MarkupMatches("<fluent-button " +
                                  "type=\"button\" " +
                                  "appearance=\"neutral\" " +
                                  $"formaction=\"{formAction}\"> " +
                                  $"{childContent}" +
                                  "</fluent-button>");
            }
            else
            {
                cut.MarkupMatches("<fluent-button " +
                                  "type=\"button\" " +
                                  "appearance=\"neutral\">" +
                                  $"{childContent}" +
                                  "</fluent-button>");
            }
        }

        [Theory]
        [InlineData("multipart/form-data")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void RenderProperly_FormEnctypeAttribute(string? formEnctype)
        {
            // Arrange && Act
            string childContent = "fluent-button";
            IRenderedComponent<FluentButton> cut = TestContext.RenderComponent<FluentButton>(
                parameters => parameters
                    .Add(p => p.Enctype, formEnctype)
                    .AddChildContent(childContent));

            // Assert
            if (formEnctype is not null)
            {
                cut.MarkupMatches("<fluent-button " +
                                  "type=\"button\" " +
                                  "appearance=\"neutral\" " +
                                  $"formenctype=\"{formEnctype}\"> " +
                                  $"{childContent}" +
                                  "</fluent-button>");
            }
            else
            {
                cut.MarkupMatches("<fluent-button " +
                                  "type=\"button\" " +
                                  "appearance=\"neutral\">" +
                                  $"{childContent}" +
                                  "</fluent-button>");
            }
        }

        [Theory]
        [InlineData("post")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void RenderProperly_FormMethodAttribute(string? formMethod)
        {
            // Arrange && Act
            string childContent = "fluent-button";
            IRenderedComponent<FluentButton> cut = TestContext.RenderComponent<FluentButton>(
                parameters => parameters
                    .Add(p => p.Method, formMethod)
                    .AddChildContent(childContent));

            // Assert
            if (formMethod is not null)
            {
                cut.MarkupMatches("<fluent-button " +
                                  "type=\"button\" " +
                                  "appearance=\"neutral\" " +
                                  $"formmethod=\"{formMethod}\"> " +
                                  $"{childContent}" +
                                  "</fluent-button>");
            }
            else
            {
                cut.MarkupMatches("<fluent-button " +
                                  "type=\"button\" " +
                                  "appearance=\"neutral\">" +
                                  $"{childContent}" +
                                  "</fluent-button>");
            }
        }

        [Fact]
        public void RenderProperly_FormNovalidateAttribute()
        {
            // Arrange && Act
            string childContent = "fluent-button";
            IRenderedComponent<FluentButton> cut = TestContext.RenderComponent<FluentButton>(
                parameters => parameters
                    .Add(p => p.NoValidate, true)
                    .AddChildContent(childContent));

            // Assert
            cut.MarkupMatches("<fluent-button " +
                              "type=\"button\" " +
                              "appearance=\"neutral\" " +
                              "formnovalidate=\"\"> " +
                              $"{childContent}" +
                              "</fluent-button>");
        }

        [Theory]
        [InlineData("_self")]
        [InlineData("_blank")]
        [InlineData("_parent")]
        [InlineData("_top")]
        [InlineData("")]
        public void RenderProperly_FormTargetAttribute(string? formTarget)
        {
            // Arrange && Act
            string childContent = "fluent-button";
            IRenderedComponent<FluentButton> cut = TestContext.RenderComponent<FluentButton>(
                parameters => parameters
                    .Add(p => p.Target, formTarget)
                    .AddChildContent(childContent));

            // Assert
            cut.MarkupMatches("<fluent-button " +
                              "type=\"button\" " +
                              "appearance=\"neutral\" " +
                              $"formtarget=\"{formTarget}\"> " +
                              $"{childContent}" +
                              "</fluent-button>");
        }
        
        [Theory]
        [InlineData(" ")]
        [InlineData("_funky")]
        public void Throw_ArgumentException_When_FormTargetAttribute_IsInvalid(string? formTarget)
        {
            // Arrange && Act
            string childContent = "fluent-button";
            
            Action action = () =>
            {
                TestContext.RenderComponent<FluentButton>(
                    parameters => parameters
                        .Add(p => p.Target, formTarget)
                        .AddChildContent(childContent));
            };

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(ButtonType.Button)]
        [InlineData(ButtonType.Reset)]
        [InlineData(ButtonType.Submit)]
        public void RenderProperly_TypeAttribute(ButtonType buttonType)
        {
            // Arrange && Act
            string childContent = "fluent-button";
            IRenderedComponent<FluentButton> cut = TestContext.RenderComponent<FluentButton>(
                parameters => parameters
                    .Add(p => p.Type, buttonType)
                    .AddChildContent(childContent));

            // Assert
            cut.MarkupMatches("<fluent-button " +
                              $"type=\"{buttonType.ToAttributeValue()}\" " +
                              "appearance=\"neutral\">" +
                              $"{childContent}" +
                              "</fluent-button>");
        }

        [Theory]
        [InlineData("id-value")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void RenderProperly_IdAttribute(string? id)
        {
            // Arrange && Act
            string childContent = "fluent-button";
            IRenderedComponent<FluentButton> cut = TestContext.RenderComponent<FluentButton>(
                parameters => parameters
                    .Add(p => p.Id, id)
                    .AddChildContent(childContent));

            // Assert
            if (id is not null)
            {
                cut.MarkupMatches("<fluent-button " +
                                  "type=\"button\" " +
                                  $"id=\"{id}\" " +
                                  "appearance=\"neutral\">" +
                                  $"{childContent}" +
                                  "</fluent-button>");
            }
            else
            {
                cut.MarkupMatches("<fluent-button " +
                                  "type=\"button\" " +
                                  "appearance=\"neutral\">" +
                                  $"{childContent}" +
                                  "</fluent-button>");
            }
        }

        [Theory]
        [InlineData("some-value")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void RenderProperly_ValueAttribute(string? value)
        {
            // Arrange && Act
            string childContent = "fluent-button";
            IRenderedComponent<FluentButton> cut = TestContext.RenderComponent<FluentButton>(
                parameters => parameters
                    .Add(p => p.Value, value)
                    .AddChildContent(childContent));

            // Assert
            if (value is not null)
            {
                cut.MarkupMatches("<fluent-button " +
                                  "type=\"button\" " +
                                  $"value=\"{value}\" " +
                                  "appearance=\"neutral\">" +
                                  $"{childContent}" +
                                  "</fluent-button>");
            }
            else
            {
                cut.MarkupMatches("<fluent-button " +
                                  "type=\"button\" " +
                                  "appearance=\"neutral\">" +
                                  $"{childContent}" +
                                  "</fluent-button>");
            }
        }

        [Theory]
        [InlineData("some-value")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void RenderProperly_CurrentValueAttribute(string? currentValue)
        {
            // Arrange && Act
            string childContent = "fluent-button";
            IRenderedComponent<FluentButton> cut = TestContext.RenderComponent<FluentButton>(
                parameters => parameters
                    .Add(p => p.CurrentValue, currentValue)
                    .AddChildContent(childContent));

            // Assert
            if (currentValue is not null)
            {
                cut.MarkupMatches("<fluent-button " +
                                  "type=\"button\" " +
                                  $"current-value=\"{currentValue}\" " +
                                  "appearance=\"neutral\">" +
                                  $"{childContent}" +
                                  "</fluent-button>");
            }
            else
            {
                cut.MarkupMatches("<fluent-button " +
                                  "type=\"button\" " +
                                  "appearance=\"neutral\">" +
                                  $"{childContent}" +
                                  "</fluent-button>");
            }
        }

        [Fact]
        public void RenderProperly_DisabledAttribute()
        {
            // Arrange && Act
            string childContent = "fluent-button";
            IRenderedComponent<FluentButton> cut = TestContext.RenderComponent<FluentButton>(
                parameters => parameters
                    .Add(p => p.Disabled, true)
                    .AddChildContent(childContent));

            // Assert
            cut.MarkupMatches("<fluent-button " +
                              "type=\"button\" " +
                              "disabled=\"\"" +
                              "appearance=\"neutral\">" +
                              $"{childContent}" +
                              "</fluent-button>");
        }

        [Fact]
        public void RenderProperly_NameAttribute()
        {
            // Arrange && Act
            string childContent = "fluent-button";
            string name = "name-value";
            IRenderedComponent<FluentButton> cut = TestContext.RenderComponent<FluentButton>(
                parameters => parameters
                    .Add(p => p.Name, name)
                    .AddChildContent(childContent));

            // Assert
            cut.MarkupMatches("<fluent-button " +
                              "type=\"button\" " +
                              $"name=\"{name}\" " +
                              "appearance=\"neutral\">" +
                              $"{childContent}" +
                              "</fluent-button>");
        }

        [Fact]
        public void RenderProperly_RequiredAttribute()
        {
            // Arrange && Act
            string childContent = "fluent-button";
            IRenderedComponent<FluentButton> cut = TestContext.RenderComponent<FluentButton>(
                parameters => parameters
                    .Add(p => p.Required, true)
                    .AddChildContent(childContent));

            // Assert
            cut.MarkupMatches("<fluent-button " +
                              "type=\"button\" " +
                              "required=\"\" " +
                              "appearance=\"neutral\">" +
                              $"{childContent}" +
                              "</fluent-button>");
        }

        [Theory]
        [InlineData(Appearance.Accent)]
        [InlineData(Appearance.Filled)]
        [InlineData(Appearance.Hypertext)]
        [InlineData(Appearance.Lightweight)]
        [InlineData(Appearance.Neutral)]
        [InlineData(Appearance.Outline)]
        [InlineData(Appearance.Stealth)]
        public void RenderProperly_AppearanceAttribute(Appearance appearance)
        {
            // Arrange && Act
            string childContent = "fluent-button";
            IRenderedComponent<FluentButton> cut = TestContext.RenderComponent<FluentButton>(
                parameters => parameters
                    .Add(p => p.Appearance, appearance)
                    .AddChildContent(childContent));

            // Assert
            cut.MarkupMatches("<fluent-button " +
                              "type=\"button\" " +
                              $"appearance=\"{appearance.ToAttributeValue()}\">" +
                              $"{childContent}" +
                              "</fluent-button>");
        }

        [Fact]
        public void RenderProperly_ClassAttribute()
        {
            // Arrange && Act
            string className = "additional-class";
            string childContent = "fluent-button";
            IRenderedComponent<FluentButton> cut = TestContext.RenderComponent<FluentButton>(
                parameters => parameters
                    .Add(p => p.Class, className)
                    .AddChildContent(childContent));

            // Assert
            cut.MarkupMatches("<fluent-button " +
                              "type=\"button\" " +
                              "appearance=\"neutral\" " +
                              $"class=\"{className}\">" +
                              $"{childContent}" +
                              "</fluent-button>");
        }

        [Fact]
        public void RenderProperly_StyleAttribute()
        {
            // Arrange && Act
            string style = "background-color: green;";
            string childContent = "fluent-button";
            IRenderedComponent<FluentButton> cut = TestContext.RenderComponent<FluentButton>(
                parameters => parameters
                    .Add(p => p.Style, style)
                    .AddChildContent(childContent));

            // Assert
            cut.MarkupMatches("<fluent-button " +
                              "type=\"button\" " +
                              "appearance=\"neutral\" " +
                              $"style=\"{style}\">" +
                              $"{childContent}" +
                              "</fluent-button>");
        }

        [Fact]
        public void RenderProperly_AdditionalAttribute()
        {
            // Arrange && Act
            string additionalAttribute1Name = "additional-attribute1-name";
            string additionalAttribute1Value = "additional-attribute1-value";
            string childContent = "fluent-button";
            IRenderedComponent<FluentButton> cut = TestContext.RenderComponent<FluentButton>(
                parameters => parameters
                    .AddUnmatched(additionalAttribute1Name, additionalAttribute1Value)
                    .AddChildContent(childContent));

            // Assert
            cut.MarkupMatches("<fluent-button " +
                              "type=\"button\" " +
                              "appearance=\"neutral\" " +
                              $"{additionalAttribute1Name}=\"{additionalAttribute1Value}\">" +
                              $"{childContent}" +
                              "</fluent-button>");
        }

        [Fact]
        public void RenderProperly_AdditionalAttributes()
        {
            // Arrange && Act
            string additionalAttribute1Name = "additional-attribute1-name";
            string additionalAttribute1Value = "additional-attribute1-value";
            string additionalAttribute2Name = "additional-attribute2-name";
            string additionalAttribute2Value = "additional-attribute2-value";
            string childContent = "fluent-button";
            IRenderedComponent<FluentButton> cut = TestContext.RenderComponent<FluentButton>(
                parameters => parameters
                    .AddUnmatched(additionalAttribute1Name, additionalAttribute1Value)
                    .AddUnmatched(additionalAttribute2Name, additionalAttribute2Value)
                    .AddChildContent(childContent));

            // Assert
            cut.MarkupMatches("<fluent-button " +
                              "type=\"button\" " +
                              "appearance=\"neutral\" " +
                              $"{additionalAttribute1Name}=\"{additionalAttribute1Value}\" " +
                              $"{additionalAttribute2Name}=\"{additionalAttribute2Value}\">" +
                              $"{childContent}" +
                              "</fluent-button>");
        }
    }
}