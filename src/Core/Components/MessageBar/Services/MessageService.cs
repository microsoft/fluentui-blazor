using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public class MessageService : IMessageService, IDisposable
{
    private readonly NavigationManager? _navigationManager;

    /// <summary />
    public MessageService()
    {

    }

    /// <summary />
    public MessageService(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
        _navigationManager.LocationChanged += NavigationManager_LocationChanged;
    }

    /// <summary />
    public event Action OnMessageItemsUpdated = default!;
    /// <summary />
    public event Func<Task> OnMessageItemsUpdatedAsync = default!;

    /// <summary />
    private ReaderWriterLockSlim MessageLock { get; } = new ReaderWriterLockSlim();

    /// <summary />
    private List<Message> MessageList { get; } = [];

    /// <summary>
    /// Gets all messages.
    /// </summary>
    public virtual IEnumerable<Message> AllMessages
    {
        get
        {
            MessageLock.EnterReadLock();
            try
            {
                return MessageList;
            }
            finally
            {
                MessageLock.ExitReadLock();
            }
        }
    }

    /// <summary>
    /// Retrieve messages to show in the message bar.
    /// </summary>
    /// <param name="count">Number of messages to get (defaults to 5)</param>
    /// <param name="section">Optional section to retrieve messages for</param>
    /// <returns></returns>
    public virtual IEnumerable<Message> MessagesToShow(int count = 5, string? section = null)
    {
        MessageLock.EnterReadLock();
        try
        {
            IEnumerable<Message>? messages = string.IsNullOrEmpty(section)
                       ? MessageList
                       : MessageList.Where(x => x.Section == section);

            return count > 0 ? messages.Take(count) : messages;
        }
        finally
        {
            MessageLock.ExitReadLock();
        }
    }

    /// <summary>
    /// Show a message based on the provided parameters in a message bar.
    /// </summary>
    /// <param name="title">Main info</param>
    /// <returns></returns>
    public Message ShowMessageBar(string title)
    {
        return ShowMessageBar(options =>
        {
            options.Title = title;
            options.Intent = MessageIntent.Info;
        });
    }

    /// <summary>
    /// Show a message based on the provided parameters in a message bar.
    /// </summary>
    /// <param name="title">Main info</param>
    /// <param name="intent">Intent of the message</param>
    /// <returns></returns>
    public Message ShowMessageBar(string title, MessageIntent intent)
    {
        return ShowMessageBar(options =>
        {
            options.Title = title;
            options.Intent = intent;
            options.Section = string.Empty;
        });
    }

    /// <summary>
    /// Show a message based on the provided parameters in a message bar.
    /// </summary>
    /// <param name="title">Main info</param>
    /// <param name="intent">Intent of the message</param>
    /// <param name="section">Section to show the messagebar in </param>
    /// <returns></returns>
    public Message ShowMessageBar(string title, MessageIntent intent, string section)
    {
        return ShowMessageBar(options =>
        {
            options.Title = title;
            options.Intent = intent;
            options.Section = section;
        });
    }

    /// <summary>
    /// Show a message based on the provided options in a message bar.
    /// </summary>
    /// <param name="options">Message options</param>
    /// <returns></returns>
    public virtual Message ShowMessageBar(Action<MessageOptions> options)
    {
        MessageOptions? configuration = new();
        options.Invoke(configuration);

        Message? message = new(configuration);

        MessageLock.EnterWriteLock();
        try
        {
            message.OnClose += Remove;
            MessageList.Add(message);
        }
        finally
        {
            MessageLock.ExitWriteLock();
        }

        OnMessageItemsUpdated?.Invoke();

        return message;
    }

    /// <summary>
    /// Show a message based on the provided parameters in a message bar.
    /// </summary>
    /// <param name="title">Main info</param>
    /// <returns></returns>
    public async Task<Message> ShowMessageBarAsync(string title)
    {
        return await ShowMessageBarAsync(options =>
        {
            options.Title = title;
            options.Intent = MessageIntent.Info;
        });
    }

    /// <summary>
    /// Show a message based on the provided parameters in a message bar.
    /// </summary>
    /// <param name="title">Main info</param>
    /// <param name="intent">Intent of the message</param>
    /// <returns></returns>
    public async Task<Message> ShowMessageBarAsync(string title, MessageIntent intent)
    {
        return await ShowMessageBarAsync(options =>
        {
            options.Title = title;
            options.Intent = intent;
            options.Section = string.Empty;
        });
    }

    /// <summary>
    /// Show a message based on the provided parameters in a message bar.
    /// </summary>
    /// <param name="title">Main info</param>
    /// <param name="intent">Intent of the message</param>
    /// <param name="section">Section to show the messagebar in </param>
    /// <returns></returns>
    public async Task<Message> ShowMessageBarAsync(string title, MessageIntent intent, string section)
    {
        return await ShowMessageBarAsync(options =>
        {
            options.Title = title;
            options.Intent = intent;
            options.Section = section;
        });
    }

    /// <summary>
    /// Show a message based on the provided message options in a message bar.
    /// </summary>
    /// <param name="options">Message options</param>
    /// <returns></returns>
    public virtual async Task<Message> ShowMessageBarAsync(Action<MessageOptions> options)
    {
        MessageOptions? configuration = new();
        options.Invoke(configuration);

        Message? message = new(configuration);

        MessageLock.EnterWriteLock();
        try
        {
            message.OnClose += Remove;
            MessageList.Add(message);
        }
        finally
        {
            MessageLock.ExitWriteLock();
        }

        await OnMessageItemsUpdatedAsync!.Invoke();

        return message;
    }

    /// <summary>
    /// Clear all messages (per section, if provided) from the message bar.
    /// </summary>
    /// <param name="section">Optional section</param>
    public virtual void Clear(string? section = null)
    {
        MessageLock.EnterWriteLock();
        try
        {
            RemoveMessageItems(section);
        }
        finally
        {
            MessageLock.ExitWriteLock();
        }

        OnMessageItemsUpdated?.Invoke();
    }

    /// <summary>
    /// Remove a message from the message bar.
    /// </summary>
    /// <param name="message">Message to remove</param>
    public virtual void Remove(Message message)
    {
        message.OnClose -= Remove;
        message.Options.OnClose?.Invoke(message); //.SafeFireAndForget();

        MessageLock.EnterWriteLock();
        try
        {
            var index = MessageList.IndexOf(message);
            if (index < 0)
            {
                return;
            }

            MessageList.RemoveAt(index);
        }
        finally
        {
            MessageLock.ExitWriteLock();
        }

        OnMessageItemsUpdated?.Invoke();
    }

    /// <summary />
    private void NavigationManager_LocationChanged(object? sender, LocationChangedEventArgs e)
    {
        MessagesToShow().Where(s => s.Options.ClearAfterNavigation)
                     .ToList()
                     .ForEach(s => Remove(s));
    }

    /// <summary />
    public void Dispose()
    {
        if (_navigationManager != null)
        {
            _navigationManager.LocationChanged -= NavigationManager_LocationChanged;
        }

        RemoveMessageItems(section: null);
    }

    /// <summary>
    /// Remove all messages (per section, if provided) from the message bar.
    /// </summary>
    /// <param name="section">Optional section</param>
    private void RemoveMessageItems(string? section = null)
    {
        if (MessageList.Count == 0)
        {
            return;
        }

        IEnumerable<Message>? messages = string.IsNullOrEmpty(section)
            ? MessageList
            : MessageList.Where(i => i.Section == section);

        foreach (Message message in messages)
        {
            message.OnClose -= Remove;
        }

        if (string.IsNullOrEmpty(section))
        {
            MessageList.Clear();
        }
        else
        {
            MessageList.RemoveAll(i => i.Section == section);
        }
    }

    /// <summary>
    /// Count the number of messages (per section, if provided) in the message bar .
    /// </summary>
    /// <param name="section">Optional section</param>
    /// <returns>int</returns>
    public int Count(string? section) => section is null
        ? MessageList.Count
        : MessageList.Count(x => x.Section == section);
}
