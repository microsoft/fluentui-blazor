using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentToasts
{
    [Inject] private IToastService ToastService { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    /// <summary>
    /// Gets or sets the position on screen where the toasts are shown. See <see cref="ToastPosition"/>
    /// Default is ToastPosition.TopRight
    /// </summary>
    [Parameter]
    public ToastPosition Position { get; set; } = ToastPosition.TopRight;

    /// <summary>
    /// Gets or sets the number of seconds a toast remains visible. Default is 7 seconds.
    /// </summary>
    [Parameter]
    public int Timeout { get; set; } = 7;

    /// <summary>
    /// Gets or sets the maximum number of toasts that can be shown at once. Default is 4.
    /// </summary>
    [Parameter]
    public int MaxToastCount { get; set; } = 4;

    /// <summary>
    /// Gets or sets whether to remove toasts when the user navigates to a new page. Default is true.
    /// </summary>
    [Parameter]
    public bool RemoveToastsOnNavigation { get; set; } = true;

    /// <summary>
    /// Gets or sets whether to show a close button on a toast. Default is true.
    /// </summary>
    [Parameter]
    public bool ShowCloseButton { get; set; } = true;

    private string _positionClass = string.Empty;

    private List<Toast> ToastList { get; set; } = new();
    private Queue<Toast> ToastWaitingQueue { get; set; } = new();

    protected override void OnInitialized()
    {
        ToastService.OnShow += ShowToast;
        ToastService.OnShowComponent += ShowCustomToast;
        ToastService.OnClearAll += ClearAll;
        ToastService.OnClearToasts += ClearToasts;
        ToastService.OnClearCustomToasts += ClearCustomToasts;
        ToastService.OnClearQueue += ClearQueue;
        ToastService.OnClearQueueToasts += ClearQueueToasts;

        if (RemoveToastsOnNavigation)
        {
            NavigationManager.LocationChanged += ClearToasts;
        }

        _positionClass = $"position-{Position.ToString().ToLower()}";
    }

    private static ToastSettings BuildCustomToastSettings(Action<ToastSettings>? settings)
    {
        ToastSettings? instanceToastSettings = new();
        settings?.Invoke(instanceToastSettings);

        return instanceToastSettings;
    }

    private static ToastSettings BuildToastSettings(ToastIntent intent, Action<ToastSettings>? settings)
    {
        ToastSettings? toastInstanceSettings = new();
        settings?.Invoke(toastInstanceSettings);

        toastInstanceSettings.Icon = intent switch
        {
            ToastIntent.Success => (FluentIcons.CheckmarkCircle, Color.Success, IconVariant.Filled),
            ToastIntent.Warning => (FluentIcons.Warning, Color.Warning, IconVariant.Filled),
            ToastIntent.Error => (FluentIcons.DismissCircle, Color.Error, IconVariant.Filled),
            ToastIntent.Info => (FluentIcons.Info, Color.Info, IconVariant.Filled),
            ToastIntent.Progress => (FluentIcons.Flash, Color.Neutral, IconVariant.Regular),
            ToastIntent.Upload => (FluentIcons.ArrowUpload, Color.Neutral, IconVariant.Regular),
            ToastIntent.Download => (FluentIcons.ArrowDownload, Color.Neutral, IconVariant.Regular),
            ToastIntent.Event => (FluentIcons.CalendarLTR, Color.Neutral, IconVariant.Regular),
            ToastIntent.Avatar => (FluentIcons.Person, Color.Neutral, IconVariant.Regular),
            ToastIntent.Custom => toastInstanceSettings.Icon,
            _ => throw new InvalidOperationException()
        };

        return toastInstanceSettings;
    }

    private void ShowToast(ToastIntent level, RenderFragment message, Action<ToastSettings>? toastSettings)
    {
        InvokeAsync(() =>
        {
            ToastSettings? settings = BuildToastSettings(level, toastSettings);
            Toast? toast = new(message, level, settings);

            if (ToastList.Count < MaxToastCount)
            {
                ToastList.Add(toast);

                StateHasChanged();
            }
            else
            {
                ToastWaitingQueue.Enqueue(toast);
            }
        });
    }

    private void ShowCustomToast(Type contentComponent, ToastParameters? parameters, Action<ToastSettings>? settings)
    {
        InvokeAsync(() =>
        {
            RenderFragment? childContent = new(builder =>
            {
                var i = 0;
                builder.OpenComponent(i++, contentComponent);
                if (parameters is not null)
                {
                    foreach (KeyValuePair<string, object> parameter in parameters.Parameters)
                    {
                        builder.AddAttribute(i++, parameter.Key, parameter.Value);
                    }
                }

                builder.CloseComponent();
            });

            ToastSettings? toastSettings = BuildCustomToastSettings(settings);
            Toast? toastInstance = new(childContent, toastSettings);

            ToastList.Add(toastInstance);

            StateHasChanged();
        });
    }

    private void ShowEnqueuedToast()
    {
        InvokeAsync(() =>
        {
            Toast? toast = ToastWaitingQueue.Dequeue();

            ToastList.Add(toast);

            StateHasChanged();
        });
    }

    public void RemoveToast(string toastId)
    {
        InvokeAsync(() =>
        {
            Toast? toastInstance = ToastList.SingleOrDefault(x => x.Id == toastId);

            if (toastInstance is not null)
            {
                ToastList.Remove(toastInstance);
                StateHasChanged();
            }

            if (ToastWaitingQueue.Any())
            {
                ShowEnqueuedToast();
            }
        });
    }

    private void ClearToasts(object? sender, LocationChangedEventArgs args)
    {
        InvokeAsync(() =>
        {
            ToastList.Clear();
            StateHasChanged();

            if (ToastWaitingQueue.Any())
            {
                ShowEnqueuedToast();
            }
        });
    }

    private void ClearAll()
    {
        InvokeAsync(() =>
        {
            ToastList.Clear();
            StateHasChanged();
        });
    }

    private void ClearToasts(ToastIntent toastLevel)
    {
        InvokeAsync(() =>
        {
            ToastList.RemoveAll(x => x.CustomComponent is null && x.Intent == toastLevel);
            StateHasChanged();
        });
    }

    private void ClearCustomToasts()
    {
        InvokeAsync(() =>
        {
            ToastList.RemoveAll(x => x.CustomComponent is not null);
            StateHasChanged();
        });
    }

    private void ClearQueue()
    {
        InvokeAsync(() =>
        {
            ToastWaitingQueue.Clear();
            StateHasChanged();
        });
    }

    private void ClearQueueToasts(ToastIntent toastLevel)
    {
        InvokeAsync(() =>
        {
            ToastWaitingQueue = new(ToastWaitingQueue.Where(x => x.Intent != toastLevel));
            StateHasChanged();
        });
    }
}
