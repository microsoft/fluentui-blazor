using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Menu;
public class FluentMenuTests : TestBase
{
    public FluentMenuTests()
    {
        TestContext.JSInterop.SetupModule("./_content/Microsoft.FluentUI.AspNetCore.Components/Components/Menu/FluentMenu.razor.js");
        TestContext.Services.AddSingleton(LibraryConfiguration.ForUnitTests);
        TestContext.Services.AddSingleton<IMenuService, MenuService>();
    }

    [Fact]
    public void FluentMenu_Default()
    {
        //Arrange
        var childContent = "<b>render me</b>";
        string anchor = default!;
        MouseButton trigger = default!;
        bool open = default!;
        HorizontalPosition horizontalPosition = default!;
        string width = default!;
        Action<bool> openChanged = _ => { };
        bool anchored = default!;
        var cut = TestContext.RenderComponent<FluentMenu>(parameters => parameters
            .Add(p => p.Anchor, anchor)
            .Add(p => p.Trigger, trigger)
            .Add(p => p.Open, open)
            .AddChildContent(childContent)
            .Add(p => p.HorizontalPosition, horizontalPosition)
            .Add(p => p.Width, width)
            .Add(p => p.OpenChanged, openChanged)
            .Add(p => p.Anchored, anchored)
        );
        //Act

        //Assert
        cut.Verify();
    }

    [Fact]
    public void FluentMenuProvider_ShouldUseFluentMenuClass()
    {
        //Arrange
        var className = "some-class";
        var menuProviderCut = TestContext.RenderComponent<FluentMenuProvider>();
        var menuCut = TestContext.RenderComponent<FluentMenu>(parameters => parameters
            .Add(p => p.UseMenuService, true)
            .Add(p => p.Class, className)
            .Add(p => p.Anchored, true)
            .Add(p => p.Id, "menu1")
            .Add(p => p.Anchor, "menuAnchor")
            
        );
        menuProviderCut.Render();
        //Act

        //Assert
        var menuInProvider = menuProviderCut.FindComponent<FluentMenu>();
        Assert.Equal(className, menuInProvider.Instance.Class, StringComparer.Ordinal);
    }
}

