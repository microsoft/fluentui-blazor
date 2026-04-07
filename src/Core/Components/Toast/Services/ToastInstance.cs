// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a toast instance used with the <see cref="IToastService"/>.
/// </summary>
public class ToastInstance : IToastInstance
{
    private static long _counter;
    internal readonly TaskCompletionSource<ToastCloseReason> ResultCompletion = new();

    /// <summary />
    internal ToastInstance(IToastService toastService, ToastOptions options)
    {
        Options = options;
        ToastService = toastService;
        Id = string.IsNullOrEmpty(options.Id) ? Identifier.NewId() : options.Id;
        Index = Interlocked.Increment(ref _counter);
    }

    /// <summary />
    internal IToastService ToastService { get; }

    /// <summary />
    internal FluentToast? FluentToast { get; set; }

    /// <summary />
    internal ToastCloseReason? PendingCloseReason { get; set; }

    /// <inheritdoc cref="IToastInstance.Options"/>
    public ToastOptions Options { get; internal set; }

    /// <inheritdoc cref="IToastInstance.Result"/>
    public Task<ToastCloseReason> Result => ResultCompletion.Task;

    /// <inheritdoc cref="IToastInstance.LifecycleStatus"/>
    public ToastLifecycleStatus LifecycleStatus { get; internal set; } = ToastLifecycleStatus.Queued;

    /// <inheritdoc cref="IToastInstance.Id"/>
    public string Id { get; }

    /// <inheritdoc cref="IToastInstance.Index"/>
    public long Index { get; }

    /// <inheritdoc cref="IToastInstance.CloseAsync()"/>
    public Task CloseAsync()
    {
        return ToastService.CloseAsync(this, ToastCloseReason.Programmatic);
    }

    /// <inheritdoc cref="IToastInstance.CloseAsync(ToastCloseReason)"/>
    public Task CloseAsync(ToastCloseReason reason)
    {
        return ToastService.CloseAsync(this, reason);
    }

    /// <inheritdoc cref="IToastInstance.DismissAsync()"/>
    public Task DismissAsync()
    {
        return ToastService.DismissAsync(this);
    }

    /// <inheritdoc cref="IToastInstance.UpdateAsync(Action{ToastOptions})"/>
    public Task UpdateAsync(Action<ToastOptions> update)
    {
        return ToastService.UpdateToastAsync(this, update);
    }
}
