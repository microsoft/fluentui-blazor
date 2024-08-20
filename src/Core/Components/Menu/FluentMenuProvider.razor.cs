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
    internal string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-menu-provider")
        .Build();

    /// <summary />
    internal string? StyleValue => new StyleBuilder(Style)
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
            MenuService.OnMenuUpdated = () => InvokeAsync(StateHasChanged);
        }
    }
}
