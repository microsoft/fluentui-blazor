using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public class MessageBarService : IMessageBarService, IDisposable
{
    private readonly NavigationManager? _navigationManager;

    /// <summary />
    public MessageBarService()
    {
        //Configuration = new MessageBarGlobalOptions();
    }

    //// <summary />
    //public MessageService(NavigationManager navigationManager)
    //    : this(navigationManager) //, new MessageBarGlobalOptions())
    //{
    //}

    /// <summary />
    public MessageBarService(NavigationManager navigationManager) //, MessageBarGlobalOptions commonOptions)
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
    private List<MessageBarContent> MessageList { get; } = new List<MessageBarContent>();

    //// <summary />
    //public virtual MessageBarGlobalOptions Configuration { get; }

    /// <summary />
    public virtual IEnumerable<MessageBarContent> AllMessages
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
    public virtual IEnumerable<MessageBarContent> MessagesShown(int count = 5, string? category = null)
    {
        MessageLock.EnterReadLock();
        try
        {
            var messages = string.IsNullOrEmpty(category)
                       ? MessageList
                       : MessageList.Where(x => x.Section == category);

            return count > 0 ? messages.Take(count) : messages;
        }
        finally
        {
            MessageLock.ExitReadLock();
        }
    }

    /// <summary />
    public MessageBarContent Add(string message)
    {
        return Add(options =>
        {
            options.Title = message;
        });
    }

    /// <summary />
    public MessageBarContent Add(string message, MessageBarIntent intent)
    {
        return Add(options =>
        {
            options.Title = message;
            options.Intent = intent;
        });
    }

    /// <summary />
    public MessageBarContent Add(string message, string category, MessageBarIntent intent)
    {
        return Add(options =>
        {
            options.Title = message;
            options.Section = category;
            options.Intent = intent;
        });
    }

    /// <summary />
    public virtual MessageBarContent Add(Action<MessageBarOptions> options)
    {
        MessageBarOptions? configuration = new();
        options.Invoke(configuration);

        MessageBarContent? message = new(configuration);

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
    public virtual void Remove(MessageBarContent message)
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

        IEnumerable<MessageBarContent>? messages = string.IsNullOrEmpty(category)
                   ? MessageList
                   : MessageList.Where(i => i.Section == category);

        foreach (MessageBarContent message in messages)
        {
            message.OnClose -= Remove;
        }

        if (string.IsNullOrEmpty(category))
        {
            MessageList.Clear();
        }
        else
        {
            ((List<MessageBarContent>)MessageList).RemoveAll(i => i.Section == category);
        }
    }

    public int Count(string? category) => category is null
            ? MessageList.Count
            : MessageList.Count(x => x.Section == category);
}
