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
        
    }

    /// <summary />
    public MessageService(NavigationManager navigationManager) //, MessageBarGlobalOptions commonOptions)
    {
        _navigationManager = navigationManager;
        _navigationManager.LocationChanged += NavigationManager_LocationChanged;
    }

    /// <summary />
    public event Action? OnMessageItemsUpdated = default!;

    /// <summary />
    private ReaderWriterLockSlim MessageLock { get; } = new ReaderWriterLockSlim();

    /// <summary />
    private List<Message> MessageList { get; } = new List<Message>();

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

    /// <summary />
    public Message Add(string message)
    {
        return Add(options =>
        {
            options.Title = message;
            options.Intent = MessageIntent.Info;
        });
    }

    /// <summary />
    public Message Add(string message, MessageIntent intent)
    {
        return Add(options =>
        {
            options.Title = message;
            options.Intent = intent;
            options.Section = string.Empty; 
        });
    }

    /// <summary />
    public Message Add(string message, MessageIntent intent, string section)
    {
        return Add(options =>
        {
            options.Title = message;
            options.Intent = intent;
            options.Section = section;
        });
    }

    /// <summary />
    public virtual Message Add(Action<MessageOptions> options)
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

    /// <summary />
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
        MessagesToShow().Where(s => s.Options.ClearAfterNavigation)
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

        RemoveMessageItems(section: null);
    }

    /// <summary />
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
            ((List<Message>)MessageList).RemoveAll(i => i.Section == section);
        }
    }

    public int Count(string? section) => section is null
            ? MessageList.Count
            : MessageList.Count(x => x.Section == section);
}
