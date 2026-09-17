// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents the result of a toast managed by the <see cref="INotificationService"/>.
/// </summary>
public class ToastResult
{
    internal static ToastResult OfDismissed(IToastInstance instance, object? data = null) => new(instance, ToastCloseReason.Dismissed, data);
    internal static ToastResult OfQuickAction(IToastInstance instance, object? data = null) => new(instance, ToastCloseReason.QuickAction, data);
    internal static ToastResult OfProgrammatic(IToastInstance instance, object? data = null) => new(instance, ToastCloseReason.Programmatic, data);
    internal static ToastResult OfTimedOut(IToastInstance instance, object? data = null) => new(instance, ToastCloseReason.TimedOut, data);
    internal static ToastResult OfVisible(IToastInstance instance) => new(instance, ToastCloseReason.Programmatic, data: null);
    internal static ToastResult OfQueued(IToastInstance instance) => new(instance, ToastCloseReason.Programmatic, data: null);

    /// <summary />
    internal ToastResult(IToastInstance instance, ToastCloseReason reason, object? data)
    {
        ArgumentNullException.ThrowIfNull(instance);

        Reason = reason;
        Data = data;
        Instance = instance;
    }

    /// <summary>
    /// Gets the reason the toast was closed.
    /// </summary>
    public ToastCloseReason Reason { get; }

    /// <summary>
    /// Gets the optional data associated with the result.
    /// </summary>
    public object? Data { get; }

    /// <summary>
    /// Gets the toast instance associated with this result.
    /// </summary>
    public IToastInstance Instance { get; internal set; }
}
