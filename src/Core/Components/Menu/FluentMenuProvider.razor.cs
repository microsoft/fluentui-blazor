// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentMenuProvider : FluentComponentBase
{
    private IMenuService? _menuService = null;

    /// <summary />
    public FluentMenuProvider()
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    internal string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-menu-provider")
        .Build();

    /// <summary />
    internal string? StyleValue => new StyleBuilder(Style)
        .AddStyle("position", "fixed")   // To prevent the menu from displaying a scrollbar in body
        .AddStyle("z-index", ZIndex.Menu.ToString())
        .Build();

    /// <summary>
    /// Gets or sets the injected service provider.
    /// </summary>
    [Inject]
    public IServiceProvider? ServiceProvider { get; set; }

    /// <summary />
    protected virtual IMenuService? MenuService => _menuService ?? (_menuService = ServiceProvider?.GetService<IMenuService>());

    /// <summary />
    protected IEnumerable<FluentMenu>? Menus => MenuService?.Menus;

    /// <summary />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (MenuService != null)
        {
            MenuService.ProviderId = Id;
            MenuService.OnMenuUpdated = () => InvokeAsync(StateHasChanged);
        }
    }
}
