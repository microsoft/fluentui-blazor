using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.NumberField;

public class FluentNumberFieldTests : TestBase
{
    [Inject]
    private LibraryConfiguration LibraryConfiguration { get; set; } = new LibraryConfiguration();

    public FluentNumberFieldTests()
    {
        TestContext.Services.AddSingleton(LibraryConfiguration);
    }

    [Fact]
    public void FluentNumberField_Default()
    {
        var currentValue = 100;

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
        var currentValue = 100;

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
        var currentValue = 100;

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
        var currentValue = 100;

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
        var currentValue = 100;

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
        var currentValue = 100;

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
        var currentValue = 100;

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
        var currentValue = 100;

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
        var currentValue = 100;

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
        var currentValue = 100;

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
        var currentValue = 100;

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
        var currentValue = 100;

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
        var currentValue = 100;

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
        var currentValue = 100;

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
        var currentValue = 100;

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
        var currentValue = 100;

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
        var currentValue = 100;

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
        var currentValue = 100;

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
        var currentValue = 100;

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
        var currentValue = 100;

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
        var currentValue = 1.4;

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
        var currentValue = 1.1E6m;

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
        var currentValue = 100;

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
        var currentValue = 100;

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
        var currentValue = 100;

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
        var currentValue = 100;

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
        var currentValue = 100;

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
        var currentValue = 100;
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
