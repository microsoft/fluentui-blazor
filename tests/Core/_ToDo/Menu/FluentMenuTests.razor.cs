// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Menu;
public partial class FluentMenuTests : TestContext
{
    public FluentMenuTests()
    {
        JSInterop.SetupModule("./_content/Microsoft.FluentUI.AspNetCore.Components/Components/Menu/FluentMenu.razor.js");
        Services.AddSingleton(LibraryConfiguration.ForUnitTests);
        Services.AddSingleton<IMenuService, MenuService>();
        JSInterop.SetupModule("./_content/Microsoft.FluentUI.AspNetCore.Components/Components/AnchoredRegion/FluentAnchoredRegion.razor.js");

        var menuModule = JSInterop.SetupModule("./_content/Microsoft.FluentUI.AspNetCore.Components/Components/Menu/FluentMenu.razor.js");
        menuModule.SetupVoid("initialize", _ => true);
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
        var cut = RenderComponent<FluentMenu>(parameters => parameters
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
        var menuProviderCut = RenderComponent<FluentMenuProvider>();
        var menuCut = RenderComponent<FluentMenu>(parameters => parameters
            .Add(p => p.UseMenuService, true)
            .Add(p => p.Class, className)
            .Add(p => p.Anchored, true)
            .Add(p => p.Id, "menu1")
            .Add(p => p.Anchor, "menuAnchor")

        );

        //Act
        menuProviderCut.Render();

        //Assert
        var menuInProvider = menuProviderCut.FindComponent<FluentMenu>();
        Assert.Equal(className, menuInProvider.Instance.Class, StringComparer.Ordinal);
    }

    [Fact]
    public async Task FluentMenu_DisposeAsync_UnregistersMenuAndNotifiesProvider()
    {
        // Arrange
        var menuService = Services.GetRequiredService<IMenuService>();
        var updateCount = 0;
        menuService.ProviderId = "menu-provider";
        menuService.OnMenuUpdated = () => updateCount++;
        var menu = new TestFluentMenu
        {
            ServiceProvider = Services,
            Anchor = "menu-anchor",
            Anchored = true
        };
        menu.Initialize();

        Assert.Same(menu, Assert.Single(menuService.Menus));

        // Act
        await menu.DisposeAsync();

        // Assert
        Assert.Empty(menuService.Menus);
        Assert.Equal(1, updateCount);
    }

    [Fact]
    public async Task FluentMenu_DisposeAsync_WhenAlreadyDisposed_DoesNotNotifyProviderAgain()
    {
        // Arrange
        var menuService = Services.GetRequiredService<IMenuService>();
        var updateCount = 0;
        menuService.ProviderId = "menu-provider";
        menuService.OnMenuUpdated = () => updateCount++;
        var menu = new TestFluentMenu
        {
            ServiceProvider = Services,
            Anchor = "menu-anchor",
            Anchored = true
        };
        menu.Initialize();
        await menu.DisposeAsync();

        // Act
        await menu.DisposeAsync();

        // Assert
        Assert.Empty(menuService.Menus);
        Assert.Equal(1, updateCount);
    }

    [Fact]
    public async Task FluentMenu_DisposeAsync_DoesNotUnregisterDifferentMenuWithSameId()
    {
        // Arrange
        var menuService = Services.GetRequiredService<IMenuService>();
        menuService.ProviderId = "menu-provider";
        var registeredMenu = new TestFluentMenu
        {
            ServiceProvider = Services,
            Id = "shared-menu-id",
            Anchor = "registered-menu-anchor",
            Anchored = true
        };
        var unregisteredMenu = new TestFluentMenu
        {
            ServiceProvider = Services,
            Id = "shared-menu-id",
            Anchor = "unregistered-menu-anchor",
            Anchored = true,
            UseMenuService = false
        };
        registeredMenu.Initialize();
        unregisteredMenu.Initialize();

        Assert.Same(registeredMenu, Assert.Single(menuService.Menus));

        // Act
        await unregisteredMenu.DisposeAsync();

        // Assert
        Assert.Same(registeredMenu, Assert.Single(menuService.Menus));
    }

    private sealed class TestFluentMenu : FluentMenu
    {
        public void Initialize()
        {
            base.OnInitialized();
        }
    }
}
