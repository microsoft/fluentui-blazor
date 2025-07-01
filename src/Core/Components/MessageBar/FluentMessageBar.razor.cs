// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Component to communicate important information about the state of the entire application or surface
/// </summary>
public partial class FluentMessageBar : FluentComponentBase
{
    private static readonly Icon IconInfo = new CoreIcons.Regular.Size20.Info().WithColor("var(--info)");
    private static readonly Icon IconWarning= new CoreIcons.Filled.Size20.Warning().WithColor("var(--warning)");
    private static readonly Icon IconSuccess = new CoreIcons.Filled.Size20.CheckmarkCircle().WithColor("var(--success)");
    private static readonly Icon IconError = new CoreIcons.Filled.Size20.DismissCircle().WithColor("var(--error)");

    /// <summary />
    public FluentMessageBar(LibraryConfiguration configuration) : base(configuration) { }

    /// <summary />
    protected virtual string? ClassValue => DefaultClassBuilder
         .Build();

    /// <summary />
    protected virtual string? StyleValue => DefaultStyleBuilder
        .Build();

    /// <summary>
    /// Gets or sets the intent of the message bar. 
    /// Default is <see cref="MessageBarIntent.Info"/>.
    /// </summary>
    [Parameter]
    public MessageBarIntent? Intent { get; set; }

    /// <summary>
    /// Gets or sets the layout of the message bar. 
    /// Default is <see cref="MessageBarLayout.SingleLine"/>.
    /// </summary>
    [Parameter]
    public MessageBarLayout? Layout { get; set; }

    /// <summary>
    /// Gets or sets the shape of the message bar.
    /// Default is <see cref="MessageBarShape.Rounded"/>.
    /// </summary>
    [Parameter]
    public MessageBarShape? Shape { get; set; }

    /// <summary>
    /// Gets or sets the fade in animation when the message bar is shown.
    /// Default is none.
    /// </summary>
    [Parameter]
    public MessageBarAnimation? Animation { get; set; }

    /// <summary>
    /// Gets or sets the `aria-live` attribute, to inform assistive technologies (like screen readers) about updates to dynamic content.
    /// </summary>
    [Parameter]
    public AriaLive? AriaLive { get; set; }

    /// <summary>
    /// Gets or sets the icon to show in the message bar based on the intent of the message.
    /// </summary>
    [Parameter]
    public Icon? Icon { get; set; }

    /// <summary>
    /// Gets or sets the most important info to be shown in the message bar.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the visibility of the message bar. Default is true.
    /// </summary>
    [Parameter]
    public bool Visible { get; set; } = true;

    /// <summary>
    /// Gets or sets the ability to dismiss the message bar. Default is true.
    /// </summary>
    [Parameter]
    public bool AllowDismiss { get; set; } = true;

    /// <summary>
    /// Gets or sets the message to be shown when not using the MessageService methods.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the content to be displayed inline after the main content.
    /// </summary>
    [Parameter]
    public RenderFragment? ActionsTemplate { get; set; }

    /// <summary>
    /// Gets or sets the time on which the message was created.
    /// Only used when <see cref="ActionsTemplate"/> is not used: `null` (if not, this parameter is ignored).
    /// </summary>
    [Parameter]
    public DateTime? TimeStamp { get; set; }

    /// <summary />
    protected virtual Task DismissClickAsync()
    {
        Visible = false;
        return Task.CompletedTask;
    }

    /// <summary />
    private string? GetIntentString()
    {
        if (Intent == null || Intent == MessageBarIntent.Custom)
        {
            return null;
        }

        return Intent.ToAttributeValue();
    }

    /// <summary />
    private Icon GetIcon()
    {
        if (Icon is null)
        {
            return Intent switch
            {
                MessageBarIntent.Error => IconError,
                MessageBarIntent.Warning => IconWarning,
                MessageBarIntent.Success => IconSuccess,
                MessageBarIntent.Info => IconInfo,
                _ => IconInfo,
            };
        }

        return Icon;
    }

    /// <summary />
    private string? GetAnimation()
    {
        return Animation switch
        {
            MessageBarAnimation.FadeIn => "fade-in",
            _ => null,
        };
    }

    /// <summary />
    private string? GetTimeStamp()
    {
        if (TimeStamp is null)
        {
            return null;
        }

        var delay = DateTimeProvider.Now - TimeStamp.Value;
        return delay.ToTimeAgo(Localizer);
    }
}
