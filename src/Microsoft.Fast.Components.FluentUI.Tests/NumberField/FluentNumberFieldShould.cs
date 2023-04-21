using Bunit;
using FluentAssertions;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.NumberField
{
    public class FluentNumberFieldShould : TestBase
    {
        [Fact]
        public void RenderProperly_Default()
        {
            // Arrange && Act
            int currentValue = 100;
            IRenderedComponent<FluentNumberField<int>> sut = TestContext.RenderComponent<FluentNumberField<int>>(
                parameters => parameters
                    .AddChildContent("100")
                    .Bind(p => p.Value, currentValue, newValue => currentValue = 101));

            // Assert
            sut.MarkupMatches("<fluent-number-field " +
                              "maxlength=\"14\" " +
                              "minlength=\"1\" " +
                              "size=\"20\" " +
                              "step=\"1\" " +
                              "max=\"2147483647\" " +
                              "min=\"-2147483648\" " +
                              "value=\"100\" " +
                              "current-value=\"100\">" +
                              "100" +
                              "</fluent-number-field>");
        }

        [Fact]
        public void RenderProperly_ReadOnlyParameter()
        {
            // Arrange && Act
            int currentValue = 100;
            IRenderedComponent<FluentNumberField<int>> sut = TestContext.RenderComponent<FluentNumberField<int>>(
                parameters => parameters
                    .AddChildContent("100")
                    .Add(p => p.Readonly, true)
                    .Bind(p => p.Value, currentValue, newValue => currentValue = 101));

            // Assert
            sut.MarkupMatches("<fluent-number-field " +
                              "maxlength=\"14\" " +
                              "minlength=\"1\" " +
                              "size=\"20\" " +
                              "step=\"1\" " +
                              "max=\"2147483647\" " +
                              "min=\"-2147483648\" " +
                              "value=\"100\" " +
                              "current-value=\"100\" " +
                              "readonly=\"\">" +
                              "100" +
                              "</fluent-number-field>");
        }

        [Fact]
        public void RenderProperly_AutoFocusParameter()
        {
            // Arrange && Act
            int currentValue = 100;
            IRenderedComponent<FluentNumberField<int>> sut = TestContext.RenderComponent<FluentNumberField<int>>(
                parameters => parameters
                    .AddChildContent("100")
                    .Add(p => p.Autofocus, true)
                    .Bind(p => p.Value, currentValue, newValue => currentValue = 101));

            // Assert
            sut.MarkupMatches("<fluent-number-field " +
                              "maxlength=\"14\" " +
                              "minlength=\"1\" " +
                              "size=\"20\" " +
                              "step=\"1\" " +
                              "max=\"2147483647\" " +
                              "min=\"-2147483648\" " +
                              "value=\"100\" " +
                              "current-value=\"100\" " +
                              "autofocus=\"\">" +
                              "100" +
                              "</fluent-number-field>");
        }

        [Fact]
        public void RenderProperly_HideStepParameter()
        {
            // Arrange && Act
            int currentValue = 100;
            IRenderedComponent<FluentNumberField<int>> sut = TestContext.RenderComponent<FluentNumberField<int>>(
                parameters => parameters
                    .AddChildContent("100")
                    .Add(p => p.HideStep, true)
                    .Bind(p => p.Value, currentValue, newValue => currentValue = 101));

            // Assert
            sut.MarkupMatches("<fluent-number-field " +
                              "maxlength=\"14\" " +
                              "minlength=\"1\" " +
                              "size=\"20\" " +
                              "step=\"1\" " +
                              "max=\"2147483647\" " +
                              "min=\"-2147483648\" " +
                              "value=\"100\" " +
                              "current-value=\"100\" " +
                              "hide-step=\"\">" +
                              "100" +
                              "</fluent-number-field>");
        }

        [Fact]
        public void RenderProperly_PlaceholderParameter()
        {
            // Arrange && Act
            int currentValue = 100;
            string placeHolder = "placeholder-value";
            IRenderedComponent<FluentNumberField<int>> sut = TestContext.RenderComponent<FluentNumberField<int>>(
                parameters => parameters
                    .AddChildContent("100")
                    .Add(p => p.Placeholder, placeHolder)
                    .Bind(p => p.Value, currentValue, newValue => currentValue = 101));

            // Assert
            sut.MarkupMatches("<fluent-number-field " +
                              "maxlength=\"14\" " +
                              "minlength=\"1\" " +
                              "size=\"20\" " +
                              "step=\"1\" " +
                              "max=\"2147483647\" " +
                              "min=\"-2147483648\" " +
                              "value=\"100\" " +
                              "current-value=\"100\" " +
                              $"placeholder=\"{placeHolder}\">" +
                              "100" +
                              "</fluent-number-field>");
        }

        [Fact]
        public void RenderProperly_DataListParameter()
        {
            // Arrange && Act
            int currentValue = 100;
            string dataList = "dataList";
            IRenderedComponent<FluentNumberField<int>> sut = TestContext.RenderComponent<FluentNumberField<int>>(
                parameters => parameters
                    .AddChildContent("100")
                    .Add(p => p.DataList, dataList)
                    .Bind(p => p.Value, currentValue, newValue => currentValue = 101));

            // Assert
            sut.MarkupMatches("<fluent-number-field " +
                              "maxlength=\"14\" " +
                              "minlength=\"1\" " +
                              "size=\"20\" " +
                              "step=\"1\" " +
                              "max=\"2147483647\" " +
                              "min=\"-2147483648\" " +
                              "value=\"100\" " +
                              "current-value=\"100\" " +
                              $"list=\"{dataList}\">" +
                              "100" +
                              "</fluent-number-field>");
        }

        [Fact]
        public void RenderProperly_MaxLengthParameter()
        {
            // Arrange && Act
            int currentValue = 100;
            int maxLength = 10;
            IRenderedComponent<FluentNumberField<int>> sut = TestContext.RenderComponent<FluentNumberField<int>>(
                parameters => parameters
                    .AddChildContent("100")
                    .Add(p => p.MaxLength, maxLength)
                    .Bind(p => p.Value, currentValue, newValue => currentValue = 101));

            // Assert
            sut.MarkupMatches("<fluent-number-field " +
                              $"maxlength=\"{maxLength}\" " +
                              "minlength=\"1\" " +
                              "size=\"20\" " +
                              "step=\"1\" " +
                              "max=\"2147483647\" " +
                              "min=\"-2147483648\" " +
                              "value=\"100\" " +
                              "current-value=\"100\">" +
                              "100" +
                              "</fluent-number-field>");
        }

        [Fact]
        public void RenderProperly_MinLengthParameter()
        {
            // Arrange && Act
            int currentValue = 100;
            int minLength = 3;
            IRenderedComponent<FluentNumberField<int>> sut = TestContext.RenderComponent<FluentNumberField<int>>(
                parameters => parameters
                    .AddChildContent("100")
                    .Add(p => p.MinLength, minLength)
                    .Bind(p => p.Value, currentValue, newValue => currentValue = 101));

            // Assert
            sut.MarkupMatches("<fluent-number-field " +
                              "maxlength=\"14\" " +
                              $"minlength=\"{minLength}\" " +
                              "size=\"20\" " +
                              "step=\"1\" " +
                              "max=\"2147483647\" " +
                              "min=\"-2147483648\" " +
                              "value=\"100\" " +
                              "current-value=\"100\">" +
                              "100" +
                              "</fluent-number-field>");
        }

        [Fact]
        public void RenderProperly_SizeParameter()
        {
            // Arrange && Act
            int currentValue = 100;
            int size = 3;
            IRenderedComponent<FluentNumberField<int>> sut = TestContext.RenderComponent<FluentNumberField<int>>(
                parameters => parameters
                    .AddChildContent("100")
                    .Add(p => p.Size, size)
                    .Bind(p => p.Value, currentValue, newValue => currentValue = 101));

            // Assert
            sut.MarkupMatches("<fluent-number-field " +
                              "maxlength=\"14\" " +
                              "minlength=\"1\" " +
                              $"size=\"{size}\" " +
                              "step=\"1\" " +
                              "max=\"2147483647\" " +
                              "min=\"-2147483648\" " +
                              "value=\"100\" " +
                              "current-value=\"100\">" +
                              "100" +
                              "</fluent-number-field>");
        }

        [Fact]
        public void RenderProperly_StepParameter()
        {
            // Arrange && Act
            int currentValue = 100;
            string step = "3";
            IRenderedComponent<FluentNumberField<int>> sut = TestContext.RenderComponent<FluentNumberField<int>>(
                parameters => parameters
                    .AddChildContent("100")
                    .Add(p => p.Step, step)
                    .Bind(p => p.Value, currentValue, newValue => currentValue = 101));

            // Assert
            sut.MarkupMatches("<fluent-number-field " +
                              "maxlength=\"14\" " +
                              "minlength=\"1\" " +
                              "size=\"20\" " +
                              $"step=\"{step}\" " +
                              "max=\"2147483647\" " +
                              "min=\"-2147483648\" " +
                              "value=\"100\" " +
                              "current-value=\"100\">" +
                              "100" +
                              "</fluent-number-field>");
        }

        [Fact]
        public void RenderProperly_MaxParameter()
        {
            // Arrange && Act
            int currentValue = 100;
            string max = "2147483647";
            IRenderedComponent<FluentNumberField<int>> sut = TestContext.RenderComponent<FluentNumberField<int>>(
                parameters => parameters
                    .AddChildContent("100")
                    .Add(p => p.Max, max)
                    .Bind(p => p.Value, currentValue, newValue => currentValue = 101));

            // Assert
            sut.MarkupMatches("<fluent-number-field " +
                              "maxlength=\"14\" " +
                              "minlength=\"1\" " +
                              "size=\"20\" " +
                              "step=\"1\" " +
                              $"max=\"{max}\" " +
                              "min=\"-2147483648\" " +
                              "value=\"100\" " +
                              "current-value=\"100\">" +
                              "100" +
                              "</fluent-number-field>");
        }

        [Fact]
        public void RenderProperly_MinParameter()
        {
            // Arrange && Act
            int currentValue = 100;
            string min = "-2147483648";
            IRenderedComponent<FluentNumberField<int>> sut = TestContext.RenderComponent<FluentNumberField<int>>(
                parameters => parameters
                    .AddChildContent("100")
                    .Add(p => p.Min, min)
                    .Bind(p => p.Value, currentValue, newValue => currentValue = 101));

            // Assert
            sut.MarkupMatches("<fluent-number-field " +
                              "maxlength=\"14\" " +
                              "minlength=\"1\" " +
                              "size=\"20\" " +
                              "step=\"1\" " +
                              "max=\"2147483647\" " +
                              $"min=\"{min}\" " +
                              "value=\"100\" " +
                              "current-value=\"100\">" +
                              "100" +
                              "</fluent-number-field>");
        }

        [Fact]
        public void RenderProperly_IdParameter()
        {
            // Arrange && Act
            int currentValue = 100;
            string id = "some-unique-id";
            IRenderedComponent<FluentNumberField<int>> sut = TestContext.RenderComponent<FluentNumberField<int>>(
                parameters => parameters
                    .AddChildContent("100")
                    .Add(p => p.Id, id)
                    .Bind(p => p.Value, currentValue, newValue => currentValue = 101));

            // Assert
            sut.MarkupMatches("<fluent-number-field " +
                              "maxlength=\"14\" " +
                              "minlength=\"1\" " +
                              "size=\"20\" " +
                              "step=\"1\" " +
                              "max=\"2147483647\" " +
                              "min=\"-2147483648\" " +
                              "value=\"100\" " +
                              "current-value=\"100\" " +
                              $"id=\"{id}\">" +
                              "100" +
                              "</fluent-number-field>");
        }

        [Fact]
        public void RenderProperly_DisabledParameter()
        {
            // Arrange && Act
            int currentValue = 100;
            bool isDisabled = true;
            IRenderedComponent<FluentNumberField<int>> sut = TestContext.RenderComponent<FluentNumberField<int>>(
                parameters => parameters
                    .AddChildContent("100")
                    .Add(p => p.Disabled, isDisabled)
                    .Bind(p => p.Value, currentValue, newValue => currentValue = 101));

            // Assert
            sut.MarkupMatches("<fluent-number-field " +
                              "maxlength=\"14\" " +
                              "minlength=\"1\" " +
                              "size=\"20\" " +
                              "step=\"1\" " +
                              "max=\"2147483647\" " +
                              "min=\"-2147483648\" " +
                              "value=\"100\" " +
                              "current-value=\"100\" " +
                              "disabled=\"\">" +
                              "100" +
                              "</fluent-number-field>");
        }

        [Fact]
        public void RenderProperly_NameParameter()
        {
            // Arrange && Act
            int currentValue = 100;
            string name = "name";
            IRenderedComponent<FluentNumberField<int>> sut = TestContext.RenderComponent<FluentNumberField<int>>(
                parameters => parameters
                    .AddChildContent("100")
                    .Add(p => p.Name, name)
                    .Bind(p => p.Value, currentValue, newValue => currentValue = 101));

            // Assert
            sut.MarkupMatches("<fluent-number-field " +
                              "maxlength=\"14\" " +
                              "minlength=\"1\" " +
                              "size=\"20\" " +
                              "step=\"1\" " +
                              "max=\"2147483647\" " +
                              "min=\"-2147483648\" " +
                              "value=\"100\" " +
                              "current-value=\"100\" " +
                              $"name=\"{name}\">" +
                              "100" +
                              "</fluent-number-field>");
        }

        [Fact]
        public void RenderProperly_RequiredParameter()
        {
            // Arrange && Act
            int currentValue = 100;
            bool isRequired = true;
            IRenderedComponent<FluentNumberField<int>> sut = TestContext.RenderComponent<FluentNumberField<int>>(
                parameters => parameters
                    .AddChildContent("100")
                    .Add(p => p.Required, isRequired)
                    .Bind(p => p.Value, currentValue, newValue => currentValue = 101));

            // Assert
            sut.MarkupMatches("<fluent-number-field " +
                              "maxlength=\"14\" " +
                              "minlength=\"1\" " +
                              "size=\"20\" " +
                              "step=\"1\" " +
                              "max=\"2147483647\" " +
                              "min=\"-2147483648\" " +
                              "value=\"100\" " +
                              "current-value=\"100\" " +
                              "required=\"\">" +
                              "100" +
                              "</fluent-number-field>");
        }

        [Theory]
        [InlineData(Appearance.Accent)]
        [InlineData(Appearance.Filled)]
        [InlineData(Appearance.Hypertext)]
        [InlineData(Appearance.Lightweight)]
        [InlineData(Appearance.Neutral)]
        [InlineData(Appearance.Outline)]
        [InlineData(Appearance.Stealth)]
        public void RenderProperly_AppearanceParameter(Appearance appearance)
        {
            // Arrange && Act
            int currentValue = 100;
            IRenderedComponent<FluentNumberField<int>> sut = TestContext.RenderComponent<FluentNumberField<int>>(
                parameters => parameters
                    .AddChildContent("100")
                    .Add(p => p.Appearance, appearance)
                    .Bind(p => p.Value, currentValue, newValue => currentValue = 101));

            // Assert
            sut.MarkupMatches("<fluent-number-field " +
                              "maxlength=\"14\" " +
                              "minlength=\"1\" " +
                              "size=\"20\" " +
                              "step=\"1\" " +
                              "max=\"2147483647\" " +
                              "min=\"-2147483648\" " +
                              "value=\"100\" " +
                              "current-value=\"100\" " +
                              $"appearance=\"{appearance.ToAttributeValue()}\">" +
                              "100" +
                              "</fluent-number-field>");
        }

        [Fact]
        public void Throw_WhenIntMin_IsOutOfRange()
        {
            // Act
            string childContent = "100";
            int currentValue = 100;
            string min = $"{int.MinValue}1";
            Action action = () =>
            {
                TestContext.RenderComponent<FluentNumberField<int>>(
                    parameters => parameters
                        .AddChildContent(childContent)
                        .Bind(p => p.Value, currentValue, newValue => currentValue = 101)
                        .Add(p => p.Min, min));
            };

            // Assert
            action.Should().Throw<OverflowException>();
        }

        [Fact]
        public void Throw_WhenIntMax_IsOutOfRange()
        {
            // Act
            string childContent = "100";
            int currentValue = 100;
            string max = $"{int.MaxValue}1";
            Action action = () =>
            {
                TestContext.RenderComponent<FluentNumberField<int>>(
                    parameters => parameters
                        .AddChildContent(childContent)
                        .Bind(p => p.Value, currentValue, newValue => currentValue = 101)
                        .Add(p => p.Max, max));
            };

            // Assert
            action.Should().Throw<OverflowException>();
        }

        [Fact]
        public void Throw_WhenIntMaxIsSmallerThanMin()
        {
            // Act
            string childContent = "100";
            int currentValue = 100;
            string min = $"{1000}";
            string max = $"{500}";
            Action action = () =>
            {
                TestContext.RenderComponent<FluentNumberField<int>>(
                    parameters => parameters
                        .AddChildContent(childContent)
                        .Bind(p => p.Value, currentValue, newValue => currentValue = 101)
                        .Add(p => p.Min, min)
                        .Add(p => p.Max, max));
            };

            // Assert
            action.Should().Throw<ArgumentException>();
        }
        
        [Theory]
        [InlineData(null, "2147483647")]
        [InlineData("", "2147483647")]
        [InlineData(" ", "2147483647")]
        public void Throw_WhenIntMinOrMax_NullOrEmptyOrWhitespace(string min, string max)
        {
            // Act
            string childContent = "100";
            int currentValue = 100;
            Action action = () =>
            {
                TestContext.RenderComponent<FluentNumberField<int>>(
                    parameters => parameters
                        .AddChildContent(childContent)
                        .Bind(p => p.Value, currentValue, newValue => currentValue = 101)
                        .Add(p => p.Min, min)
                        .Add(p => p.Max, max));
            };

            // Assert
            action.Should().Throw<Exception>();
        }
        

        [Fact]
        public void Throw_WhenLongMin_IsOutOfRange()
        {
            // Act
            string childContent = "100";
            long currentValue = 100;
            string min = $"{long.MinValue}1";
            Action action = () =>
            {
                TestContext.RenderComponent<FluentNumberField<long>>(
                    parameters => parameters
                        .AddChildContent(childContent)
                        .Bind(p => p.Value, currentValue, newValue => currentValue = 101)
                        .Add(p => p.Min, min));
            };

            // Assert
            action.Should().Throw<OverflowException>();
        }

        [Fact]
        public void Throw_WhenLongMax_IsOutOfRange()
        {
            // Act
            string childContent = "100";
            long currentValue = 100;
            string max = $"{long.MaxValue}1";
            Action action = () =>
            {
                TestContext.RenderComponent<FluentNumberField<long>>(
                    parameters => parameters
                        .AddChildContent(childContent)
                        .Bind(p => p.Value, currentValue, newValue => currentValue = 101)
                        .Add(p => p.Max, max));
            };

            // Assert
            action.Should().Throw<OverflowException>();
        }

        [Fact]
        public void Throw_WhenLongMaxIsSmallerThanMin()
        {
            // Act
            string childContent = "100";
            long currentValue = 100;
            string min = $"{1000L}";
            string max = $"{500L}";
            Action action = () =>
            {
                TestContext.RenderComponent<FluentNumberField<long>>(
                    parameters => parameters
                        .AddChildContent(childContent)
                        .Bind(p => p.Value, currentValue, newValue => currentValue = 101)
                        .Add(p => p.Min, min)
                        .Add(p => p.Max, max));
            };

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Throw_WhenShortMin_IsOutOfRange()
        {
            // Act
            string childContent = "100";
            short currentValue = 100;
            string min = $"{short.MinValue}1";
            Action action = () =>
            {
                TestContext.RenderComponent<FluentNumberField<short>>(
                    parameters => parameters
                        .AddChildContent(childContent)
                        .Bind(p => p.Value, currentValue, newValue => currentValue = 101)
                        .Add(p => p.Min, min));
            };

            // Assert
            action.Should().Throw<OverflowException>();
        }

        [Fact]
        public void Throw_WhenShortMax_IsOutOfRange()
        {
            // Act
            string childContent = "100";
            short currentValue = 100;
            string max = $"{short.MaxValue}1";
            Action action = () =>
            {
                TestContext.RenderComponent<FluentNumberField<short>>(
                    parameters => parameters
                        .AddChildContent(childContent)
                        .Bind(p => p.Value, currentValue, newValue => currentValue = 101)
                        .Add(p => p.Max, max));
            };

            // Assert
            action.Should().Throw<OverflowException>();
        }

        [Fact]
        public void Throw_WhenShortMaxIsSmallerThanMin()
        {
            // Act
            string childContent = "100";
            short currentValue = 100;
            string min = $"{1000}";
            string max = $"{500}";
            Action action = () =>
            {
                TestContext.RenderComponent<FluentNumberField<short>>(
                    parameters => parameters
                        .AddChildContent(childContent)
                        .Bind(p => p.Value, currentValue, newValue => currentValue = 101)
                        .Add(p => p.Min, min)
                        .Add(p => p.Max, max));
            };

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Throw_WhenFloatMaxIsSmallerThanMin()
        {
            // Act
            string childContent = "100";
            float currentValue = 100;
            string min = $"{100.0f}";
            string max = $"{50.0f}";
            Action action = () =>
            {
                TestContext.RenderComponent<FluentNumberField<float>>(
                    parameters => parameters
                        .AddChildContent(childContent)
                        .Bind(p => p.Value, currentValue, newValue => currentValue = 101)
                        .Add(p => p.Min, min)
                        .Add(p => p.Max, max));
            };

            // Assert
            action.Should().Throw<ArgumentException>();
        }
        
        [Fact]
        public void Throw_WhenDoubleMaxIsSmallerThanMin()
        {
            // Act
            string childContent = "100";
            double currentValue = 1.4;
            string min = $"{10.1}";
            string max = $"{5.2}";
            Action action = () =>
            {
                TestContext.RenderComponent<FluentNumberField<double>>(
                    parameters => parameters
                        .AddChildContent(childContent)
                        .Bind(p => p.Value, currentValue, newValue => currentValue = 101)
                        .Add(p => p.Min, min)
                        .Add(p => p.Max, max));
            };

            // Assert
            action.Should().Throw<ArgumentException>();
        }
        
        [Fact]
        public void Throw_WhenDecimalMaxIsSmallerThanMin()
        {
            // Act
            string childContent = "100";
            decimal currentValue = 1.1E6m;
            string min = $"{1.5E6m}";
            string max = $"{1.4E6m}";
            Action action = () =>
            {
                TestContext.RenderComponent<FluentNumberField<decimal>>(
                    parameters => parameters
                        .AddChildContent(childContent)
                        .Bind(p => p.Value, currentValue, newValue => currentValue = 101)
                        .Add(p => p.Min, min)
                        .Add(p => p.Max, max));
            };

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void RenderProperly_ClassParameter()
        {
            // Arrange && Act
            string childContent = "100";
            int currentValue = 100;
            string cssClass = "additional-css-class";
            IRenderedComponent<FluentNumberField<int>> sut = TestContext.RenderComponent<FluentNumberField<int>>(
                parameters => parameters
                    .AddChildContent(childContent)
                    .Bind(p => p.Value, currentValue, newValue => currentValue = 101)
                    .Add(p => p.Class, cssClass));

            // Assert
            sut.MarkupMatches("<fluent-number-field " +
                              "maxlength=\"14\" " +
                              "minlength=\"1\" " +
                              "size=\"20\" " +
                              "step=\"1\" " +
                              "max=\"2147483647\" " +
                              "min=\"-2147483648\" " +
                              "value=\"100\" " +
                              "current-value=\"100\" " +
                              $"class={cssClass}>" +
                              "100" +
                              "</fluent-number-field>");
        }

        [Fact]
        public void RenderProperly_StyleParameter()
        {
            // Arrange && Act
            string childContent = "100";
            int currentValue = 100;
            string style = "background-color: red";
            IRenderedComponent<FluentNumberField<int>> sut = TestContext.RenderComponent<FluentNumberField<int>>(
                parameters => parameters
                    .AddChildContent(childContent)
                    .Bind(p => p.Value, currentValue, newValue => currentValue = 101)
                    .Add(p => p.Style, style));

            // Assert
            sut.MarkupMatches("<fluent-number-field " +
                              "maxlength=\"14\" " +
                              "minlength=\"1\" " +
                              "size=\"20\" " +
                              "step=\"1\" " +
                              "max=\"2147483647\" " +
                              "min=\"-2147483648\" " +
                              "value=\"100\" " +
                              "current-value=\"100\" " +
                              $"style=\"{style}\">" +
                              "100" +
                              "</fluent-number-field>");
        }

        [Fact]
        public void RenderProperly_AdditionalParameter()
        {
            // Arrange && Act
            string childContent = "100";
            int currentValue = 100;
            string additionalParameterName = "additional-parameter-name";
            string additionalParameterValue = "additional-parameter-value";
            IRenderedComponent<FluentNumberField<int>> sut = TestContext.RenderComponent<FluentNumberField<int>>(
                parameters => parameters
                    .AddChildContent(childContent)
                    .Bind(p => p.Value, currentValue, newValue => currentValue = 101)
                    .AddUnmatched(additionalParameterName, additionalParameterValue));

            // Assert
            sut.MarkupMatches("<fluent-number-field " +
                              "maxlength=\"14\" " +
                              "minlength=\"1\" " +
                              "size=\"20\" " +
                              "step=\"1\" " +
                              "max=\"2147483647\" " +
                              "min=\"-2147483648\" " +
                              "value=\"100\" " +
                              "current-value=\"100\" " +
                              $"{additionalParameterName}=\"{additionalParameterValue}\">" +
                              "100" +
                              "</fluent-number-field>");
        }

        [Fact]
        public void RenderProperly_AdditionalParameters()
        {
            // Arrange && Act
            string childContent = "100";
            int currentValue = 100;
            string additionalParameter1Name = "additional-parameter1-name";
            string additionalParameter1Value = "additional-parameter1-value";
            string additionalParameter2Name = "additional-parameter2-name";
            string additionalParameter2Value = "additional-parameter2-value";
            IRenderedComponent<FluentNumberField<int>> sut = TestContext.RenderComponent<FluentNumberField<int>>(
                parameters => parameters
                    .AddChildContent(childContent)
                    .Bind(p => p.Value, currentValue, newValue => currentValue = 101)
                    .AddUnmatched(additionalParameter1Name, additionalParameter1Value)
                    .AddUnmatched(additionalParameter2Name, additionalParameter2Value));

            // Assert
            sut.MarkupMatches("<fluent-number-field " +
                              "maxlength=\"14\" " +
                              "minlength=\"1\" " +
                              "size=\"20\" " +
                              "step=\"1\" " +
                              "max=\"2147483647\" " +
                              "min=\"-2147483648\" " +
                              "value=\"100\" " +
                              "current-value=\"100\" " +
                              $"{additionalParameter1Name}=\"{additionalParameter1Value}\" " +
                              $"{additionalParameter2Name}=\"{additionalParameter2Value}\">" +
                              "100" +
                              "</fluent-number-field>");
        }
    }
}