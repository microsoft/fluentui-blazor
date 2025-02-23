using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Search;

public class FluentSearchTests : TestContext
{
    public FluentSearchTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddSingleton(LibraryConfiguration.ForUnitTests);
    }


    [Fact]
    public void FluentSearch_Default()
    {
        //Arrange
        var cut = RenderComponent<FluentSearch>();

        //Act

        //Assert
        cut.Verify();
    }

    [Fact]
    public void FluentSearch_WithLabel()
    {
        //Arrange
        string label = "With a label";
        var cut = RenderComponent<FluentSearch>(parameters =>
        {
            parameters.Add(s => s.Label, label);
        });

        //Act

        //Assert
        cut.Verify();
    }

    [Theory]
    [InlineData(FluentInputAppearance.Outline)]
    [InlineData(FluentInputAppearance.Filled)]
    public void FluentSearch_AppearanceAttribute(FluentInputAppearance appearance)
    {
        // Arrange & Act
        var cut = RenderComponent<FluentSearch>(parameters =>
        {
            parameters.Add(p => p.Appearance, appearance);
        });

        // Assert
        cut.Verify(suffix: appearance.ToString());
    }

    [Fact]
    public void FluentSearch_MaxLengthAttribute()
    {
        // Arrange & Act
        var cut = RenderComponent<FluentSearch>(parameters =>
        {
            parameters.Add(p => p.Maxlength, 10);
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentSearch_MinLengthAttribute()
    {
        // Arrange & Act
        var cut = RenderComponent<FluentSearch>(parameters =>
        {
            parameters.Add(p => p.Minlength, 5);
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentSearch_PatternAttribute()
    {
        // Arrange & Act
        var cut = RenderComponent<FluentSearch>(parameters =>
        {
            parameters.Add(p => p.Pattern, "[A-Za-z]{3}");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentSearch_SizeAttribute()
    {
        // Arrange & Act
        var cut = RenderComponent<FluentSearch>(parameters =>
        {
            parameters.Add(p => p.Size, 10);
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentSearch_SpellcheckAttribute()
    {
        // Arrange & Act
        var cut = RenderComponent<FluentSearch>(parameters =>
        {
            parameters.Add(p => p.Spellcheck, true);
        });

        // Assert
        cut.Verify();
    }


    [Fact]
    public void FluentSearch_DataListAttribute()
    {
        // Arrange & Act
        var cut = RenderComponent<FluentSearch>(parameters =>
        {
            parameters.Add(p => p.DataList, "datalist-id");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentSearch_ChildContent()
    {
        // Arrange & Act

        var cut = RenderComponent<FluentSearch>(parameters =>
        {
            parameters.AddChildContent("<div>Child content</div>");
        });

        // Assert
        cut.Verify();
    }

    [Theory]
    [InlineData("id-value")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void FluentSearch_IdAttribute(string id)
    {
        // Arrange & Act
        var cut = RenderComponent<FluentSearch>(parameters =>
        {
            parameters.Add(p => p.Id, id);
        });

        // Assert
        cut.Verify(suffix: id);
    }

    [Theory]
    [InlineData("some-value")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void FluentSearch_ValueAttribute(string value)
    {
        // Arrange & Act
        var cut = RenderComponent<FluentSearch>(parameters =>
        {
            parameters.Add(p => p.Value, value);
        });

        // Assert
        cut.Verify(suffix: value);
    }

    [Fact]
    public void FluentSearch_RequiredAttribute()
    {
        // Arrange & Act
        var cut = RenderComponent<FluentSearch>(parameters =>
        {
            parameters.Add(p => p.Required, true);
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentSearch_DisabledAttribute()
    {
        // Arrange & Act
        var cut = RenderComponent<FluentSearch>(parameters =>
        {
            parameters.Add(p => p.Disabled, true);
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentSearch_ReadOnlyAttribute()
    {
        // Arrange & Act
        var cut = RenderComponent<FluentSearch>(parameters =>
        {
            parameters.Add(p => p.ReadOnly, true);
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentSearch_AutoFocusAttribute()
    {
        // Arrange & Act
        var cut = RenderComponent<FluentSearch>(parameters =>
        {
            parameters.Add(p => p.Autofocus, true);
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentSearch_PlaceholderAttribute()
    {
        // Arrange & Act
        var cut = RenderComponent<FluentSearch>(parameters =>
        {
            parameters.Add(p => p.Placeholder, "Enter text here");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentSearch_NameAttribute()
    {
        // Arrange & Act
        var cut = RenderComponent<FluentSearch>(parameters =>
        {
            parameters.Add(p => p.Name, "name");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentSearch_AdditionalAttributes()
    {
        // Arrange & Act
        var cut = RenderComponent<FluentSearch>(parameters =>
        {
            parameters.AddUnmatched("additional-attribute-name", "additional-attribute-value");
        });

        // Assert
        cut.Verify();
    }

}

