using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public partial class FluentMessageBar : FluentComponentBase
{
    private Color? _color;

    [Inject] GlobalState GlobalState { get; set; } = default!;

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
    protected string? StyleValue => new StyleBuilder()
        .AddStyle(Style)
        .Build();

    /// <summary>
    /// The type of message bar. Default is MessageType.MessageBar. See <see cref="MessageType"/> for more details.
    /// </summary>
    [Parameter]
    public MessageType Type { get; set; } = MessageType.MessageBar;

    /// <summary>
    /// The actual message instance shown in the message bar.
    /// </summary>
    [Parameter]
    public Message Content { get; set; } = Message.Empty();

    /// <summary>
    /// The message to be shown whennot using the MessageService methods
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Intent of the message bar. Default is MessageIntent.Info. See <see cref="MessageIntent"/> for more details.
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
    /// Icon to show in the message bar based on the intent of the message. See <see cref="Icon"/> for more details.
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
    /// Visibility of the message bar. Default is true.
    /// </summary>
    [Parameter]
    public bool Visible { get; set; } = true;

    /// <summary>
    /// Most important info to be shown in the message bar.
    /// </summary>
    [Parameter]
    public string Title
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
    /// Time on which the message was created. Default is DateTime.Now. Onlu used when MessageType is Notification.
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
    /// The color of the icon. Only applied when intent is MessageBarIntent.Custom
    /// Default is Color.Accent
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

    protected override void OnParametersSet()
    {
        _color = Content.Intent switch
        {
            MessageIntent.Info => Color.Info,
            MessageIntent.Warning => Color.Warning,
            MessageIntent.Error => Color.Error,
            MessageIntent.Success => Color.Success,
            _ => IconColor,
        };
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
}
