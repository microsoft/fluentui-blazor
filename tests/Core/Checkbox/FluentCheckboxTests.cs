using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Checkbox;

public class FluentCheckboxTests : TestBase
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void FluentCheckbox_DefaultValues(bool currentValue)
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentCheckbox>(parameters =>
        {
            parameters.Bind(bind => bind.Value, currentValue, newValue => currentValue = false);
            parameters.AddChildContent("childContent");
        });

        // Assert
        cut.Verify(suffix: currentValue.ToString());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void FluentCheckbox_ReadonlyParameter(bool currentValue)
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentCheckbox>(parameters =>
        {
            parameters.Bind(p => p.Value, currentValue, newValue => currentValue = false);
            parameters.Add(p => p.ReadOnly, true);
            parameters.AddChildContent("childContent");
        });

        // Assert
        cut.Verify(suffix: currentValue.ToString());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("id-parameter")]
    public void FluentCheckbox_IdParameter(string? idParameter)
    {
        var currentValue = true;

        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentCheckbox>(parameters =>
        {
            parameters.Bind(p => p.Value, currentValue, newValue => currentValue = true);
            parameters.Add(p => p.Id, idParameter);
            parameters.AddChildContent("childContent");
        });

        // Assert
        cut.Verify(suffix: idParameter);
    }

    [Fact]
    public void FluentCheckbox_DisabledParameter()
    {
        var currentValue = true;

        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentCheckbox>(parameters =>
        {
            parameters.Bind(p => p.Value, currentValue, newValue => currentValue = false);
            parameters.Add(p => p.Disabled, true);
            parameters.AddChildContent("childContent");
        });

        // Assert
        cut.Verify();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("name-parameter")]
    public void FluentCheckbox_NameParameter(string? nameParameter)
    {
        var currentValue = true;

        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentCheckbox>(parameters =>
        {
            parameters.Bind(p => p.Value, currentValue, newValue => currentValue = false);
            parameters.Add(p => p.Name, nameParameter);
            parameters.AddChildContent("childContent");
        });

        // Assert
        cut.Verify(suffix: nameParameter);
    }

    [Fact]
    public void FluentCheckbox_RequiredParameter()
    {
        var currentValue = true;

        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentCheckbox>(parameters =>
        {
            parameters.Bind(p => p.Value, currentValue, newValue => currentValue = false);
            parameters.Add(p => p.Required, true);
            parameters.AddChildContent("childContent");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCheckbox_ClassParameter()
    {
        var currentValue = true;

        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentCheckbox>(parameters =>
        {
            parameters.Bind(p => p.Value, currentValue, newValue => currentValue = false);
            parameters.Add(p => p.Class, "additional-css-class");
            parameters.AddChildContent("childContent");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCheckbox_StyleParameter()
    {
        var currentValue = true;

        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentCheckbox>(parameters =>
        {
            parameters.Bind(p => p.Value, currentValue, newValue => currentValue = false);
            parameters.Add(p => p.Style, "background-color: red");
            parameters.AddChildContent("childContent");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCheckbox_AdditionalParameter()
    {
        var currentValue = true;

        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentCheckbox>(parameters =>
        {
            parameters.Bind(p => p.Value, currentValue, newValue => currentValue = false);
            parameters.AddUnmatched("parameterName", "parameterValue");
            parameters.AddChildContent("childContent");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCheckbox_AdditionalParameters()
    {
        var currentValue = true;

        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentCheckbox>(parameters =>
        {
            parameters.Bind(p => p.Value, currentValue, newValue => currentValue = false);
            parameters.AddUnmatched("parameter1Name", "parameter1Value");
            parameters.AddUnmatched("parameter2Name", "parameter2Value");
            parameters.AddChildContent("childContent");
        });

        // Assert
        cut.Verify();
    }
}
