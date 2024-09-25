using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components.Tests.Extensions;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.MenuButton;

public class FluentMenuButtonTests : TestBase
{
    public FluentMenuButtonTests()
    {
        TestContext.Services.AddSingleton(LibraryConfiguration.ForUnitTests);
    }

    [Fact]
    public void FluentMenuButton_Default()
    {
        //Arrange
        FluentButton button = default!;
        FluentMenu menu = default!;
        string text = default!;
        string buttonStyle = default!;
        string menuStyle = default!;
        Dictionary<string, string> items = default!;
        Action<MenuChangeEventArgs> onMenuChanged = _ => { };
        var cut = TestContext.RenderComponent<FluentMenuButton>(parameters => parameters
            .Add(p => p.Button, button)
            .Add(p => p.Menu, menu)
            .Add(p => p.Text, text)
            .Add(p => p.ButtonStyle, buttonStyle)
            .Add(p => p.MenuStyle, menuStyle)
            .Add(p => p.Items, items)
            .Add(p => p.OnMenuChanged, onMenuChanged)
            .Add(p => p.IconStart, SampleIcons.Info)
        );
        //Act

        //Assert
        cut.Verify();
    }
}

