// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Interface for implementing a MessageService
/// </summary>
public partial interface IMessageService : IFluentServiceBase<IMessageInstance>
{
    ///// <summary />
    //Action MessageItemsUpdated { get; set; }

    ///// <summary />
    //Func<Task> OnMessageItemsUpdatedAsync { get; set; }

    /// <summary>
    /// Dismisses the message.
    /// </summary>
    /// <param name="message">Instance of the message to close.</param>
    /// <returns></returns>
    Task DismissAsync(IMessageInstance message);

    /// <summary />
    IEnumerable<IMessageInstance> MessagesToShow(int count = 5, string? section = null);

    /// <summary />
    Task<IMessageInstance> ShowMessageBarAsync(Action<MessageOptions> options);

    /// <summary />
    Task<IMessageInstance> ShowMessageBarAsync(string title);

    /// <summary />
    Task<IMessageInstance> ShowMessageBarAsync(string title, MessageIntent intent);

    /// <summary />
    Task<IMessageInstance> ShowMessageBarAsync(string title, MessageIntent intent, string section);

    /// <summary />
    void Clear(string? section = null);

    /// <summary />
    void Remove(IMessageInstance message);

    /// <summary />
    int Count(string? section);
}
