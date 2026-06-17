// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents the result of a message bar managed by the <see cref="IMessageBarService"/>.
/// </summary>
public class MessageBarResult
{
    internal static MessageBarResult OfDismissed(object? data = null) => new(MessageBarCloseReason.Dismissed, data);
    internal static MessageBarResult OfProgrammatic(object? data = null) => new(MessageBarCloseReason.Programmatic, data);
    internal static MessageBarResult OfTimedOut(object? data = null) => new(MessageBarCloseReason.TimedOut, data);

    /// <summary />
    protected internal MessageBarResult(MessageBarCloseReason reason, object? data)
    {
        Reason = reason;
        Data = data;
    }

    /// <summary>
    /// Gets the reason the message bar was closed.
    /// </summary>
    public MessageBarCloseReason Reason { get; }

    /// <summary>
    /// Gets the optional data associated with the result.
    /// </summary>
    public object? Data { get; }
}
