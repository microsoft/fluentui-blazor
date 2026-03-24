// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Service for showing Toasts.
/// </summary>
public partial class ToastService : FluentServiceBase<IToastInstance>, IToastService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IJSRuntime _jsRuntime;

    /// <summary>
    /// Initializes a new instance of the <see cref="ToastService"/> class.
    /// </summary>
    /// <param name="serviceProvider">List of services available in the application.</param>
    /// <param name="localizer">Localizer for the application.</param>
    public ToastService(IServiceProvider serviceProvider, IFluentLocalizer? localizer)
    {
        _serviceProvider = serviceProvider;
        _jsRuntime = serviceProvider.GetRequiredService<IJSRuntime>();
        Localizer = localizer ?? FluentLocalizerInternal.Default;
    }

    /// <summary />
    protected IFluentLocalizer Localizer { get; }

    /// <inheritdoc cref="IToastService.CloseAsync(IToastInstance, ToastCloseReason)"/>
    public async Task CloseAsync(IToastInstance Toast, ToastCloseReason reason)
    {
        var ToastInstance = Toast as ToastInstance;

        if (ToastInstance?.FluentToast is FluentToast fluentToast)
        {
            ToastInstance.PendingCloseReason = reason;
            await fluentToast.RequestCloseAsync();
            return;
        }

        // Raise the ToastState.Closing event
        ToastInstance?.FluentToast?.RaiseOnStateChangeAsync(Toast, DialogState.Closing);

        // Remove the Toast from the ToastProvider
        await RemoveToastFromProviderAsync(Toast);

        // Set the result of the Toast
        ToastInstance?.ResultCompletion.TrySetResult(reason);

        // Raise the ToastState.Closed event
        ToastInstance?.FluentToast?.RaiseOnStateChangeAsync(Toast, DialogState.Closed);
    }

    /// <inheritdoc cref="IToastService.ShowToastAsync(ToastOptions)"/>
    public Task<ToastCloseReason> ShowToastAsync(ToastOptions? options = null)
    {
        return ShowToastCoreAsync(options ?? new ToastOptions());
    }

    /// <inheritdoc cref="IToastService.ShowToastAsync(Action{ToastOptions})"/>
    public Task<ToastCloseReason> ShowToastAsync(Action<ToastOptions> options)
    {
        return ShowToastAsync(new ToastOptions(options));
    }

    /// <inheritdoc cref="IToastService.UpdateToastAsync(IToastInstance, Action{ToastOptions})"/>
    public async Task UpdateToastAsync(IToastInstance toast, Action<ToastOptions> update)
    {
        if (toast is not ToastInstance instance)
        {
            throw new ArgumentException($"{nameof(toast)} must be a {nameof(ToastInstance)}.", nameof(toast));
        }

        update(instance.Options);
        await ServiceProvider.OnUpdatedAsync.Invoke(instance);
    }

    /// <summary />
    private async Task<ToastCloseReason> ShowToastCoreAsync(ToastOptions options)
    {
        if (this.ProviderNotAvailable())
        {
            throw new FluentServiceProviderException<FluentToastProvider>();
        }

        var instance = new ToastInstance(this, options);

        // Add the Toast to the service, and render it.
        ServiceProvider.Items.TryAdd(instance?.Id ?? "", instance ?? throw new InvalidOperationException("Failed to create FluentToast."));
        await ServiceProvider.OnUpdatedAsync.Invoke(instance);

        return await instance.Result;
    }

    /// <summary>
    /// Removes the Toast from the ToastProvider.
    /// </summary>
    /// <param name="Toast"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    internal async Task RemoveToastFromProviderAsync(IToastInstance? Toast)
    {
        if (Toast is null)
        {
            return;
        }

        // Remove the HTML code from the ToastProvider
        if (!ServiceProvider.Items.TryRemove(Toast.Id, out _))
        {
            throw new InvalidOperationException($"Failed to remove Toast from ToastProvider: the ID '{Toast.Id}' doesn't exist in the ToastServiceProvider.");
        }

        await ServiceProvider.OnUpdatedAsync.Invoke(Toast);
    }
}
