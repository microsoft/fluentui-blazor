// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentToastProvider : FluentComponentBase, IDisposable
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

    /// <summary />
    protected virtual INotificationService? NotificationService => GetCachedServiceOrNull<INotificationService>();

    /// <summary />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (NotificationService is NotificationService service)
        {
            // Register this provider as a subscriber. Multiple providers can coexist:
            service.Subscribe(Id, async _ =>
            {
                SynchronizeToastQueue();
                await InvokeAsync(StateHasChanged);
            });

            SynchronizeToastQueue();
        }
    }

    /// <summary />
    public void Dispose()
    {
        if (NotificationService is NotificationService service && !string.IsNullOrEmpty(Id))
        {
            service.Unsubscribe(Id);
        }
    }

    /// <summary />
    private IEnumerable<IToastInstance> ToastItems
        => NotificationService?.Items.Values
                               .Where(item => item is IToastInstance)
                               .Cast<IToastInstance>()
        ?? [];

    /// <summary />
    private IEnumerable<IToastInstance> GetRenderedToasts()
        => ToastItems.Where(toast => toast.LifecycleStatus is ToastLifecycleStatus.Visible or ToastLifecycleStatus.Dismissed)
                     .OrderBy(toast => toast.Index);

    private EventCallback<ToastEventArgs> GetOnStatusChangeCallback(IToastInstance toast)
        => EventCallback.Factory.Create<ToastEventArgs>(this, toast.Options.OnStatusChange ?? ((_) => { }));

    /// <summary />
    private RenderFragment RenderToastContent(IToastInstance? toast) => builder =>
    {
        if (toast is null || string.IsNullOrEmpty(toast.Options.Message))
        {
            return;
        }

        builder.AddContent(0, new MarkupStringSanitized(toast.Options.Message, MarkupStringSanitized.Formats.Html, LibraryConfiguration));
    };

    /// <summary>
    /// Synchronizes the toast queue by promoting queued toasts to visible status based on the maximum allowed toast count.
    /// </summary>
    private void SynchronizeToastQueue()
    {
        var maxToastCount = configuration.Toast.MaxToastCount;
        var activeCount = ToastItems.Count(toast => toast.LifecycleStatus is ToastLifecycleStatus.Visible or ToastLifecycleStatus.Dismissed);
        var queuedToasts = ToastItems.Where(toast => toast.LifecycleStatus == ToastLifecycleStatus.Queued)
                                     .OrderBy(toast => toast.Index)
                                     .ToList();

        foreach (var toast in queuedToasts)
        {
            if (activeCount >= maxToastCount)
            {
                break;
            }

            if (toast is ToastInstance instance)
            {
                instance.SetStatus(ToastLifecycleStatus.Visible);
                activeCount++;
            }
        }
    }
}
