// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents the result of a message bar managed by the <see cref="IMessageBarService"/>.
/// </summary>
public class MessageBarResult
{
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

    /// <summary>
    /// Gets a value indicating whether the message bar was dismissed by the user.
    /// </summary>
    public bool Dismissed => Reason == MessageBarCloseReason.Dismissed;

    /// <summary>
    /// Gets a value indicating whether the message bar closed because its lifetime elapsed.
    /// </summary>
    public bool TimedOut => Reason == MessageBarCloseReason.TimedOut;

    /// <summary>
    /// Creates a <see cref="MessageBarResult"/> describing a user-driven dismissal.
    /// </summary>
    public static MessageBarResult OfDismissed(object? data = null) => new(MessageBarCloseReason.Dismissed, data);

    /// <summary>
    /// Creates a <see cref="MessageBarResult"/> describing a programmatic close.
    /// </summary>
    public static MessageBarResult OfProgrammatic(object? data = null) => new(MessageBarCloseReason.Programmatic, data);

    /// <summary>
    /// Creates a <see cref="MessageBarResult"/> describing an automatic close after the lifetime elapsed.
    /// </summary>
    public static MessageBarResult OfTimedOut(object? data = null) => new(MessageBarCloseReason.TimedOut, data);
}
