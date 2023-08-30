using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public class MessageService : IMessageService, IDisposable
{
    private readonly NavigationManager? _navigationManager;

    /// <summary />
    public MessageService()
    {
        //Configuration = new MessageBarGlobalOptions();
    }

    //// <summary />
    //public MessageService(NavigationManager navigationManager)
    //    : this(navigationManager) //, new MessageBarGlobalOptions())
    //{
    //}

    /// <summary />
    public MessageService(NavigationManager navigationManager) //, MessageBarGlobalOptions commonOptions)
    {
        _navigationManager = navigationManager;
        //Configuration = commonOptions;

        _navigationManager.LocationChanged += NavigationManager_LocationChanged;
    }

    /// <summary />
    public event Action? OnMessageItemsUpdated = default!;

    /// <summary />
    private ReaderWriterLockSlim MessageLock { get; } = new ReaderWriterLockSlim();

    /// <summary />
    private IList<Message> MessageList { get; } = new List<Message>();

    //// <summary />
    //public virtual MessageBarGlobalOptions Configuration { get; }

    /// <summary />
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

    /// <summary />
    public virtual IEnumerable<Message> MessagesShown(int count = 5, string? category = null)
    {
        MessageLock.EnterReadLock();
        try
        {
            var messages = string.IsNullOrEmpty(category)
                       ? MessageList
                       : MessageList.Where(x => x.Category == category);

            return count > 0 ? messages.Take(count) : messages;
        }
        finally
        {
            MessageLock.ExitReadLock();
        }
    }

    /// <summary />
    public Message Add(string message)
    {
        return Add(options =>
        {
            options.Title = message;
        });
    }

    /// <summary />
    public Message Add(string message, MessageType severity)
    {
        return Add(options =>
        {
            options.Title = message;
            options.Type = severity;
        });
    }

    /// <summary />
    public Message Add(string message, string category, MessageType severity)
    {
        return Add(options =>
        {
            options.Title = message;
            options.Category = category;
            options.Type = severity;
        });
    }

    /// <summary />
    public virtual Message Add(Action<MessageOptions> options)
    {
        //var configuration = new MessageOptions(Configuration);
        //options.Invoke(configuration);

        Message? message = new(); // (configuration);

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

    /// <summary />
    public virtual void Clear(string? category = null)
    {
        MessageLock.EnterWriteLock();
        try
        {
            RemoveMessageItems(category);
        }
        finally
        {
            MessageLock.ExitWriteLock();
        }

        OnMessageItemsUpdated?.Invoke();
    }

    /// <summary />
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
        //if (Configuration.ClearAfterNavigation)
        //{
        //    Clear();
        //}
        //else
        //{
        MessagesShown().Where(s => s.Options.ClearAfterNavigation)
                     .ToList()
                     .ForEach(s => Remove(s));
        //}
    }

    /// <summary />
    public void Dispose()
    {
        if (_navigationManager != null)
        {
            _navigationManager.LocationChanged -= NavigationManager_LocationChanged;
        }

        RemoveMessageItems(category: null);
    }

    /// <summary />
    private void RemoveMessageItems(string? category = null)
    {
        if (MessageList.Count == 0)
        {
            return;
        }

        IEnumerable<Message>? messages = string.IsNullOrEmpty(category)
                   ? MessageList
                   : MessageList.Where(i => i.Category == category);

        foreach (Message message in messages)
        {
            message.OnClose -= Remove;
        }

        if (string.IsNullOrEmpty(category))
        {
            MessageList.Clear();
        }
        else
        {
            ((List<Message>)MessageList).RemoveAll(i => i.Category == category);
        }
    }

    public int Count(string? category) => category is null
            ? MessageList.Count
            : MessageList.Count(x => x.Category == category);
}
