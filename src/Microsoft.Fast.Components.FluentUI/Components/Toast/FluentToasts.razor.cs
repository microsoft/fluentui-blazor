using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Fast.Components.FluentUI.DataGrid.Infrastructure;

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

    private readonly List<FluentToast> _toastList;
    private Queue<FluentToast> _toastWaitingQueue;

    /// <summary>
    /// Constructs an instance of <see cref="FluentToasts"/>.
    /// </summary>
    public FluentToasts()
    {
        _toastList = new();
        _toastWaitingQueue = new();
    }


    protected override void OnInitialized()
    {
        ToastService.OnShow += ShowToast;
        ToastService.OnShowCustomComponent += ShowCustomToast;
        ToastService.OnShowToastComponent += ShowToastComponentToast;
        ToastService.OnShowToastComponentWithAction += ShowToastComponentToastWithAction;
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

        return toastInstanceSettings;
    }

    private ToastSettings BuildToastSettings(ToastIntent intent, Action<ToastAction>? action, Action<ToastSettings>? settings)
    {
        ToastSettings? toastInstanceSettings = BuildToastSettings(intent, settings);

        ToastAction a = new();
        action?.Invoke(a);
        toastInstanceSettings.PrimaryAction = a;

        return toastInstanceSettings;
    }

    private void ShowToast(ToastIntent intent, string title, Action<ToastSettings>? toastSettings)
    {
        
        _ = InvokeAsync(() =>
        {
            ToastSettings? settings = BuildToastSettings(intent, toastSettings);
            FluentToast? toast = new(intent, title, settings);

            if (_toastList.Count < MaxToastCount)
            {
                _toastList.Add(toast);

                StateHasChanged();
            }
            else
            {
                _toastWaitingQueue.Enqueue(toast);
            }
        });
    }

    private void ShowCustomToast(Type contentComponent, ToastParameters? parameters, Action<ToastSettings>? settings)
    {
        _ = InvokeAsync(() =>
        {
            RenderFragment? childContent = new(builder =>
            {
                builder.OpenComponent(0, contentComponent);
                if (parameters is not null )
                {
                    builder.AddMultipleAttributes(1, parameters.Parameters);
                }

                builder.CloseComponent();
            });

            ToastSettings? toastSettings = BuildCustomToastSettings(settings);
            FluentToast? toastInstance = new(childContent, toastSettings);

            _toastList.Add(toastInstance);

            StateHasChanged();
        });
    }

    private void ShowToastComponentToast(Type toastComponent, ToastIntent intent, string title, Action<ToastSettings>? settings)
    {
        _ = InvokeAsync(() =>
        {
            RenderFragment? customContent = new(builder =>
            {

                builder.OpenComponent(0, toastComponent);
                builder.AddAttribute(1, "Intent", intent);
                builder.AddAttribute(2, "Title", title);

                builder.CloseComponent();
            });

            ToastSettings? toastSettings = BuildToastSettings(intent, settings);
            FluentToast? toastInstance = new(customContent, toastSettings);

            _toastList.Add(toastInstance);

            StateHasChanged();
        });
    }

    private void ShowToastComponentToastWithAction(Type toastComponent, ToastIntent intent, string title, Action<ToastAction>? action,  Action<ToastSettings>? settings)
    {
        ToastSettings? toastSettings = BuildToastSettings(intent, action, settings);

        _ = InvokeAsync(() =>
        {
            RenderFragment? customContent = new(builder =>
            {

                builder.OpenComponent(0, toastComponent);
                builder.AddAttribute(1, "Intent", intent);
                builder.AddAttribute(2, "Title", title);

                builder.CloseComponent();
            });

            FluentToast? toastInstance = new(customContent, toastSettings);

            _toastList.Add(toastInstance);

            StateHasChanged();
        });
    }


    private void ShowEnqueuedToast()
    {
        _ = InvokeAsync(() =>
        {
            FluentToast? toast = _toastWaitingQueue.Dequeue();

            _toastList.Add(toast);

            StateHasChanged();
        });
    }

    public void RemoveToast(string toastId)
    {
        _ = InvokeAsync(() =>
        {
            FluentToast? toastInstance = _toastList.SingleOrDefault(x => x.Id == toastId);

            if (toastInstance is not null)
            {
                _toastList.Remove(toastInstance);
                StateHasChanged();
            }

            if (_toastWaitingQueue.Count > 0)
            {
                ShowEnqueuedToast();
            }
        });
    }

    private void ClearToasts(object? sender, LocationChangedEventArgs args)
    {
        _ = InvokeAsync(() =>
        {
            _toastList.Clear();
            StateHasChanged();

            if (_toastWaitingQueue.Count > 0)
            {
                ShowEnqueuedToast();
            }
        });
    }

    private void ClearAll()
    {
        _ = InvokeAsync(() =>
        {
            _toastList.Clear();
            StateHasChanged();
        });
    }

    private void ClearToasts(ToastIntent toastLevel)
    {
        _ = InvokeAsync(() =>
        {
            _toastList.RemoveAll(x => x.ChildContent is null && x.Intent == toastLevel);
            StateHasChanged();
        });
    }

    private void ClearCustomToasts()
    {
        _ = InvokeAsync(() =>
        {
            _toastList.RemoveAll(x => x.ChildContent is not null);
            StateHasChanged();
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
