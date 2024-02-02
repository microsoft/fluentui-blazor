using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentMessageBar : FluentComponentBase, IDisposable
{
    private CountdownTimer? _countdownTimer;
    private Color? _color;

    [Inject] private GlobalState GlobalState { get; set; } = default!;

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-messagebar", () => Type == MessageType.MessageBar)
        .AddClass("dark", () => GlobalState.Luminance == StandardLuminance.DarkMode)
        .AddClass("fluent-messagebar-notification", () => Type == MessageType.Notification)
        .AddClass("intent-info", () => Intent == MessageIntent.Info)
        .AddClass("intent-warning", () => Intent == MessageIntent.Warning)
        .AddClass("intent-error", () => Intent == MessageIntent.Error)
        .AddClass("intent-success", () => Intent == MessageIntent.Success)
        .AddClass("intent-custom", () => Intent == MessageIntent.Custom)
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder(Style).Build();

    /// <summary>
    /// Gets or sets the type of message bar. 
    /// Default is MessageType.MessageBar. See <see cref="MessageType"/> for more details.
    /// </summary>
    [Parameter]
    public MessageType Type { get; set; } = MessageType.MessageBar;

    /// <summary>
    /// Gets or sets the actual message instance shown in the message bar.
    /// </summary>
    [Parameter]
    public Message Content { get; set; } = Message.Empty();

    /// <summary>
    /// Gets or sets the message to be shown when not using the MessageService methods.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the intent of the message bar. 
    /// Default is MessageIntent.Info. See <see cref="MessageIntent"/> for more details.
    /// </summary>
    [Parameter]
    public MessageIntent? Intent
    {
        get
        {
            return Content.Intent;
        }

        set
        {
            Content.Options.Intent = value;
        }
    }

    /// <summary>
    /// Gets or sets the icon to show in the message bar based on the intent of the message. See <see cref="Icon"/> for more details.
    /// </summary>
    [Parameter]
    public Icon? Icon
    {
        get
        {
            if (Content.Options.Icon != null && Content.Intent == MessageIntent.Custom)
            {
                return Content.Options.Icon;
            }
            else
            {
                return Content.Intent switch
                {
                    MessageIntent.Info => new CoreIcons.Filled.Size20.Info(),
                    MessageIntent.Warning => new CoreIcons.Filled.Size20.Warning(),
                    MessageIntent.Error => new CoreIcons.Filled.Size20.DismissCircle(),
                    MessageIntent.Success => new CoreIcons.Filled.Size20.CheckmarkCircle(),
                    _ => null,
                };
            }
        }

        set
        {
            Content.Options.Icon = value;
        }
    }

    /// <summary>
    /// Gets or sets the visibility of the message bar. 
    /// Default is true.
    /// </summary>
    [Parameter]
    public bool Visible { get; set; } = true;

    /// <summary>
    /// Gets or sets the title. 
    /// Most important info to be shown in the message bar.
    /// </summary>
    [Parameter]
    public string? Title
    {
        get
        {
            return Content.Title;
        }

        set
        {
            Content.Title = value;
        }
    }

    /// <summary>
    /// Gets or sets the time on which the message was created. 
    /// Default is DateTime.Now. 
    /// Only used when MessageType is Notification.
    /// </summary>
    [Parameter]
    public DateTime? Timestamp
    {
        get
        {
            return Content.Options.Timestamp;
        }

        set
        {
            Content.Options.Timestamp = value;
        }
    }

    /// <summary>
    /// Gets or sets the color of the icon. 
    /// Only applied when intent is MessageBarIntent.Custom.
    /// Default is Color.Accent.
    /// </summary>
    [Parameter]
    public Color? IconColor { get; set; } = Color.Accent;

    ///// <summary>
    ///// On app and page level a Message bar should NOT have rounded corners. On component level it should.
    ///// </summary>  
    //[Parameter]
    //public bool RoundedCorners { get; set; } = true;

    /// <summary>
    /// A link can be shown after the message. 
    /// </summary>
    protected ActionLink<Message>? Link => Content.Options.Link;

    /// <summary>
    /// Button to show as primary action.
    /// </summary>
    protected ActionButton<Message>? PrimaryAction => Content.Options.PrimaryAction;

    /// <summary>
    /// Button to show as secondary action.
    /// </summary>
    protected ActionButton<Message>? SecondaryAction => Content.Options.SecondaryAction;

    /// <summary />
    protected bool ShowPrimaryAction => !string.IsNullOrEmpty(Content.Options.PrimaryAction?.Text);

    /// <summary />
    protected bool ShowSecondaryAction => !string.IsNullOrEmpty(Content.Options.SecondaryAction?.Text);

    protected override void OnInitialized()
    {
        GlobalState.OnChange += StateHasChanged;
    }

    protected override async Task OnParametersSetAsync()
    {
        _color = Content.Intent switch
        {
            MessageIntent.Info => Color.Info,
            MessageIntent.Warning => Color.Warning,
            MessageIntent.Error => Color.Error,
            MessageIntent.Success => Color.Success,
            _ => IconColor,
        };

        if (Content.Options.Timeout.HasValue)
        {
            if (Content.Options.Timeout == 0)
            {
                return;
            }
            else
            {
                _countdownTimer = new CountdownTimer(Content.Options.Timeout.Value).OnElapsed(DismissClicked);
                await _countdownTimer!.StartAsync();
            }
        }
    }

    /// <summary />
    protected Task LinkClickedAsync(MouseEventArgs e)
    {
        if (Link?.OnClick != null)
        {
            return Link.OnClick.Invoke(Content);
        }

        return Task.CompletedTask;
    }

    /// <summary />
    protected Task PrimaryActionClickedAsync(MouseEventArgs e)
    {
        if (PrimaryAction?.OnClick != null)
        {
            return PrimaryAction.OnClick.Invoke(Content);
        }

        return Task.CompletedTask;
    }

    protected Task SecondaryActionClickedAsync(MouseEventArgs e)
    {
        if (SecondaryAction?.OnClick != null)
        {
            return SecondaryAction.OnClick.Invoke(Content);
        }

        return Task.CompletedTask;
    }

    /// <summary />
    protected void DismissClicked()
    {
        Visible = false;
        Content.Close();
    }

    protected void PauseTimeout()
    {
        Console.WriteLine("PauseTimeout");
        _countdownTimer?.Pause();
    }

    protected void ResumeTimeout()
    {
        Console.WriteLine("ResumeTimeout");
        _countdownTimer?.Resume();
    }

    public void Dispose()
    {
        _countdownTimer?.Dispose();
        _countdownTimer = null;
    }
}
