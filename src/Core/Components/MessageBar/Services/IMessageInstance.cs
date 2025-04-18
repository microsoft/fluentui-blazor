// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Interface for MessageInstance
/// </summary>

public interface IMessageInstance
{
    ///// <summary>
    ///// Gets the component type of the message.
    ///// </summary>
    //internal Type ComponentType { get; }

    /// <summary>
    /// Gets the unique identifier for the message.
    /// If this value is not set in the <see cref="MessageOptions"/>, a new identifier is generated.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Gets the index of the message (sequential number).
    /// </summary>
    long Index { get; }

    /// <summary>
    /// Gets the options used to configure the message.
    /// </summary>
    MessageOptions Options { get; }

    ///// <summary>
    ///// Gets the result of the message.
    ///// </summary>
    //Task<MessageResult> Result { get; }

    /// <summary>
    /// Dismisses the message.
    /// </summary>
    /// <returns></returns>
    Task DismissAsync();

    ///// <summary>
    ///// Closes the message with the specified result.
    ///// </summary>
    ///// <returns></returns>
    //Task CloseAsync();

    ///// <summary>
    ///// Closes the message with the specified result.
    ///// </summary>
    ///// <param name="result">Result to close the message with.</param>
    ///// <returns></returns>
    //Task CloseAsync(MessageResult result);

    ///// <summary>
    ///// Closes the message with the specified result.
    ///// </summary>
    ///// <param name="result">Result to close the message with.</param>
    ///// <returns></returns>
    //Task CloseAsync<T>(T result);
}
