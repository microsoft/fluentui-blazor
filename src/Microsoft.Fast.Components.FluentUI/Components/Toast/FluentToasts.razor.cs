using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;


namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentToasts 
{
    [Inject] 
    private IToastService ToastService { get; set; } = default!;
    
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
        ToastService.OnShow += ShowToast;
        ToastService.OnShowCustomComponent += ShowCustomToast;
        ToastService.OnShowToastComponent += ShowToastComponentToast;
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
        ToastSettings? toastInstanceSettings = new();
        settings?.Invoke(toastInstanceSettings);

        return toastInstanceSettings;
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

        //toastInstanceSettings.EndContentType ??= ToastEndContentType.Dismiss;

        return toastInstanceSettings;
    }

    private ToastSettings BuildToastSettings(ToastIntent intent, Action<ToastAction>? action, Action<ToastSettings>? settings)
    {
        ToastSettings? toastInstanceSettings = BuildToastSettings(intent, settings);

        ToastAction a = new();
        action?.Invoke(a);
        //toastInstanceSettings.PrimaryAction = a;

        return toastInstanceSettings;
    }

    private ToastParameters BuildToastParameters(ToastIntent intent, Action<ToastAction>? action, Action<ToastSettings>? settings)
    {
        ToastParameters parameters = new();

        ToastSettings? toastInstanceSettings = BuildToastSettings(intent, settings);


        ToastAction a = new();
        action?.Invoke(a);

        parameters.Add("PrimaryAction", a);

        //toastInstanceSettings.PrimaryAction = a;

        return parameters;
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

    private void ShowToast(ToastIntent intent, string title, Action<ToastSettings>? toastSettings)
    {

        _ = InvokeAsync(() =>
        {
            ToastSettings? settings = BuildToastSettings(intent, toastSettings);
            ToastInstance toast = new(intent, title, settings);

            ListOrQueue(toast);
        });
    }

    private void ShowCustomToast(Type contentComponent, ToastParameters? parameters, Action<ToastSettings>? settings)
    {
        _ = InvokeAsync(() =>
        {
            RenderFragment? childContent = new(builder =>
            {
                builder.OpenComponent(0, contentComponent);
                if (parameters is not null)
                {
                    builder.AddMultipleAttributes(1, parameters.Parameters);
                }

                builder.CloseComponent();
            });

            ToastSettings? toastSettings = BuildCustomToastSettings(settings);
            ToastInstance toast = new(childContent, toastSettings);

            ListOrQueue(toast);
        });
    }

    private void ShowToastComponentToast(Type toastComponent, ToastIntent intent, string title, Action<ToastSettings>? settings)
    {
        _ = InvokeAsync(() =>
        {
            ToastParameters parameters = new();

            parameters.Add("Intent", intent);
            parameters.Add("Title", title);

            RenderFragment? customContent = new(builder =>
            {

                builder.OpenComponent(0, toastComponent);
                builder.AddMultipleAttributes(1, parameters.Parameters);
                builder.CloseComponent();
            });

            ToastSettings? toastSettings = BuildToastSettings(intent, settings);
            ToastInstance toast = new(intent, customContent, toastSettings);
            ListOrQueue(toast);
        });
    }

    private void ShowToastComponentToast(Type toastComponent, ToastIntent intent, string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null)
    {
        _ = InvokeAsync(() =>
        {
            ToastParameters parameters = new();

            parameters.Add("Intent", intent);
            parameters.Add("Title", title);
            
            if (action is not null)
            {
                ToastAction act = new();
                action.Invoke(act);

                parameters.Add("PrimaryAction", act);
                parameters.Add("EndContentType", ToastEndContentType.Action);
            }

            RenderFragment? customContent = new(builder =>
            {

                builder.OpenComponent(0, toastComponent);
                builder.AddMultipleAttributes(1, parameters.Parameters);
                builder.CloseComponent();
            });

            ToastSettings? toastSettings = BuildToastSettings(intent, settings);

            ToastInstance toast = new(intent, customContent, toastSettings);
            
            ListOrQueue(toast);
        });
    }

    private void ShowEnqueuedToasts()
    {
        _ = InvokeAsync(() =>
        {
            bool shouldRerender = false;
            while (_toastList.Count < MaxToastCount && _toastWaitingQueue.Count > 0)
            {
                ToastInstance? toast = _toastWaitingQueue.Dequeue();

                _toastList.Add(toast);
                shouldRerender = true;
            }

            if (shouldRerender)
            {
                StateHasChanged();
            }
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

    private void ClearToasts(object? sender, LocationChangedEventArgs args)
    {
        _ = InvokeAsync(() =>
        {
            _toastList.Clear();
            
            ShowEnqueuedToasts();
            
        });
    }

    private void ClearAll()
    {
        _ = InvokeAsync(() =>
        {
            _toastList.Clear();
            
            ShowEnqueuedToasts();
        });
    }

    private void ClearToasts(ToastIntent intent)
    {
        _ = InvokeAsync(() =>
        {
            _toastList.RemoveAll(x => x.Intent == intent);
            
            ShowEnqueuedToasts();
        });
    }

    private void ClearCustomToasts()
    {
        _ = InvokeAsync(() =>
        {
            _toastList.RemoveAll(x => x.CustomContent is not null);
            
            ShowEnqueuedToasts();
        });
    }

    private void ClearQueue()
    {
        _ = InvokeAsync(() =>
        {
            _toastWaitingQueue.Clear();
            StateHasChanged();
        });
    }

    private void ClearQueueToasts(ToastIntent toastLevel)
    {
        _ = InvokeAsync(() =>
        {
            _toastWaitingQueue = new(_toastWaitingQueue.Where(x => x.Intent != toastLevel));
            StateHasChanged();
        });
    }
}
