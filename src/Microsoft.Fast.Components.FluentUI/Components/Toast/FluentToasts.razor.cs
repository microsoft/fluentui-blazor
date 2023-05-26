using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;


namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentToasts
{
    [Inject]
    private ToastManager ToastManager { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

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

    private readonly InternalToastContext _internalToastContext;
    private readonly List<ToastInstance> _toastList;
    private Queue<ToastInstance> _toastWaitingQueue;

    private readonly RenderFragment _renderToasts;

    /// <summary>
    /// Constructs an instance of <see cref="FluentToasts"/>.
    /// </summary>
    public FluentToasts()
    {
        _internalToastContext = new(this);
        _toastList = new();
        _toastWaitingQueue = new();
        _renderToasts = RenderToasts;
    }


    protected override void OnInitialized()
    {
        ToastManager.OnShow += ShowToast;
        ToastManager.OnClearAll += ClearAll;
        ToastManager.OnClearIntent += ClearIntent;
        ToastManager.OnClearQueue += ClearQueue;
        ToastManager.OnClearQueueIntent += ClearQueueIntent;

        if (RemoveToastsOnNavigation)
        {
            NavigationManager.LocationChanged += ClearToasts;
        }

        _positionClass = $"position-{Position.ToString().ToLower()}";
    }

    private ToastSettings BuildToastSettings(ToastIntent intent, Action<ToastSettings>? settings)
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

        toastInstanceSettings.Timeout = toastInstanceSettings.Timeout == 0 ? Timeout : toastInstanceSettings.Timeout;


        return toastInstanceSettings;
    }

    private void ListOrQueue(ToastInstance toast)
    {
        if (_toastList.Count < MaxToastCount)
        {
            _toastList.Add(toast);

            StateHasChanged();
        }
        else
        {
            _toastWaitingQueue.Enqueue(toast);
        }
    }

    private void ShowToast(Type toastComponent, ToastParameters parameters, Action<ToastSettings>? settings)
    {
        _ = InvokeAsync(() =>
        {
            ToastIntent intent = parameters.Intent;
            ToastSettings? toastSettings = BuildToastSettings(intent, settings);

            ToastInstance toast = new(toastComponent, parameters, toastSettings);

            ListOrQueue(toast);
        });
    }

    private void ShowToastComponentToast(Type toastComponent, ToastIntent intent, string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null)
    {
        _ = InvokeAsync(() =>
        {
            ToastParameters parameters = new()
            {
                Intent = intent,
                Title = title,
                EndContentType = ToastEndContentType.Dismiss
            };

            if (action is not null)
            {
                ToastAction act = new();
                action.Invoke(act);

                parameters.Add("PrimaryAction", act);
                parameters.EndContentType = ToastEndContentType.Action;
            }

            ToastSettings? toastSettings = BuildToastSettings(intent, settings);

            ToastInstance toast = new(toastComponent, parameters, toastSettings);

            ListOrQueue(toast);
        });
    }

    private void ShowEnqueuedToasts()
    {
        _ = InvokeAsync(() =>
        {

            while (_toastList.Count < MaxToastCount && _toastWaitingQueue.Count > 0)
            {
                ToastInstance? toast = _toastWaitingQueue.Dequeue();

                _toastList.Add(toast);
            }

            StateHasChanged();
        });
    }

    public void RemoveToast(string toastId)
    {
        _ = InvokeAsync(() =>
        {
            ToastInstance? toastInstance = _toastList.SingleOrDefault(x => x.Id == toastId);

            if (toastInstance is not null)
            {
                _toastList.Remove(toastInstance);

                ShowEnqueuedToasts();
            }
        });
    }
    private void ClearAll(bool includeQueue)
    {
        _ = InvokeAsync(() =>
        {
            _toastList.Clear();
            if (includeQueue)
            {
                _toastWaitingQueue.Clear();
                StateHasChanged();
            }
            else
            {
                ShowEnqueuedToasts();
            }
        });
    }

    private void ClearToasts(object? sender, LocationChangedEventArgs args)
    {
        _ = InvokeAsync(() =>
        {
            _toastList.Clear();
            _toastWaitingQueue.Clear();

            StateHasChanged();
        });
    }

    private void ClearIntent(ToastIntent intent, bool includeQueue)
    {
        //_ = InvokeAsync(() =>
        //{

        //});

        _toastList.RemoveAll(x => x.Intent == intent);
        if (includeQueue)
        {
            _toastWaitingQueue = new(_toastWaitingQueue.Where(x => x.Intent != intent));
        }

        ShowEnqueuedToasts();
    }

    private void ClearQueue()
    {
        _ = InvokeAsync(() =>
        {
            _toastWaitingQueue.Clear();
            StateHasChanged();
        });
    }

    private void ClearQueueIntent(ToastIntent intent)
    {
        _ = InvokeAsync(() =>
        {
            _toastWaitingQueue = new(_toastWaitingQueue.Where(x => x.Intent != intent));
            StateHasChanged();
        });
    }
}
