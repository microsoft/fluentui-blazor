using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentToastProvider
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
    /// Gets or sets the number of milliseconds a toast remains visible. Default is 7000 (7 seconds).
    /// </summary>
    [Parameter]
    public int Timeout { get; set; } = 7000;

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
    /// Constructs an instance of <see cref="FluentToastProvider"/>.
    /// </summary>
    public FluentToastProvider()
    {
        _internalToastContext = new(this);
        _toastList = [];
        _toastWaitingQueue = new();
        _renderToasts = RenderToasts;
    }

    protected override void OnInitialized()
    {
        ToastService.OnShow += ShowToast;
        ToastService.OnUpdate += UpdateToast;
        ToastService.OnClose += CloseToast;
        ToastService.OnClearAll += ClearAll;
        ToastService.OnClearIntent += ClearIntent;
        ToastService.OnClearQueue += ClearQueue;
        ToastService.OnClearQueueIntent += ClearQueueIntent;

        if (RemoveToastsOnNavigation)
        {
            NavigationManager.LocationChanged += ClearToasts;
        }

        _positionClass = $"position-{Position.ToString().ToLower()}";
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

    private void ShowToast(Type? toastComponent, ToastParameters parameters, object content)
    {
        _ = InvokeAsync(() =>
        {
            ToastInstance toast = new(toastComponent, parameters, content);

            ListOrQueue(toast);
        });
    }

    private void UpdateToast(string? toastId, ToastParameters parameters)
    {
        ToastInstance? toastInstance = _toastList.SingleOrDefault(x => x.Id == toastId);

        if (toastInstance is not null)
        {
            toastInstance.Parameters = parameters;
            InvokeAsync(StateHasChanged);
        };

    }

    private void CloseToast(string id)
        => RemoveToast(id);

    private void ShowEnqueuedToasts()
    {
        _ = InvokeAsync(() =>
        {

            while (_toastList.Count < MaxToastCount && _toastWaitingQueue.Count > 0)
            {
                ToastInstance toast = _toastWaitingQueue.Dequeue();

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

        _toastList.RemoveAll(x => x.Parameters.Intent == intent);
        if (includeQueue)
        {
            _toastWaitingQueue = new(_toastWaitingQueue.Where(x => x.Parameters.Intent != intent));
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
            _toastWaitingQueue = new(_toastWaitingQueue.Where(x => x.Parameters.Intent != intent));
            StateHasChanged();
        });
    }
}
