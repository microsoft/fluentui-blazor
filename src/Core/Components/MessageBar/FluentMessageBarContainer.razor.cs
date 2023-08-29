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

    /// <summary />
    [Inject]
    private IMessageService MessageService { get; set; } = default!;

    /// <summary>
    /// Display only alerts from this category.
    /// </summary>
    [Parameter]
    public string? Category { get; set; }

    /// <summary>
    /// Display the list of alerts (false) or only the number of alerts (true).
    /// </summary>
    [Parameter]
    public bool IsNumberOnly { get; set; } = false;

    /// <summary>
    /// Displays alerts as a single line (with the message only)
    /// or as a card (with the detailed message).
    /// </summary>
    [Parameter]
    public MessageBarFormat Format { get; set; } = MessageBarFormat.Default;

    /// <summary>
    /// Maximum number of alerts displayed. All others are stored in memory to be displayed when an existing alert is closed.
    /// By default, this property is <see cref="MessageBarGlobalOptions.MaxMessageCount" />.
    /// Set a value equal to or less than zero, to display all alerts for this <see cref="Category" /> (or all categories if not set).
    /// </summary>
    [Parameter]
    public int? MaxMessageCount { get; set; }

    /// <summary />
    protected IEnumerable<Message> AllMessagesForThisCategory
    {
        get
        {
            return string.IsNullOrEmpty(Category)
                          ? MessageService.AllMessages
                          : MessageService.AllMessages.Where(x => x.Category == Category);
        }
    }

    /// <summary />
    protected IEnumerable<Message> MessagesShown
    {
        get
        {
            if (MaxMessageCount.HasValue)
            {
                int maxAlerts = MaxMessageCount.Value > 0 ? MaxMessageCount.Value : int.MaxValue;

                return MessageService.Configuration.NewestOnTop
                            ? AllMessagesForThisCategory.Reverse().TakeLast(maxAlerts)
                            : AllMessagesForThisCategory.TakeLast(maxAlerts);
            }
            else
            {
                return MessageService.Configuration.NewestOnTop
                            ? MessageService.MessagesShown(Category).Reverse()
                            : MessageService.MessagesShown(Category);
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
