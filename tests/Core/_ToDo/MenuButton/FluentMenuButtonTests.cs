// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components.Tests.Extensions;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.MenuButton;

public class FluentMenuButtonTests : TestBase
{

    public GlobalState GlobalState { get; set; } = new GlobalState();

    public FluentMenuButtonTests()
    {
        TestContext.Services.AddSingleton(LibraryConfiguration.ForUnitTests);
        TestContext.Services.AddSingleton(GlobalState);
        TestContext.JSInterop.SetupModule("./_content/Microsoft.FluentUI.AspNetCore.Components/Components/Menu/FluentMenu.razor.js");
        TestContext.JSInterop.SetupModule("./_content/Microsoft.FluentUI.AspNetCore.Components/Components/AnchoredRegion/FluentAnchoredRegion.razor.js");

        var menuButtonModule = TestContext.JSInterop.SetupModule("./_content/Microsoft.FluentUI.AspNetCore.Components/Components/MenuButton/FluentMenuButton.razor.js");
        menuButtonModule.SetupVoid("initialize", _ => true);
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
        );
        //Act

        //Assert
        cut.Verify();
    }

    [Fact]
    public void FluentMenuButton_IconStart()
    {
        //Arrange
        FluentButton button = default!;
        FluentMenu menu = default!;
        string text = default!;
        string buttonStyle = default!;
        string menuStyle = default!;
        Dictionary<string, string> items = default!;
        var icon = new SampleIcons.Samples.Info();

        Action<MenuChangeEventArgs> onMenuChanged = _ => { };
        var cut = TestContext.RenderComponent<FluentMenuButton>(parameters => parameters
            .Add(p => p.Button, button)
            .Add(p => p.Menu, menu)
            .Add(p => p.Text, text)
            .Add(p => p.ButtonStyle, buttonStyle)
            .Add(p => p.MenuStyle, menuStyle)
            .Add(p => p.Items, items)
            .Add(p => p.OnMenuChanged, onMenuChanged)
            .Add(p => p.IconStart, icon)
        );
        //Act

        //Assert
        cut.Verify();
    }

    [Fact]
    public void FluentMenuButton_Throws_IfBothTextAndButtonContentAreSet()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            TestContext.RenderComponent<FluentMenuButton>(parameters => parameters
                .Add(p => p.Text, "Button Text")
                .Add<RenderFragment>(p => p.ButtonContent, builder =>
                {
                    builder.OpenComponent<FluentMenuItem>(0);
                    builder.AddAttribute(1, "Text", "Menu Item 1");
                    builder.CloseComponent();
                })
            );
        });
    }

    [Fact]
    public void FluentMenuButton_Renders_ButtonContent()
    {
        // Arrange
        var cut = TestContext.RenderComponent<FluentMenuButton>(parameters => parameters
            .Add<RenderFragment>(p => p.ButtonContent, builder =>
            {
                builder.OpenComponent<FluentLabel>(0);
                builder.AddAttribute(1, "Typo", Typography.H4);
                builder.AddAttribute(1, "ChildContent", (RenderFragment)(b =>
                {
                    b.AddContent(2, "Custom Button Content");
                }));
                builder.CloseComponent();
            })
        );

        // Assert
        cut.Verify();
    }
}

