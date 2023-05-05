using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentToasts2
{
    [Inject] private IToastService2 ToastService { get; set; } = default!;
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

    private List<Toast2> ToastList { get; set; } = new();
    private Queue<Toast2> ToastWaitingQueue { get; set; } = new();

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

    private ToastSettings BuildToastSettings(ToastIntent intent, Action<ToastSettings>? settings)
    {
        ToastSettings? toastInstanceSettings = new();
        settings?.Invoke(toastInstanceSettings);

        return intent switch
        {
            ToastIntent.Neutral => new ToastSettings(
                toastInstanceSettings.Icon,
                toastInstanceSettings.IconColor,
                toastInstanceSettings.IconVariant,
                ShowCloseButton,
                toastInstanceSettings.OnClick,
                toastInstanceSettings.Timeout == 0 ? Timeout : toastInstanceSettings.Timeout                ),
            ToastIntent.Info => new ToastSettings(
                FluentIcons.Info,
                Color.Info,
                IconVariant.Filled,
                ShowCloseButton,
                toastInstanceSettings.OnClick,
                toastInstanceSettings.Timeout == 0 ? Timeout : toastInstanceSettings.Timeout),
            ToastIntent.Success => new ToastSettings(
                FluentIcons.CheckmarkCircle,
                Color.Success,
                IconVariant.Filled,
                ShowCloseButton,
                toastInstanceSettings.OnClick,
                toastInstanceSettings.Timeout == 0 ? Timeout : toastInstanceSettings.Timeout),
            ToastIntent.Warning => new ToastSettings(
                FluentIcons.Warning,
                Color.Warning,
                IconVariant.Filled,
                ShowCloseButton,
                toastInstanceSettings.OnClick,
                toastInstanceSettings.Timeout == 0 ? Timeout : toastInstanceSettings.Timeout),
            ToastIntent.Error => new ToastSettings(
                FluentIcons.DismissCircle,
                Color.Error,
                IconVariant.Filled,
                ShowCloseButton,
                toastInstanceSettings.OnClick,
                toastInstanceSettings.Timeout == 0 ? Timeout : toastInstanceSettings.Timeout),
            _ => throw new InvalidOperationException()
        };
    }

    private void ShowToast(ToastIntent level, RenderFragment message, Action<ToastSettings>? toastSettings)
    {
        InvokeAsync(() =>
        {
            ToastSettings? settings = BuildToastSettings(level, toastSettings);
            Toast2? toast = new(message, level, settings);

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
            Toast2? toastInstance = new(childContent, toastSettings);

            ToastList.Add(toastInstance);

            StateHasChanged();
        });
    }

    private void ShowEnqueuedToast()
    {
        InvokeAsync(() =>
        {
            Toast2? toast = ToastWaitingQueue.Dequeue();

            ToastList.Add(toast);

            StateHasChanged();
        });
    }

    public void RemoveToast(string toastId)
    {
        InvokeAsync(() =>
        {
            Toast2? toastInstance = ToastList.SingleOrDefault(x => x.Id == toastId);

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
