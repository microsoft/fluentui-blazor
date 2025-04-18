// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentMessageBarProvider : FluentComponentBase
{
    /// <summary />
    public FluentMessageBarProvider()
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    internal string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-messagebar-provider")
        .Build();

    /// <summary />
    internal string? StyleValue => DefaultStyleBuilder
        .Build();

    /// <summary>
    /// Gets or sets the injected service provider.
    /// </summary>
    [Inject]
    public IServiceProvider? ServiceProvider { get; set; }

    /// <summary>
    /// Gets or sets the injected navigation manager.
    /// </summary>
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    /// <summary />
    protected virtual IMessageService? MessageService => GetCachedServiceOrNull<IMessageService>();

    /// <summary>
    /// Display only messages for this section.
    /// </summary>
    [Parameter]
    public string? Section { get; set; }

    /// <summary>
    /// Displays messages as a single line (with the message only)
    /// or as a card (with the detailed message).
    /// </summary>
    [Parameter]
    public MessageType Type { get; set; } = MessageType.MessageBar;

    /// <summary>
    /// Maximum number of messages displayed. Rest is stored in memory to be displayed when an shown message is closed.
    /// Default value is 5
    /// Set a value equal to or less than zero, to display all messages for this <see cref="Section" /> (or all categories if not set).
    /// </summary>
    [Parameter]
    public int? MaxMessageCount { get; set; } = 5;

    /// <summary>
    /// Display the newest messages on top (true) or on bottom (false).
    /// </summary>
    [Parameter]
    public bool NewestOnTop { get; set; } = true;

    /// <summary>
    /// Clear all (shown and stored) messages when the user navigates to a new page.
    /// </summary>
    [Parameter]
    public bool ClearAfterNavigation { get; set; } = false;

    /// <summary />
    protected IEnumerable<IMessageInstance>? AllMessagesForSection
    {
        get
        {
            return string.IsNullOrEmpty(Section)
                          ? MessageService?.Items.Values
                          : MessageService?.Items.Values.Where(x => string.Equals(x.Options.Section, Section, StringComparison.OrdinalIgnoreCase));
        }
    }

    /// <summary />
    protected IEnumerable<IMessageInstance>? MessagesToShow
    {
        get
        {
            if (MaxMessageCount.HasValue)
            {
                var maxMessages = MaxMessageCount.Value > 0 ? MaxMessageCount.Value : int.MaxValue;

                return NewestOnTop
                            ? AllMessagesForSection?.Reverse().TakeLast(maxMessages)
                            : AllMessagesForSection?.TakeLast(maxMessages);
            }

            return NewestOnTop
                        ? MessageService?.MessagesToShow(-1, Section).Reverse()
                        : MessageService?.MessagesToShow(-1, Section);
        }
    }

    /// <summary />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (MessageService is not null)
        {
            MessageService.ProviderId = Id;
            MessageService.OnUpdatedAsync = async (item) =>
            {
                await InvokeAsync(StateHasChanged);
            };

            //MessageService.OnUpdatedAsync += OnMessageItemsUpdatedHandler;
            //MessageService.OnMessageItemsUpdatedAsync += OnMessageItemsUpdatedHandlerAsync;

            if (ClearAfterNavigation)
            {
                NavigationManager.LocationChanged += ClearMessages;
            }
        }
    }

    /// <summary>
    /// Only for Unit Tests
    /// </summary>
    /// <param name="id"></param>
    internal void UpdateId(string? id)
    {
        Id = id;

        if (MessageService is not null)
        {
            MessageService.ProviderId = id;
        }
    }

    /// <summary />
    protected virtual void OnMessageItemsUpdatedHandler()
    {
        _ = InvokeAsync(StateHasChanged);
    }

    /// <summary />
    protected virtual async Task OnMessageItemsUpdatedHandlerAsync()
    {
        await Task.Run(() =>
        {
            _ = InvokeAsync(StateHasChanged);
        });
    }

    private void ClearMessages(object? sender, LocationChangedEventArgs args)
    {
        if (AllMessagesForSection?.Any() == true)
        {
            _ = InvokeAsync(() =>
            {
                MessageService?.Clear(Section);
                StateHasChanged();
            });
        }
    }

    /// <summary />
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (MessageService is not null)
            {
                //MessageService.MessageItemsUpdated -= OnMessageItemsUpdatedHandler;
                //MessageService.OnMessageItemsUpdatedAsync -= OnMessageItemsUpdatedHandlerAsync;
            }

            NavigationManager.LocationChanged -= ClearMessages;
        }
    }

    /// <summary />
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
    }
}
