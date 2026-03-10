// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentToastProvider : FluentComponentBase
{
    /// <summary />
    public FluentToastProvider(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    internal string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-toast-provider")
        .Build();

    /// <summary />
    internal string? StyleValue => DefaultStyleBuilder
        .AddStyle("z-index", ZIndex.Toast.ToString(CultureInfo.InvariantCulture))
        .Build();

    /// <summary>
    /// Gets or sets the injected service provider.
    /// </summary>
    [Inject]
    public IServiceProvider? ServiceProvider { get; set; }

    /// <summary />
    protected virtual IToastService? ToastService => GetCachedServiceOrNull<IToastService>();

    /// <summary />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (ToastService is not null)
        {
            ToastService.ProviderId = Id;
            ToastService.OnUpdatedAsync = async (item) =>
            {
                await InvokeAsync(StateHasChanged);
            };
        }
    }

    /// <summary />
    private static Action<ToastEventArgs> EmptyOnStateChange => (_) => { };

    /// <summary>
    /// Only for Unit Tests
    /// </summary>
    /// <param name="id"></param>
    internal void UpdateId(string? id)
    {
        Id = id;

        if (ToastService is not null)
        {
            ToastService.ProviderId = id;
        }
    }
}
