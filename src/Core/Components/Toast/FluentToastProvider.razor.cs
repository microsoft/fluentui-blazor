// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
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
    private RenderFragment? RenderToastContent(IToastInstance? toast)
    {
        if (toast is null)
        {
            return null;
        }

        var hasMessage = !string.IsNullOrEmpty(toast.Options.Message);
        var hasComponent = toast.ComponentType is not null;

        if (!hasMessage && !hasComponent)
        {
            return null;
        }

        return builder =>
        {
            if (hasMessage)
            {
                builder.AddContent(0, new MarkupStringSanitized(toast.Options.Message!, MarkupStringSanitized.Formats.Html, LibraryConfiguration));
            }

            if (hasComponent)
            {
                builder.OpenComponent(1, toast.ComponentType!);
                if (toast.Options.Parameters is not null)
                {
                    foreach (var parameter in toast.Options.Parameters)
                    {
                        builder.AddAttribute(2, parameter.Key, parameter.Value);
                    }
                }

                builder.CloseComponent();
            }
        };
    }

    private TimeSpan GetLifetime(IToastInstance toast)
    {
        // If the toast has a specific lifetime defined, use it.
        if (toast.Options.Lifetime.HasValue)
        {
            return toast.Options.Lifetime.Value;
        }

        // Keep the toast open until the user interacts with it or dismisses it.
        var hasPrimaryAction = !string.IsNullOrEmpty(toast.Options.QuickAction1.Label);
        var hasSecondaryAction = !string.IsNullOrEmpty(toast.Options.QuickAction2.Label);
        if (hasPrimaryAction || hasSecondaryAction)
        {
            return TimeSpan.Zero;
        }

        // Otherwise, use the default lifetime from the configuration, or TimeSpan.Zero if not defined.
        return configuration.Toast.Lifetime ?? TimeSpan.Zero;
    }

    /// <summary>
    /// Renders the footer content of the toast, including the primary and secondary quick actions if they are defined in the toast options.
    /// </summary>
    private RenderFragment? RenderFooterContent(IToastInstance toast)
    {
        var hasPrimaryAction = !string.IsNullOrEmpty(toast.Options.QuickAction1.Label);
        var hasSecondaryAction = !string.IsNullOrEmpty(toast.Options.QuickAction2.Label);

        if (!hasPrimaryAction && !hasSecondaryAction)
        {
            return null;
        }

        var inverted = toast.Options.Inverted ?? configuration.Toast.Inverted;
        var actions = new List<ToastOptionsAction> { toast.Options.QuickAction1, toast.Options.QuickAction2 };

        return builder =>
        {
            builder.OpenComponent<FluentStack>(0);
            builder.AddComponentParameter(1, nameof(FluentStack.HorizontalGap), "12px");
            builder.AddComponentParameter(2, nameof(FluentStack.ChildContent), (RenderFragment)(stackBuilder =>
            {
                foreach (var action in actions)
                {
                    if (string.IsNullOrEmpty(action.Label))
                    {
                        continue;
                    }

                    stackBuilder.OpenComponent<FluentLink>(0);
                    stackBuilder.AddComponentParameter(1, nameof(FluentLink.OnClick), EventCallback.Factory.Create<MouseEventArgs>(this, async () =>
                    {
                        if (action.OnClickAsync is not null)
                        {
                            await action.OnClickAsync.Invoke(new ToastEventArgs(toast, ToastLifecycleStatus.Visible));
                        }
                    }));
                    stackBuilder.AddComponentParameter(2, nameof(FluentLink.Tooltip), action.Tooltip);
                    stackBuilder.AddComponentParameter(3, nameof(FluentLink.Style), inverted ? "color: var(--colorBrandForegroundInverted);" : null);
                    stackBuilder.AddComponentParameter(4, nameof(FluentLink.ChildContent), (RenderFragment)(contentBuilder => contentBuilder.AddContent(0, action.Label)));
                    stackBuilder.CloseComponent();
                }
            }));
            builder.CloseComponent();
        };
    }

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
