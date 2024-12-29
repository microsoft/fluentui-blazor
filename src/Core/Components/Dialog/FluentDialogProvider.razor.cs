// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentDialogProvider : FluentComponentBase
{
    private IDialogService? _dialogService;

    /// <summary />
    public FluentDialogProvider()
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    internal string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-dialog-provider")
        .Build();

    /// <summary />
    internal string? StyleValue => new StyleBuilder(Style)
        .AddStyle("z-index", ZIndex.Dialog.ToString(CultureInfo.InvariantCulture))
        .Build();

    /// <summary>
    /// Gets or sets the injected service provider.
    /// </summary>
    [Inject]
    public IServiceProvider? ServiceProvider { get; set; }

    /// <summary />
    protected virtual IDialogService? DialogService => _dialogService ??= ServiceProvider?.GetService<IDialogService>();

    /// <summary />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (DialogService is not null)
        {
            DialogService.ProviderId = Id;
            DialogService.OnUpdatedAsync = async (item) =>
            {
                await InvokeAsync(StateHasChanged);
            };
        }
    }

    /// <summary />
    private static Action<DialogEventArgs> EmptyOnStateChange => (_) => { };

    /// <summary>
    /// Only for Unit Tests
    /// </summary>
    /// <param name="id"></param>
    internal void UpdateId(string? id)
    {
        Id = id;

        if (DialogService is not null)
        {
            DialogService.ProviderId = id;
        }
    }
}
