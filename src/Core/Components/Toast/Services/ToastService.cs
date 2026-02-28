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
public partial class ToastService : FluentServiceBase<IToastInstance>, IToastService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IJSRuntime _jsRuntime;

    /// <summary>
    /// Initializes a new instance of the <see cref="ToastService"/> class.
    /// </summary>
    /// <param name="serviceProvider">List of services available in the application.</param>
    /// <param name="localizer">Localizer for the application.</param>
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(ToastEventArgs))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(ToastInstance))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(IToastInstance))]
    public ToastService(IServiceProvider serviceProvider, IFluentLocalizer? localizer)
    {
        _serviceProvider = serviceProvider;
        _jsRuntime = serviceProvider.GetRequiredService<IJSRuntime>();
        Localizer = localizer ?? FluentLocalizerInternal.Default;
    }

    /// <summary />
    protected IFluentLocalizer Localizer { get; }

    /// <inheritdoc cref="IToastService.CloseAsync(IToastInstance, ToastResult)"/>
    public async Task CloseAsync(IToastInstance Toast, ToastResult result)
    {
        var ToastInstance = Toast as ToastInstance;

        // Raise the ToastState.Closing event
        ToastInstance?.FluentToast?.RaiseOnStateChangeAsync(Toast, DialogState.Closing);

        // Remove the Toast from the ToastProvider
        await RemoveToastFromProviderAsync(Toast);

        // Set the result of the Toast
        ToastInstance?.ResultCompletion.TrySetResult(result);

        // Raise the ToastState.Closed event
        ToastInstance?.FluentToast?.RaiseOnStateChangeAsync(Toast, DialogState.Closed);
    }

    /// <inheritdoc cref="IToastService.ShowToastAsync{TToast}(ToastOptions)"/>
    public Task<ToastResult> ShowToastAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TToast>(ToastOptions options) where TToast : ComponentBase
    {
        return ShowToastAsync(typeof(TToast), options);
    }

    /// <inheritdoc cref="IToastService.ShowToastAsync{TToast}(Action{ToastOptions})"/>
    public Task<ToastResult> ShowToastAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TToast>(Action<ToastOptions> options) where TToast : ComponentBase
    {
        return ShowToastAsync(typeof(TToast), new ToastOptions(options));
    }

    /// <summary />
    private async Task<ToastResult> ShowToastAsync([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type componentType, ToastOptions options)
    {
        if (!componentType.IsSubclassOf(typeof(ComponentBase)))
        {
            throw new ArgumentException($"{componentType.FullName} must be a Blazor Component", nameof(componentType));
        }

        if (this.ProviderNotAvailable())
        {
            throw new FluentServiceProviderException<FluentToastProvider>();
        }

        var instance = new ToastInstance(this, componentType, options);

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
