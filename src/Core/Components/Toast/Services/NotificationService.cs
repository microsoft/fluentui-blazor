// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Service for showing Toasts.
/// </summary>
public partial class NotificationService : FluentServiceBase<INotificationInstance>, INotificationService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IJSRuntime _jsRuntime;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationService"/> class.
    /// </summary>
    /// <param name="serviceProvider">List of services available in the application.</param>
    /// <param name="localizer">Localizer for the application.</param>
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(MessageBarEventArgs))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(MessageBarInstance))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(INotificationInstance))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(IMessageBarInstance))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(IToastInstance))]
    public NotificationService(IServiceProvider serviceProvider, IFluentLocalizer? localizer)
    {
        _serviceProvider = serviceProvider;
        _jsRuntime = serviceProvider.GetRequiredService<IJSRuntime>();
        Localizer = localizer ?? FluentLocalizerInternal.Default;
        ServiceProvider.OnUpdatedAsync = DispatchOnUpdatedAsync;
    }

    /// <summary />
    protected IFluentLocalizer Localizer { get; }

    /// <inheritdoc cref="INotificationService.ShowSuccessToastAsync(string?, string?, int?, string?, Func{ToastEventArgs, Task}?)"/>
    public Task<ToastResult> ShowSuccessToastAsync(string title, string? message = null, int? lifetime = 5, string? dismissLabel = null, Func<ToastEventArgs, Task>? dismissOnClickAsync = null)
        => ShowSimpleToastAsync(ToastIntent.Success, title, message, lifetime, dismissLabel, dismissOnClickAsync);

    /// <inheritdoc cref="INotificationService.ShowWarningToastAsync(string, string?, int?, string?, Func{ToastEventArgs, Task}?)"/>
    public Task<ToastResult> ShowWarningToastAsync(string title, string? message = null, int? lifetime = 5, string? dismissLabel = null, Func<ToastEventArgs, Task>? dismissOnClickAsync = null)
        => ShowSimpleToastAsync(ToastIntent.Warning, title, message, lifetime, dismissLabel, dismissOnClickAsync);

    /// <inheritdoc cref="INotificationService.ShowErrorToastAsync(string, string?, int?, string?, Func{ToastEventArgs, Task}?)"/>
    public Task<ToastResult> ShowErrorToastAsync(string title, string? message = null, int? lifetime = 5, string? dismissLabel = null, Func<ToastEventArgs, Task>? dismissOnClickAsync = null)
        => ShowSimpleToastAsync(ToastIntent.Error, title, message, lifetime, dismissLabel, dismissOnClickAsync);

    /// <inheritdoc cref="INotificationService.ShowInfoToastAsync(string, string?, int?, string?, Func{ToastEventArgs, Task}?)"/>
    public Task<ToastResult> ShowInfoToastAsync(string title, string? message = null, int? lifetime = 5, string? dismissLabel = null, Func<ToastEventArgs, Task>? dismissOnClickAsync = null)
        => ShowSimpleToastAsync(ToastIntent.Info, title, message, lifetime, dismissLabel, dismissOnClickAsync);

    /// <inheritdoc cref="INotificationService.ShowProgressToastAsync(string, string?, int?, string?, Func{ToastEventArgs, Task}?)"/>
    public Task<ToastResult> ShowProgressToastAsync(string title, string? message = null, int? lifetime = 5, string? dismissLabel = null, Func<ToastEventArgs, Task>? dismissOnClickAsync = null)
        => ShowSimpleToastAsync(ToastIntent.Progress, title, message, lifetime, dismissLabel, dismissOnClickAsync);

    /// <inheritdoc cref="INotificationService.ShowToastAsync(ToastOptions)"/>
    public async Task<ToastResult> ShowToastAsync(ToastOptions options)
    {
        var instance = ShowToastInstanceCore(componentType: null, options);
        await ServiceProvider.OnUpdatedAsync.Invoke(instance);
        return await instance.Result;
    }

    /// <inheritdoc cref="INotificationService.ShowToastAsync(Action{ToastOptions})"/>
    public Task<ToastResult> ShowToastAsync(Action<ToastOptions> options)
        => ShowToastAsync(new ToastOptions(options));

    /// <inheritdoc cref="INotificationService.ShowToastAsync{TToast}(ToastOptions)"/>
    public async Task<ToastResult> ShowToastAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TToast>(ToastOptions options)
        where TToast : ComponentBase
    {
        var instance = ShowToastInstanceCore(typeof(TToast), options);
        await ServiceProvider.OnUpdatedAsync.Invoke(instance);
        return await instance.Result;
    }

    /// <inheritdoc cref="INotificationService.ShowToastAsync{TToast}(ToastOptions)"/>
    public Task<ToastResult> ShowToastAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TToast>(Action<ToastOptions> options)
            where TToast : ComponentBase
        => ShowToastAsync<TToast>(new ToastOptions(options));

    /// <inheritdoc cref="INotificationService.GetToastInstance(string)"/>
    public IToastInstance? GetToastInstance(string id)
    {
        if (ServiceProvider.Items.TryGetValue(id, out var instance) && instance is IToastInstance toastInstance)
        {
            return toastInstance;
        }

        return null;
    }

    /// <inheritdoc cref="INotificationService.CloseAsync(IToastInstance, object?)"/>
    public Task CloseAsync(IToastInstance toast, object? data = null)
    {
        if (data is not null and ToastResult result)
        {
            return CloseCoreAsync(toast, result);
        }

        return CloseCoreAsync(toast, ToastResult.OfProgrammatic(toast, data));
    }

    /// <inheritdoc cref="INotificationService.CloseAllToastsAsync"/>
    public async Task<int> CloseAllToastsAsync()
    {
        var toasts = ServiceProvider.Items.Values.Where(item => item is IToastInstance).Cast<IToastInstance>().ToList();

        foreach (var toast in toasts)
        {
            await CloseCoreAsync(toast, ToastResult.OfProgrammatic(toast));
        }

        return toasts.Count;
    }

    /// <summary>
    /// Internal helper that builds and shows a simple intent-based toast.
    /// </summary>
    private Task<ToastResult> ShowSimpleToastAsync(ToastIntent intent, string title, string? message, int? lifetime, string? dismissLabel, Func<ToastEventArgs, Task>? dismissOnClickAsync)
    {
        var options = new ToastOptions
        {
            Intent = intent,
            Title = title,
            Message = message,
            Lifetime = lifetime.HasValue ? TimeSpan.FromSeconds(lifetime.Value) : null,
        };

        if (!string.IsNullOrEmpty(dismissLabel))
        {
            options.AllowDismiss = true;
            options.DismissAction.Label = dismissLabel;
            options.DismissAction.OnClickAsync = async (e) =>
            {
                if (dismissOnClickAsync is not null)
                {
                    await dismissOnClickAsync.Invoke(e);
                }

                await e.Instance.CloseAsync(ToastCloseReason.Dismissed);
            };
        }

        return ShowToastAsync(options);
    }

    /// <summary />
    private ToastInstance ShowToastInstanceCore([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type? componentType, ToastOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (this.ProviderNotAvailable())
        {
            throw new FluentServiceProviderException<FluentToastProvider>();
        }

        var instance = new ToastInstance(this, componentType, options);

        // Add the Toast to the service.
        if (!ServiceProvider.Items.TryAdd(instance.Id, instance))
        {
            throw new InvalidOperationException($"A Toast with the ID '{instance.Id}' is already registered.");
        }

        // Raise the initial ToastLifecycleStatus.Queued event.
        instance.SetStatus(ToastLifecycleStatus.Queued);

        return instance;
    }

    /// <summary />
    private async Task CloseCoreAsync(IToastInstance toast, ToastResult result)
    {
        if (toast is not ToastInstance instance)
        {
            return;
        }

        if (instance.LifecycleStatus == ToastLifecycleStatus.Unmounted)
        {
            return;
        }

        // Raise the ToastLifecycleStatus.Dismissed event before to remove the Toast from the provider.
        instance.SetStatus(ToastLifecycleStatus.Dismissed);

        // Remove the Toast from the ToastProvider.
        await RemoveToastFromProviderAsync(instance);

        // Set the result of the Toast.        
        instance.ResultCompletion.TrySetResult(result);
    }

    /// <summary>
    /// Removes the Toast from the ToastProvider.
    /// </summary>
    internal async Task RemoveToastFromProviderAsync(IToastInstance? toast)
    {
        if (toast is null || toast is not ToastInstance instance)
        {
            return;
        }

        // Update the Toast.Opened parameter to false to trigger the closing animation.
        await instance.UpdateOpenedAsync(false);

        // Fire-and-forget: schedule the removal without blocking the caller.
        // to let the UI update and the ToastLifecycleStatus.Dismissed event to be processed first.
        // to let the CSS animation to complete before the Toast is removed from the memory.
        _ = Task.Run(async () =>
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            if (ServiceProvider.Items.TryRemove(toast.Id, out _))
            {
                // Raise the final ToastLifecycleStatus.Unmounted event.
                instance.SetStatus(ToastLifecycleStatus.Unmounted);

                await ServiceProvider.OnUpdatedAsync.Invoke(toast);
            }
        });
    }
}
