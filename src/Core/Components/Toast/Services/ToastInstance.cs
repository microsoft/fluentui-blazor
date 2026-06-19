// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a toast instance used with the <see cref="INotificationService"/>.
/// </summary>
public class ToastInstance : IToastInstance
{
    private static long _counter;
    internal readonly TaskCompletionSource<ToastResult> ResultCompletion = new();
    private readonly Type? _componentType;

    /// <summary />
    internal ToastInstance(INotificationService notificationService, ToastOptions options)
        : this(notificationService, componentType: null, options)
    {
    }

    /// <summary />
    internal ToastInstance(INotificationService notificationService, Type? componentType, ToastOptions options)
    {
        Options = options;
        NotificationService = notificationService;
        _componentType = componentType;
        Id = string.IsNullOrEmpty(options.Id) ? Identifier.NewId() : options.Id;
        Index = Interlocked.Increment(ref _counter);
    }

    /// <summary />
    Type? INotificationInstance.ComponentType => _componentType;

    /// <summary />
    internal INotificationService NotificationService { get; }

    /// <inheritdoc cref="IToastInstance.Options"/>
    public ToastOptions Options { get; internal set; }

    /// <inheritdoc cref="IToastInstance.Result"/>
    public Task<ToastResult> Result => ResultCompletion.Task;

    /// <inheritdoc cref="IToastInstance.LifecycleStatus"/>
    public ToastLifecycleStatus LifecycleStatus { get; internal set; } = ToastLifecycleStatus.Visible;

    /// <inheritdoc cref="INotificationInstance.Id"/>
    public string Id { get; }

    /// <inheritdoc cref="INotificationInstance.Index"/>
    public long Index { get; }

    /// <inheritdoc cref="INotificationInstance.CloseAsync()"/>
    public Task CloseAsync()
    {
        return NotificationService.CloseAsync(this);
    }

    /// <inheritdoc cref="IToastInstance.CloseAsync(ToastResult)"/>
    public Task CloseAsync(ToastResult result)
    {
        return NotificationService.CloseAsync(this, result);
    }
}
