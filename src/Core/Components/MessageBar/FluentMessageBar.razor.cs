using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public partial class FluentMessageBar : FluentComponentBase
{
    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("power-alert", () => Format == MessageBarFormat.Default)
        .AddClass("power-alert-notification", () => Format == MessageBarFormat.Notification)
        .AddClass("alert-info", () => Type == MessageType.Info)
        .AddClass("alert-warning", () => Type == MessageType.Warning)
        .AddClass("alert-error", () => Type == MessageType.Error)
        .AddClass("alert-success", () => Type == MessageType.Success)
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder()
        .AddStyle(Style)
        .Build();

    /// <summary />
    [Parameter]
    public MessageBarFormat Format { get; set; } = MessageBarFormat.Default;

    /// <summary />
    [Parameter]
    public Message Message { get; set; } = Message.Empty();

    /// <summary />
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary />
    [Parameter]
    public MessageType? Type
    {
        get
        {
            return Message.Type;
        }

        set
        {
            Message.Options.Type = value;
        }
    }

    /// <summary />
    [Parameter]
    public Icon? Icon
    {
        get
        {
            if (Message.Options.Icon != null)
            {
                return Message.Options.Icon;
            }
            else
            {
                return Format switch
                {
                    MessageBarFormat.Notification => Message.Type switch
                    {
                        MessageType.Info => new CoreIcons.Filled.Size24.Info(),
                        MessageType.Warning => new CoreIcons.Filled.Size24.Warning(),
                        MessageType.Error => new CoreIcons.Filled.Size24.DismissCircle(),
                        MessageType.Success => new CoreIcons.Filled.Size24.CheckmarkCircle(),
                        _ => null,
                    },
                    _ => Message.Type switch
                    {
                        MessageType.Info => new CoreIcons.Regular.Size24.Info(),
                        MessageType.Warning => new CoreIcons.Filled.Size24.Warning(),
                        MessageType.Error => new CoreIcons.Filled.Size24.DismissCircle(),
                        MessageType.Success => new CoreIcons.Filled.Size24.CheckmarkCircle(),
                        _ => null,
                    },
                };
            }
        }

        set
        {
            Message.Options.Icon = value;
        }
    }

    /// <summary />
    [Parameter]
    public bool ShowDismiss
    {
        get
        {
            return Message.Options.ShowDismiss;
        }

        set
        {
            Message.Options.ShowDismiss = value;
        }
    }

    /// <summary />
    [Parameter]
    public bool IsVisible { get; set; } = true;

    /// <summary />
    [Parameter]
    public string Text
    {
        get
        {
            return Message.Text;
        }

        set
        {
            Message.Text = value;
        }
    }

    /// <summary />
    [Parameter]
    public DateTime? Timestamp
    {
        get
        {
            return Message.Options.Timestamp;
        }

        set
        {
            Message.Options.Timestamp = value;
        }
    }

    /// <summary />
    public Icon DismissIcon { get; set; } = new CoreIcons.Regular.Size24.Dismiss();

    /// <summary />
    protected bool ShowIcon => Icon != null;

    /// <summary />
    protected MessageAction Action => Message.Options.Action;

    /// <summary />
    protected bool ShowActionButton => !string.IsNullOrEmpty(Message.Options.Action.Text);

    /// <summary />
    protected Task ActionClickedAsync(MouseEventArgs e)
    {
        if (Action?.OnClick != null)
        {
            return Action.OnClick.Invoke(Message);
        }

        return Task.CompletedTask;
    }

    /// <summary />
    protected void DismissClicked()
    {
        IsVisible = false;
        Message.Close();
    }
}
