// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a toast instance used with the <see cref="INotificationService"/>.
/// </summary>
public class ToastInstance : IToastInstance
{
    private static long _counter;
    internal readonly TaskCompletionSource<ToastResult> ResultCompletion = new();
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
    private readonly Type? _componentType;

    /// <summary />
    internal ToastInstance(INotificationService notificationService, ToastOptions options)
        : this(notificationService, componentType: null, options)
    {
    }

    /// <summary />
    internal ToastInstance(INotificationService notificationService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type? componentType, ToastOptions options)
    {
        Options = options;
        NotificationService = notificationService;
        _componentType = componentType;
        Id = string.IsNullOrEmpty(options.Id) ? Identifier.NewId() : options.Id;
        Index = Interlocked.Increment(ref _counter);
    }

    /// <summary>
    /// Gets or sets a callback that is invoked when the toast's opened state changes.
    /// </summary>
    internal Func<bool, Task> UpdateOpenedAsync { get; set; } = _ => Task.CompletedTask;

    /// <summary />
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
    Type? INotificationInstance.ComponentType => _componentType;

    /// <summary />
    internal INotificationService NotificationService { get; }

    /// <inheritdoc cref="IToastInstance.Options"/>
    public ToastOptions Options { get; internal set; }

    /// <inheritdoc cref="IToastInstance.Result"/>
    public Task<ToastResult> Result => ResultCompletion.Task;

    /// <inheritdoc cref="IToastInstance.LifecycleStatus"/>
    public ToastLifecycleStatus LifecycleStatus { get; internal set; } = ToastLifecycleStatus.Unmounted;

    /// <inheritdoc cref="INotificationInstance.Id"/>
    public string Id { get; }

    /// <inheritdoc cref="INotificationInstance.Index"/>
    public long Index { get; }

    /// <inheritdoc cref="INotificationInstance.CloseAsync()"/>
    public Task CloseAsync()
    {
        return NotificationService.CloseAsync(this);
    }

    /// <inheritdoc cref="IToastInstance.CloseAsync(ToastCloseReason, object?)"/>
    public Task CloseAsync(ToastCloseReason reason, object? data = null)
    {
        return NotificationService.CloseAsync(this, new ToastResult(reason, data));
    }

    /// <summary>
    /// Sets the lifecycle status of the toast 
    /// and invokes the <see cref="ToastOptions.OnStatusChange"/> callback if provided.
    /// </summary>
    /// <param name="status">The new lifecycle status of the toast.</param>
    internal void SetStatus(ToastLifecycleStatus status)
    {
        if (LifecycleStatus == status)
        {
            return;
        }

        LifecycleStatus = status;

        if (Options.OnStatusChange is not null)
        {
            var args = new ToastEventArgs(this, status);
            Options.OnStatusChange.Invoke(args);
        }
    }
}
