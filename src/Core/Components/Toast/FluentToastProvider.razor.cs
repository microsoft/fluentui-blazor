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
    private readonly LibraryConfiguration configuration;

    /// <summary />
    public FluentToastProvider(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
        this.configuration = configuration;
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
                SynchronizeToastQueue();
                await InvokeAsync(StateHasChanged);
            };

            SynchronizeToastQueue();
        }
    }

    /// <summary />
    internal static Action<ToastEventArgs> EmptyOnStatusChange => (_) => { };

    private EventCallback<ToastEventArgs> GetOnStatusChangeCallback(IToastInstance toast)
        => EventCallback.Factory.Create<ToastEventArgs>(this, toast.Options.OnStatusChange ?? EmptyOnStatusChange);

    private int GetTimeout(IToastInstance toast)
        => toast.Options.Timeout ?? configuration.Toast.Timeout;

    private ToastPosition? GetPosition(IToastInstance toast)
        => toast.Options.Position ?? configuration.Toast.Position;

    private int GetVerticalOffset(IToastInstance toast)
        => toast.Options.VerticalOffset ?? configuration.Toast.VerticalOffset;

    private int GetHorizontalOffset(IToastInstance toast)
        => toast.Options.HorizontalOffset ?? configuration.Toast.HorizontalOffset;

    private bool GetPauseOnHover(IToastInstance toast)
        => toast.Options.PauseOnHover ?? configuration.Toast.PauseOnHover;

    private bool GetPauseOnWindowBlur(IToastInstance toast)
        => toast.Options.PauseOnWindowBlur ?? configuration.Toast.PauseOnWindowBlur;

    private IEnumerable<IToastInstance> GetRenderedToasts()
        => ToastService?.Items.Values
            .Where(toast => toast.LifecycleStatus is ToastLifecycleStatus.Visible or ToastLifecycleStatus.Dismissed)
            .OrderByDescending(toast => toast.Index)
            ?? Enumerable.Empty<IToastInstance>();

    private void SynchronizeToastQueue()
    {
        if (ToastService is null)
        {
            return;
        }

        var maxToastCount = configuration.Toast.MaxToastCount;
        var activeCount = ToastService.Items.Values.Count(toast => toast.LifecycleStatus is ToastLifecycleStatus.Visible or ToastLifecycleStatus.Dismissed);
        var queuedToasts = ToastService.Items.Values
            .Where(toast => toast.LifecycleStatus == ToastLifecycleStatus.Queued)
            .OrderByDescending(toast => toast.Index)
            .ToList();

        foreach (var toast in queuedToasts)
        {
            if (activeCount >= maxToastCount)
            {
                break;
            }

            if (toast is ToastInstance instance)
            {
                instance.LifecycleStatus = ToastLifecycleStatus.Visible;
                toast.Options.OnStatusChange?.Invoke(new ToastEventArgs(instance, ToastLifecycleStatus.Visible));
                activeCount++;
            }
        }
    }
}
