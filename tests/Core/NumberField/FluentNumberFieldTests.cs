using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.NumberField;

public class FluentNumberFieldTests : TestBase
{
    [Fact]
    public void FluentNumberField_Default()
    {
        int currentValue = 100;

        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentNumberField<int>>(parameters =>
        {
            parameters.Bind(p => p.Value, currentValue, newValue => currentValue = 101);
            parameters.AddChildContent("100");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNumberField_ReadOnlyParameter()
    {
        int currentValue = 100;

        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentNumberField<int>>(parameters =>
        {
            parameters.Add(p => p.ReadOnly, true);
            parameters.Bind(p => p.Value, currentValue, newValue => currentValue = 101);
            parameters.AddChildContent("100");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNumberField_AutoFocusParameter()
    {
        int currentValue = 100;

        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentNumberField<int>>(parameters =>
        {
            parameters.Add(p => p.Autofocus, true);
            parameters.Bind(p => p.Value, currentValue, newValue => currentValue = 101);
            parameters.AddChildContent("100");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNumberField_HideStepParameter()
    {
        int currentValue = 100;

        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentNumberField<int>>(parameters =>
        {
            parameters.Add(p => p.HideStep, true);
            parameters.Bind(p => p.Value, currentValue, newValue => currentValue = 101);
            parameters.AddChildContent("100");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNumberField_PlaceholderParameter()
    {
        int currentValue = 100;

        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentNumberField<int>>(parameters =>
        {
            parameters.Add(p => p.Placeholder, "placeholder-value");
            parameters.Bind(p => p.Value, currentValue, newValue => currentValue = 101);
            parameters.AddChildContent("100");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNumberField_DataListParameter()
    {
        int currentValue = 100;

        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentNumberField<int>>(parameters =>
        {
            parameters.Add(p => p.DataList, "datalist");
            parameters.Bind(p => p.Value, currentValue, newValue => currentValue = 101);
            parameters.AddChildContent("100");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNumberField_MaxLengthParameter()
    {
        int currentValue = 100;

        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentNumberField<int>>(parameters =>
        {
            parameters.Add(p => p.MaxLength, 10);
            parameters.Bind(p => p.Value, currentValue, newValue => currentValue = 101);
            parameters.AddChildContent("100");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNumberField_MinLengthParameter()
    {
        int currentValue = 100;

        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentNumberField<int>>(parameters =>
        {
            parameters.Add(p => p.MinLength, 3);
            parameters.Bind(p => p.Value, currentValue, newValue => currentValue = 101);
            parameters.AddChildContent("100");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNumberField_SizeParameter()
    {
        int currentValue = 100;

        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentNumberField<int>>(parameters =>
        {
            parameters.Add(p => p.Size, 3);
            parameters.Bind(p => p.Value, currentValue, newValue => currentValue = 101);
            parameters.AddChildContent("100");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNumberField_StepParameter()
    {
        int currentValue = 100;

        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentNumberField<int>>(parameters =>
        {
            parameters.Add(p => p.Step, "3");
            parameters.Bind(p => p.Value, currentValue, newValue => currentValue = 101);
            parameters.AddChildContent("100");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNumberField_MaxParameter()
    {
        int currentValue = 100;

        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentNumberField<int>>(parameters =>
        {
            parameters.Add(p => p.Max, "2147483647");
            parameters.Bind(p => p.Value, currentValue, newValue => currentValue = 101);
            parameters.AddChildContent("100");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNumberField_MinParameter()
    {
        int currentValue = 100;

        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentNumberField<int>>(parameters =>
        {
            parameters.Add(p => p.Min, "-2147483648");
            parameters.Bind(p => p.Value, currentValue, newValue => currentValue = 101);
            parameters.AddChildContent("100");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNumberField_IdParameter()
    {
        int currentValue = 100;

        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentNumberField<int>>(parameters =>
        {
            parameters.Add(p => p.Id, "some-unique-id");
            parameters.Bind(p => p.Value, currentValue, newValue => currentValue = 101);
            parameters.AddChildContent("100");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNumberField_DisabledParameter()
    {
        int currentValue = 100;

        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentNumberField<int>>(parameters =>
        {
            parameters.Add(p => p.Disabled, true);
            parameters.Bind(p => p.Value, currentValue, newValue => currentValue = 101);
            parameters.AddChildContent("100");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNumberField_NameParameter()
    {
        int currentValue = 100;

        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentNumberField<int>>(parameters =>
        {
            parameters.Add(p => p.Name, "name");
            parameters.Bind(p => p.Value, currentValue, newValue => currentValue = 101);
            parameters.AddChildContent("100");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNumberField_RequiredParameter()
    {
        int currentValue = 100;

        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentNumberField<int>>(parameters =>
        {
            parameters.Add(p => p.Required, true);
            parameters.Bind(p => p.Value, currentValue, newValue => currentValue = 101);
            parameters.AddChildContent("100");
        });

        // Assert
        cut.Verify();
    }

    [Theory]
    [InlineData(FluentInputAppearance.Filled)]
    [InlineData(FluentInputAppearance.Outline)]
    public void FluentNumberField_AppearanceParameter(FluentInputAppearance appearance)
    {
        int currentValue = 100;

        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentNumberField<int>>(parameters =>
        {
            parameters.Add(p => p.Appearance, appearance);
            parameters.Bind(p => p.Value, currentValue, newValue => currentValue = 101);
            parameters.AddChildContent("100");
        });

        // Assert
        cut.Verify(suffix: appearance.ToString());
    }

    [Fact]
    public void FluentNumberField_Throw_WhenIntMaxIsSmallerThanMin()
    {
        int currentValue = 100;

        // Act
        Action action = () =>
        {
            TestContext.RenderComponent<FluentNumberField<int>>(parameters =>
            {
                parameters.Add(p => p.Min, $"1000");
                parameters.Add(p => p.Max, $"500");
                parameters.Bind(p => p.Value, currentValue, newValue => currentValue = 101);
                parameters.AddChildContent("100");
            });
        };

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("", "2147483647")]
    [InlineData(" ", "2147483647")]
    public void FluentNumberField_Throw_WhenIntMinOrMax_NullOrEmptyOrWhitespace(string min, string max)
    {
        int currentValue = 100;

        // Act
        Action action = () =>
        {
            TestContext.RenderComponent<FluentNumberField<int>>(parameters =>
            {
                parameters.Add(p => p.Min, min);
                parameters.Add(p => p.Max, max);
                parameters.Bind(p => p.Value, currentValue, newValue => currentValue = 101);
                parameters.AddChildContent("100");
            });
        };

        // Assert
        action.Should().Throw<Exception>();
    }

    [Fact]
    public void FluentNumberField_Throw_WhenLongMaxIsSmallerThanMin()
    {
        long currentValue = 100;

        // Act
        Action action = () =>
        {
            TestContext.RenderComponent<FluentNumberField<long>>(parameters =>
            {
                parameters.Add(p => p.Min, $"{1000L}");
                parameters.Add(p => p.Max, $"{500L}");
                parameters.Bind(p => p.Value, currentValue, newValue => currentValue = 101);
                parameters.AddChildContent("100");
            });
        };

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void FluentNumberField_Throw_WhenShortMaxIsSmallerThanMin()
    {
        int currentValue = 100;

        // Act
        Action action = () =>
        {
            TestContext.RenderComponent<FluentNumberField<int>>(parameters =>
            {
                parameters.Add(p => p.Min, $"{1000}");
                parameters.Add(p => p.Max, $"{500}");
                parameters.Bind(p => p.Value, currentValue, newValue => currentValue = 101);
                parameters.AddChildContent("100");
            });
        };

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void FluentNumberField_Throw_WhenFloatMaxIsSmallerThanMin()
    {
        float currentValue = 100;

        // Act
        Action action = () =>
        {
            TestContext.RenderComponent<FluentNumberField<float>>(parameters =>
            {
                parameters.Add(p => p.Min, $"{100.0f}");
                parameters.Add(p => p.Max, $"{50.0f}");
                parameters.Bind(p => p.Value, currentValue, newValue => currentValue = 101);
                parameters.AddChildContent("100");
            });
        };

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void FluentNumberField_Throw_WhenDoubleMaxIsSmallerThanMin()
    {
        double currentValue = 1.4;

        // Act
        Action action = () =>
        {
            TestContext.RenderComponent<FluentNumberField<double>>(parameters =>
            {
                parameters.Add(p => p.Min, $"{10.1}");
                parameters.Add(p => p.Max, $"{5.2}");
                parameters.Bind(p => p.Value, currentValue, newValue => currentValue = 101);
                parameters.AddChildContent("100");
            });
        };

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void FluentNumberField_Throw_WhenDecimalMaxIsSmallerThanMin()
    {
        decimal currentValue = 1.1E6m;

        // Act
        Action action = () =>
        {
            TestContext.RenderComponent<FluentNumberField<decimal>>(parameters =>
            {
                parameters.Add(p => p.Min, $"{1.5E6m}");
                parameters.Add(p => p.Max, $"{1.4E6m}");
                parameters.Bind(p => p.Value, currentValue, newValue => currentValue = 101);
                parameters.AddChildContent("100");
            });
        };

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void FluentNumberField_ClassParameter()
    {
        int currentValue = 100;

        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentNumberField<int>>(parameters =>
        {
            parameters.Add(p => p.Class, "additional-css-class");
            parameters.Bind(p => p.Value, currentValue, newValue => currentValue = 101);
            parameters.AddChildContent("100");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNumberField_StyleParameter()
    {
        int currentValue = 100;

        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentNumberField<int>>(parameters =>
        {
            parameters.Add(p => p.Style, "background-color: red");
            parameters.Bind(p => p.Value, currentValue, newValue => currentValue = 101);
            parameters.AddChildContent("100");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNumberField_AdditionalParameter()
    {
        int currentValue = 100;

        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentNumberField<int>>(parameters =>
        {
            parameters.AddUnmatched("additional-parameter-name", "additional-parameter-value");
            parameters.Bind(p => p.Value, currentValue, newValue => currentValue = 101);
            parameters.AddChildContent("100");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNumberField_AdditionalParameters()
    {
        int currentValue = 100;

        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentNumberField<int>>(parameters =>
        {
            parameters.AddUnmatched("additional-parameter1-name", "additional-parameter1-value");
            parameters.AddUnmatched("additional-parameter2-name", "additional-parameter2-value");
            parameters.Bind(p => p.Value, currentValue, newValue => currentValue = 101);
            parameters.AddChildContent("100");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNumberField_LabelTemplate()
    {
        int currentValue = 100;

        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentNumberField<int>>(parameters =>
        {
            parameters.Bind(p => p.Value, currentValue, newValue => currentValue = 101);
            parameters.Add(p => p.LabelTemplate, "<h1>My label</h1>");
            parameters.AddChildContent("100");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNumberField_Label()
    {
        int currentValue = 100;
        TestContext.JSInterop.Mode = JSRuntimeMode.Loose;

        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentNumberField<int>>(parameters =>
        {
            parameters.Bind(p => p.Value, currentValue, newValue => currentValue = 101);
            parameters.Add(p => p.Label, "My label");
            parameters.AddChildContent("100");
        });

        // Assert
        cut.Verify();
    }
}