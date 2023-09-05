using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public partial class FluentMessageBar : FluentComponentBase
{
    private Color? _color;

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-messagebar", () => Format == MessageBarFormat.Default)
        .AddClass("fluent-messagebar-notification", () => Format == MessageBarFormat.Notification)
        .AddClass("intent-info", () => Intent == MessageBarIntent.Info)
        .AddClass("intent-warning", () => Intent == MessageBarIntent.Warning)
        .AddClass("intent-error", () => Intent == MessageBarIntent.Error)
        .AddClass("intent-success", () => Intent == MessageBarIntent.Success)
        .AddClass("intent-custom", () => Intent == MessageBarIntent.Custom)
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
    public MessageBarContent Content { get; set; } = MessageBarContent.Empty();

    /// <summary />
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary />
    [Parameter]
    public MessageBarIntent? Intent
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

    /// <summary />
    [Parameter]
    public Icon? Icon
    {
        get
        {
            if (Content.Options.Icon != null && Content.Intent == MessageBarIntent.Custom)
            {
                return Content.Options.Icon;
            }
            else
            {
                return Content.Intent switch
                {
                    MessageBarIntent.Info => new CoreIcons.Filled.Size20.Info(),
                    MessageBarIntent.Warning => new CoreIcons.Filled.Size20.Warning(),
                    MessageBarIntent.Error => new CoreIcons.Filled.Size20.DismissCircle(),
                    MessageBarIntent.Success => new CoreIcons.Filled.Size20.CheckmarkCircle(),
                    _ => null,
                };
            }
        }

        set
        {
            Content.Options.Icon = value;
        }
    }

    /// <summary />
    [Parameter]
    public bool IsVisible { get; set; } = true;

    /// <summary />
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

    /// <summary />
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

    /// <summary>
    /// On app and page level a Message bar should NOT have rounded corners. On component level it should.
    /// </summary>  
    [Parameter]
    public bool RoundedCorners { get; set; } = true;

    /// <summary />
    protected MessageBarAction PrimaryAction => Content.Options.PrimaryAction;

    /// <summary />
    protected MessageBarAction SecondaryAction => Content.Options.SecondaryAction;

    /// <summary />
    protected bool ShowPrimaryAction => !string.IsNullOrEmpty(Content.Options.PrimaryAction.Text);

    /// <summary />
    protected bool ShowSecondaryAction => !string.IsNullOrEmpty(Content.Options.SecondaryAction.Text);

    protected override void OnParametersSet()
    {
        _color = Content.Intent switch
        {
            MessageBarIntent.Info => Color.Info,
            MessageBarIntent.Warning => Color.Warning,
            MessageBarIntent.Error => Color.Error,
            MessageBarIntent.Success => Color.Success,
            _ => IconColor,
        };
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
        IsVisible = false;
        Content.Close();
    }
}
