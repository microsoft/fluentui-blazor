using Bunit;
using FluentAssertions;
using Microsoft.FluentUI.AspNetCore.Components.Tests.Extensions;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Button;

public partial class FluentButtonTests : TestContext
{
    private static TestContext TestContext => new(); // TODO: To remove and to use the `RenderComponent` inherited method.

    public FluentButtonTests()
    {
        TestContext.JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [Fact]
    public void FluentButton_Default()
    {
        // Arrange
        using var id = Identifier.SequentialContext();

        // Act
        var cut = TestContext.RenderComponent<FluentButton>(parameters =>
        {
            parameters.AddChildContent("fluent-button");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentButton_AutofocusAttribute()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentButton>(parameters =>
        {
            parameters.AddChildContent("fluent-button");
            parameters.Add(p => p.Autofocus, true);
        });

        // Assert
        cut.Verify();
    }

    [Theory]
    [InlineData("form-id-attribute")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void FluentButton_FormIdAttribute(string? formId)
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentButton>(parameters =>
        {
            parameters.Add(p => p.FormId, formId);
            parameters.AddChildContent("fluent-button");
        });

        // Assert
        cut.Verify(suffix: formId);
    }

    [Theory]
    [InlineData("submit")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void FluentButton_FormActionAttribute(string? formAction)
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentButton>(parameters =>
        {
            parameters.Add(p => p.Action, formAction);
            parameters.AddChildContent("fluent-button");
        });

        // Assert
        cut.Verify(suffix: formAction);
    }

    [Theory]
    [InlineData("multipart/form-data")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void FluentButton_FormEnctypeAttribute(string? formEnctype)
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentButton>(parameters =>
        {
            parameters.Add(p => p.Enctype, formEnctype);
            parameters.AddChildContent("fluent-button");
        });

        // Assert
        cut.Verify(suffix: formEnctype);
    }

    [Theory]
    [InlineData("post")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void FluentButton_FormMethodAttribute(string? formMethod)
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentButton>(parameters =>
        {
            parameters.Add(p => p.Method, formMethod);
            parameters.AddChildContent("fluent-button");
        });

        // Assert
        cut.Verify(suffix: formMethod);
    }

    [Fact]
    public void FluentButton_FormNovalidateAttribute()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentButton>(parameters =>
        {
            parameters.Add(p => p.NoValidate, true);
            parameters.AddChildContent("fluent-button");
        });

        // Assert
        cut.Verify();
    }

    [Theory]
    [InlineData("_self")]
    [InlineData("_blank")]
    [InlineData("_parent")]
    [InlineData("_top")]
    [InlineData("")]
    public void FluentButton_FormTargetAttribute(string? formTarget)
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentButton>(parameters =>
        {
            parameters.Add(p => p.Target, formTarget);
            parameters.AddChildContent("fluent-button");
        });

        // Assert
        cut.Verify(suffix: formTarget);
    }

    [Theory]
    [InlineData(" ")]
    [InlineData("_funky")]
    public void Throw_ArgumentException_When_FormTargetAttribute_IsInvalid(string? formTarget)
    {
        // Arrange && Act
        Action action = () =>
        {
            var cut = TestContext.RenderComponent<FluentButton>(parameters =>
            {
                parameters.Add(p => p.Target, formTarget);
                parameters.AddChildContent("fluent-button");
            });
        };

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(ButtonType.Button)]
    [InlineData(ButtonType.Reset)]
    [InlineData(ButtonType.Submit)]
    public void FluentButton_TypeAttribute(ButtonType buttonType)
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentButton>(parameters =>
        {
            parameters.Add(p => p.Type, buttonType);
            parameters.AddChildContent("fluent-button");
        });

        // Assert
        cut.Verify(suffix: buttonType.ToString());
    }

    [Theory]
    [InlineData("id-value")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void FluentButton_IdAttribute(string? id)
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentButton>(parameters =>
        {
            parameters.Add(p => p.Id, id);
            parameters.AddChildContent("fluent-button");
        });

        // Assert
        cut.Verify(suffix: id);
    }

    [Theory]
    [InlineData("some-value")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void FluentButton_ValueAttribute(string? value)
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentButton>(parameters =>
        {
            parameters.Add(p => p.Value, value);
            parameters.AddChildContent("fluent-button");
        });

        // Assert
        cut.Verify(suffix: value);
    }

    [Theory]
    [InlineData("some-value")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void FluentButton_CurrentValueAttribute(string? currentValue)
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentButton>(parameters =>
        {
            parameters.Add(p => p.CurrentValue, currentValue);
            parameters.AddChildContent("fluent-button");
        });

        // Assert
        cut.Verify(suffix: currentValue);
    }

    [Fact]
    public void FluentButton_DisabledAttribute()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentButton>(parameters =>
        {
            parameters.Add(p => p.Disabled, true);
            parameters.AddChildContent("fluent-button");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentButton_NameAttribute()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentButton>(parameters =>
        {
            parameters.Add(p => p.Name, "name-value");
            parameters.AddChildContent("fluent-button");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentButton_RequiredAttribute()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentButton>(parameters =>
        {
            parameters.Add(p => p.Required, true);
            parameters.AddChildContent("fluent-button");
        });

        // Assert
        cut.Verify();
    }

    [Theory]
    [InlineData(Appearance.Accent)]
    [InlineData(Appearance.Lightweight)]
    [InlineData(Appearance.Neutral)]
    [InlineData(Appearance.Outline)]
    [InlineData(Appearance.Stealth)]
    public void FluentButton_AppearanceAttribute(Appearance appearance)
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentButton>(parameters =>
        {
            parameters.Add(p => p.Appearance, appearance);
            parameters.AddChildContent("fluent-button");
        });

        // Assert
        cut.Verify(suffix: appearance.ToString());
    }

    [Fact]
    public void FluentButton_ClassAttribute()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentButton>(parameters =>
        {
            parameters.Add(p => p.Class, "additional-class");
            parameters.AddChildContent("fluent-button");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentButton_StyleAttribute()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentButton>(parameters =>
        {
            parameters.Add(p => p.Style, "background-color: green;");
            parameters.AddChildContent("fluent-button");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentButton_AdditionalAttribute()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentButton>(parameters =>
        {
            parameters.AddUnmatched("additional-attribute-name", "additional-attribute-value");
            parameters.AddChildContent("fluent-button");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentButton_AdditionalAttributes()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentButton>(parameters =>
        {
            parameters.AddUnmatched("additional-attribute1-name", "additional-attribute1-value");
            parameters.AddUnmatched("additional-attribute2-name", "additional-attribute2-value");
            parameters.AddChildContent("fluent-button");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentButton_IconStart()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentButton>(parameters =>
        {
            parameters.Add(p => p.IconStart, SampleIcons.Info);
            parameters.AddChildContent("My button");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentButton_IconEnd()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentButton>(parameters =>
        {
            parameters.Add(p => p.IconEnd, SampleIcons.Info);
            parameters.AddChildContent("My button");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentButton_IconNoContent()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentButton>(parameters =>
        {
            parameters.Add(p => p.IconEnd, SampleIcons.Info);
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentButton_OnClick_Disabled()
    {
        var clicked = false;

        // Arrange
        var cut = TestContext.RenderComponent<FluentButton>(parameters =>
        {
            parameters.Add(p => p.OnClick, (e) => { clicked = true; });
            parameters.AddChildContent("My button");
        });

        // Act - `InvokeAsync` to avoid "The current thread is not associated with the Dispatcher" error.
        cut.InvokeAsync(() => cut.Instance.SetDisabled(true));
        cut.Find("fluent-button").Click();

        // Assert
        Assert.False(clicked);
    }
}
