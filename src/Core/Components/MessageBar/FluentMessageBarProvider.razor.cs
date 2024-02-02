using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentMessageBarProvider : FluentComponentBase, IDisposable
{
    /// <summary />
    protected string? ClassValue => new CssBuilder(Class).Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder(Style).Build();

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
    protected IEnumerable<Message> AllMessagesForCategory
    {
        get
        {
            return string.IsNullOrEmpty(Section)
                          ? MessageService.AllMessages
                          : MessageService.AllMessages.Where(x => x.Section == Section);
        }
    }

    /// <summary />
    protected IEnumerable<Message> MessagesToShow
    {
        get
        {
            if (MaxMessageCount.HasValue)
            {
                var maxMessages = MaxMessageCount.Value > 0 ? MaxMessageCount.Value : int.MaxValue;

                return NewestOnTop
                            ? AllMessagesForCategory.Reverse().TakeLast(maxMessages)
                            : AllMessagesForCategory.TakeLast(maxMessages);
            }
            else
            {
                return NewestOnTop
                            ? MessageService.MessagesToShow(-1, Section).Reverse()
                            : MessageService.MessagesToShow(-1, Section);
            }
        }
    }

    /// <summary />
    protected override void OnInitialized()
    {
        MessageService.OnMessageItemsUpdated += OnMessageItemsUpdatedHandler;
        MessageService.OnMessageItemsUpdatedAsync += OnMessageItemsUpdatedHandlerAsync;
        base.OnInitialized();
    }

    /// <summary />
    protected virtual void OnMessageItemsUpdatedHandler()
    {
        InvokeAsync(StateHasChanged);
    }

    protected async virtual Task OnMessageItemsUpdatedHandlerAsync()
    {
        await Task.Run(() =>
        {
            InvokeAsync(StateHasChanged);
        });
    }

    /// <summary />
    public void Dispose()
    {
        MessageService.OnMessageItemsUpdated -= OnMessageItemsUpdatedHandler;
        MessageService.OnMessageItemsUpdatedAsync -= OnMessageItemsUpdatedHandlerAsync;
    }
}
