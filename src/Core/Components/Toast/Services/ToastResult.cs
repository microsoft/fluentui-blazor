// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents the result of a toast managed by the <see cref="INotificationService"/>.
/// </summary>
public class ToastResult
{
    internal static ToastResult OfDismissed(object? data = null) => new(ToastCloseReason.Dismissed, data);
    internal static ToastResult OfQuickAction(object? data = null) => new(ToastCloseReason.QuickAction, data);
    internal static ToastResult OfProgrammatic(object? data = null) => new(ToastCloseReason.Programmatic, data);
    internal static ToastResult OfTimedOut(object? data = null) => new(ToastCloseReason.TimedOut, data);

    /// <summary />
    protected internal ToastResult(ToastCloseReason reason, object? data)
    {
        Reason = reason;
        Data = data;
    }

    /// <summary>
    /// Gets the reason the toast was closed.
    /// </summary>
    public ToastCloseReason Reason { get; }

    /// <summary>
    /// Gets the optional data associated with the result.
    /// </summary>
    public object? Data { get; }
}
