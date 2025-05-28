// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
///  Represents a message instance used with the <see cref="IMessageService"/>.
/// </summary>
public class MessageInstance : IMessageInstance
{
    private static long _counter;
    //private readonly Type _componentType;
    //internal readonly TaskCompletionSource<MessageResult> ResultCompletion = new();

    /// <summary />
    internal MessageInstance(IMessageService messageService, /*Type componentType,*/ MessageOptions options)
    {
        //_componentType = componentType;
        Options = options;
        MessageService = messageService;
        Id = string.IsNullOrEmpty(options.Id) ? Identifier.NewId() : options.Id;
        Index = Interlocked.Increment(ref _counter);
    }

    ///// <summary />
    //Type IMessageInstance.ComponentType => _componentType;

    /// <summary />
    internal IMessageService MessageService { get; }

    /// <summary />
    internal FluentMessageBar? FluentMessageBar { get; set; }

    /// <inheritdoc cref="IMessageInstance.Options"/>
    public MessageOptions Options { get; internal set; }

    /// <inheritdoc cref="IMessageInstance.Id"/>"
    public string Id { get; }

    /// <inheritdoc cref="IMessageInstance.Index"/>"
    public long Index { get; }

    /// <inheritdoc cref="IMessageInstance.DismissAsync()"/>
    public Task DismissAsync()
    {
        return MessageService.DismissAsync(this);
    }
}
