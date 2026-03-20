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
    internal readonly TaskCompletionSource<ToastResult> ResultCompletion = new();

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
    internal ToastResult? PendingResult { get; set; }

    /// <inheritdoc cref="IToastInstance.Options"/>
    public ToastOptions Options { get; internal set; }

    /// <inheritdoc cref="IToastInstance.Result"/>
    public Task<ToastResult> Result => ResultCompletion.Task;

    /// <inheritdoc cref="IToastInstance.Id"/>"
    public string Id { get; }

    /// <inheritdoc cref="IToastInstance.Index"/>"
    public long Index { get; }

    /// <inheritdoc cref="IToastInstance.CancelAsync()"/>
    public Task CancelAsync()
    {
        return ToastService.CloseAsync(this, ToastResult.Cancel());
    }

    /// <inheritdoc cref="IToastInstance.CloseAsync()"/>
    public Task CloseAsync()
    {
        return ToastService.CloseAsync(this, ToastResult.Ok());
    }

    /// <inheritdoc cref="IToastInstance.CloseAsync{T}(T)"/>
    public Task CloseAsync<T>(T result)
    {
        return ToastService.CloseAsync(this, ToastResult.Ok(result));
    }

    /// <inheritdoc cref="IToastInstance.CloseAsync(ToastResult)"/>
    public Task CloseAsync(ToastResult result)
    {
        return ToastService.CloseAsync(this, result);
    }

    /// <inheritdoc cref="IToastInstance.UpdateAsync(Action{ToastOptions})"/>
    public Task UpdateAsync(Action<ToastOptions> update)
    {
        return ToastService.UpdateToastAsync(this, update);
    }
}
