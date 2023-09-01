using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public partial class FluentMessageBarContainer : FluentComponentBase, IDisposable
{
    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder()
        .AddStyle(Style)
        .Build();

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
    public MessageBarFormat Format { get; set; } = MessageBarFormat.Default;

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
    protected IEnumerable<MessageBarContent> AllMessagesForCategory
    {
        get
        {
            return string.IsNullOrEmpty(Section)
                          ? MessageService.AllMessages
                          : MessageService.AllMessages.Where(x => x.Section == Section);
        }
    }

    /// <summary />
    protected IEnumerable<MessageBarContent> MessagesShown
    {
        get
        {
            if (MaxMessageCount.HasValue)
            {
                int maxMessages = MaxMessageCount.Value > 0 ? MaxMessageCount.Value : int.MaxValue;

                return NewestOnTop
                            ? AllMessagesForCategory.Reverse().TakeLast(maxMessages)
                            : AllMessagesForCategory.TakeLast(maxMessages);
            }
            else
            {
                return NewestOnTop
                            ? MessageService.MessagesShown(-1, Section).Reverse()
                            : MessageService.MessagesShown(-1, Section);
            }
        }
    }

    /// <summary />
    protected override void OnInitialized()
    {
        MessageService.OnMessageItemsUpdated += OnAlertUpdateHandler;
        base.OnInitialized();
    }

    /// <summary />
    protected virtual void OnAlertUpdateHandler()
    {
        InvokeAsync(StateHasChanged); //.SafeFireAndForget();
    }

    /// <summary />
    public void Dispose()
    {
        MessageService.OnMessageItemsUpdated -= OnAlertUpdateHandler;
    }
}
