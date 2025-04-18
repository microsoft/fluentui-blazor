// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Service for showing messages in a MessageBar or notification center.
/// </summary>
public class MessageService : FluentServiceBase<IMessageInstance>, IMessageService, IAsyncDisposable
{
    private readonly NavigationManager? _navigationManager;
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageService"/> class.
    /// </summary>
    /// <param name="serviceProvider">List of services available in the application.</param>
    /// <param name="localizer">Localizer for the application.</param>
    /// <param name="navigationManager">Navigation manager for the application.</param>
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(MessageBarEventArgs))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(MessageInstance))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(IMessageInstance))]
    public MessageService(IServiceProvider serviceProvider, IFluentLocalizer? localizer, NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
        _navigationManager.LocationChanged += NavigationManager_LocationChanged;
        _serviceProvider = serviceProvider;
        Localizer = localizer ?? FluentLocalizerInternal.Default;
    }

    /// <summary>
    /// Gets or sets the provider ID.
    /// </summary>
    public string? ProviderId { get; set; }

    /// <summary />
    protected IFluentLocalizer Localizer { get; }

    /// <inheritdoc cref="IMessageService.DismissAsync(IMessageInstance)"/>
    public async Task DismissAsync(IMessageInstance message)
    {
        var messageInstance = message as MessageInstance;

        // Raise the DialogState.Dismissing event
        messageInstance?.FluentMessageBar?.RaiseOnStateChangeAsync(message, MessageBarState.Dismissing);

        // Remove the message from the MessageProvider
        await RemoveMessageFromProviderAsync(message);

        // Raise the MessageState.Dismissed event
        messageInstance?.FluentMessageBar?.RaiseOnStateChangeAsync(messageInstance, MessageBarState.Dismissed);
    }

    ///// <summary />
    //public Action MessageItemsUpdated { get; set; } = default!;
    ///// <summary />
    //public Func<Task> OnMessageItemsUpdatedAsync { get; set; } = default!;

    /// <summary />
    private ReaderWriterLockSlim MessageLock { get; } = new ReaderWriterLockSlim();

    /// <summary>
    /// Retrieve messages to show in the message bar.
    /// </summary>
    /// <param name="count">Number of messages to get (defaults to 5)</param>
    /// <param name="section">Optional section to retrieve messages for</param>
    /// <returns></returns>
    public virtual IEnumerable<IMessageInstance> MessagesToShow(int count = 5, string? section = null)
    {
        MessageLock.EnterReadLock();
        try
        {
            var messages = string.IsNullOrEmpty(section)
                       ? ServiceProvider.Items.Values
                       : ServiceProvider.Items.Values.Where(x => string.Equals(x.Options.Section, section, StringComparison.OrdinalIgnoreCase));

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
    public async Task<IMessageInstance> ShowMessageBarAsync(string title)
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
    public async Task<IMessageInstance> ShowMessageBarAsync(string title, MessageIntent intent)
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
    /// <param name="section">Section to show the message bar in </param>
    /// <returns></returns>
    public async Task<IMessageInstance> ShowMessageBarAsync(string title, MessageIntent intent, string section)
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
    public virtual async Task<IMessageInstance> ShowMessageBarAsync(Action<MessageOptions> options)
    {
        if (this.ProviderNotAvailable())
        {
            throw new FluentServiceProviderException<FluentMessageBarProvider>();
        }

        var messageOptions = new MessageOptions();
        options(messageOptions);

        var instance = new MessageInstance(this, messageOptions);

        // Add the message to the service, and render it.
        ServiceProvider.Items.TryAdd(instance?.Id ?? "", instance ?? throw new InvalidOperationException("Failed to create FluentMessageBar."));

        await ServiceProvider.OnUpdatedAsync.Invoke(instance);

        return instance;
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
            RemoveMessages(section);
        }
        finally
        {
            MessageLock.ExitWriteLock();
        }

        //MessageItemsUpdated?.Invoke();
    }

    /// <summary>
    /// Remove a message from the message bar.
    /// </summary>
    /// <param name="message">Message to remove</param>
    public virtual void Remove(IMessageInstance message)
    {
        //message.OnClose -= Remove;
        //_ = message.Options.OnClose?.Invoke(message);

        MessageLock.EnterWriteLock();
        try
        {
            var index = ServiceProvider.Items.Values.ToList().IndexOf(message);
            if (index < 0)
            {
                return;
            }

            ServiceProvider.Items.TryRemove(message.Id, out _);
        }
        finally
        {
            MessageLock.ExitWriteLock();
        }

        //MessageItemsUpdated?.Invoke();
    }

    /// <summary />
    private void NavigationManager_LocationChanged(object? sender, LocationChangedEventArgs e)
    {
        MessagesToShow().Where(s => s.Options.ClearAfterNavigation)
                     .ToList()
                     .ForEach(Remove);
    }

    /// <summary>
    /// Remove all messages (per section, if provided) from the message bar.
    /// </summary>
    /// <param name="section">Optional section</param>
    private void RemoveMessages(string? section = null)
    {
        if (ServiceProvider.Items.Values.Count == 0)
        {
            return;
        }

        var messages = string.IsNullOrEmpty(section)
            ? ServiceProvider.Items.Values
            : ServiceProvider.Items.Values.Where(i => string.Equals(i.Options.Section, section, StringComparison.OrdinalIgnoreCase));

        foreach (var message in messages)
        {
            //message.OnClose -= Remove;
        }

        if (string.IsNullOrEmpty(section))
        {
            ServiceProvider.Items.Clear();
        }
        else
        {
            //ServiceProvider.Items.Remove(i => string.Equals(i.Section, section, StringComparison.OrdinalIgnoreCase), out _);
        }
    }

    /// <summary>
    /// Count the number of messages (per section, if provided) in the message bar .
    /// </summary>
    /// <param name="section">Optional section</param>
    /// <returns>int</returns>
    public int Count(string? section) => section is null
        ? ServiceProvider.Items.Count
        : ServiceProvider.Items.Values.Count(x => string.Equals(x.Options.Section, section, StringComparison.OrdinalIgnoreCase));

    /// <summary>
    /// Removes the message from the MessageBarProvider.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    internal Task RemoveMessageFromProviderAsync(IMessageInstance? message)
    {
        if (message is null)
        {
            return Task.CompletedTask;
        }

        // Remove the HTML code from the MessageBarProvider
        if (!ServiceProvider.Items.TryRemove(message.Id, out _))
        {
            throw new InvalidOperationException($"Failed to remove message from MessageBarProvider: the ID '{message.Id}' doesn't exist in the MessageBar ServiceProvider.");
        }

        return ServiceProvider.OnUpdatedAsync.Invoke(message);
    }

    /// <summary>
    ///  Disposes the MessageService and cleans up resources.
    /// </summary>
    /// <returns></returns>
    public async ValueTask DisposeAsync()
    {
        if (_navigationManager != null)
        {
            _navigationManager.LocationChanged -= NavigationManager_LocationChanged;
        }

        RemoveMessages(section: null);

        // Dispose of any other resources if necessary
        await Task.CompletedTask;

        // Suppress finalization to comply with CA1816
        GC.SuppressFinalize(this);
    }
}
