// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The FluentToast component represents a transient message that appears on the screen to provide feedback or
/// information to the user. It is typically used for displaying notifications, alerts, or status messages in a
/// non-intrusive manner. The FluentToast component can be customized with various options such as position, intent,
/// timeout duration, and actions, allowing developers to create engaging and informative user experiences.
/// </summary>
public partial class FluentToast : FluentComponentBase
{
    /// <summary />
    public FluentToast(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    [Inject]
    private INotificationService? NotificationService { get; set; } = default!;

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder.Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder.Build();

    /// <summary>
    /// Gets the instance, if the toast is rendered using the <see cref="INotificationService"/>. Otherwise, returns null.
    /// </summary>
    [CascadingParameter]
    internal IToastInstance? ToastInstance { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the component is currently open.
    /// </summary>
    [Parameter]
    public bool Opened { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the open state changes.
    /// </summary>
    /// <remarks>
    /// Use this event to respond to changes in the component's open or closed state. The callback receives a value
    /// indicating the new open state: <see langword="true"/> if the component is open; otherwise,
    /// <see langword="false"/>.
    /// </remarks>
    [Parameter]
    public EventCallback<bool> OpenedChanged { get; set; }

    /// <summary>
    /// Gets or sets the lifetime of the toast.
    /// When set to a positive value, the toast is automatically dismissed after this duration elapses, 
    /// triggering the appropriate lifecycle events.
    /// When `null`, the toast stays visible until it is dismissed programmatically or by the user.
    /// </summary>
    [Parameter]
    public TimeSpan? Lifetime { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="ToastPosition"/> on the screen where the toast notification is displayed.
    /// </summary>
    [Parameter]
    public ToastPosition? Position { get; set; }

    /// <summary>
    /// Gets or sets the vertical offset, in pixels, applied to the component's position.
    /// </summary>
    [Parameter]
    public int VerticalOffset { get; set; } = 16;

    /// <summary>
    /// Gets or sets the horizontal offset, in pixels, applied to the component's content.
    /// </summary>
    [Parameter]
    public int HorizontalOffset { get; set; } = 20;

    /// <summary>
    /// Gets or sets a value indicating whether the toast uses inverted colors.
    /// </summary>
    [Parameter]
    public bool Inverted { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="ToastIntent"/> intent of the toast notification, indicating its purpose or severity.
    /// </summary>
    /// <remarks>
    /// The intent determines the visual styling and icon used for the toast notification. Common intents include
    /// informational, success, warning, and error. Setting the appropriate intent helps users quickly understand the
    /// nature of the message.
    /// </remarks>
    [Parameter]
    public ToastIntent? Intent { get; set; }

    /// <summary>
    /// Gets or sets the level of notification politeness for assistive technologies.
    /// </summary>
    /// <remarks>
    /// Use this property to control how screen readers announce the toast notification. Setting an appropriate
    /// politeness level can help ensure that important messages are delivered to users without unnecessary
    /// interruption.
    /// </remarks>
    [Parameter]
    public ToastPoliteness? Politeness { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="Lifetime"/> pauses when the user hovers over the component.
    /// </summary>
    [Parameter]
    public bool PauseOnHover { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="Lifetime"/> pauses when the browser window loses focus.
    /// </summary>
    [Parameter]
    public bool PauseOnWindowBlur { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the toast status changes.
    /// </summary>
    /// <remarks>
    /// Use this property to handle status updates for the toast component, such as when it is shown, hidden, or
    /// dismissed. The callback receives a <see cref="ToastEventArgs"/> instance containing details about the status
    /// change.
    /// </remarks>
    [Parameter]
    public EventCallback<ToastEventArgs> OnStatusChange { get; set; }

    /// <summary>
    /// Gets or sets the icon rendered in the toast header.
    /// When set, this overrides the default icon determined by the <see cref="Intent" />
    /// (Warning, Error, Success, Info) of the toast.
    /// </summary>
    [Parameter]
    public Icon? Icon { get; set; }

    /// <summary>
    /// Gets or sets the title displayed in the toast header.
    /// For security reasons, the content is sanitized using the configured <see cref="LibraryConfiguration.MarkupSanitized"/> before rendering.
    /// For formatted content with markup, use <see cref="ChildContent"/> instead.    
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the subtitle displayed in the toast, below the <see cref="ChildContent"/>.
    /// </summary>
    [Parameter]
    public string? Subtitle { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the toast can be dismissed by the user. Default is <see langword="true"/>.
    /// When <see langword="true"/>, a dismiss button is rendered;
    /// Use <see cref="DismissAction"/> to customize its label.
    /// </summary>
    [Parameter]
    public bool AllowDismiss { get; set; } = true;

    /// <summary>
    /// Gets or sets the label for the dismiss action button (e.g., `DismissAction="Close"`).
    /// Only relevant when <see cref="AllowDismiss"/> is <see langword="true"/>.
    /// </summary>
    [Parameter]
    public string? DismissAction { get; set; }

    /// <summary>
    /// Gets or sets the content rendered in the toast body.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the content rendered in the toast footer section, typically used for displaying additional information or actions.
    /// </summary>
    [Parameter]
    public RenderFragment? FooterTemplate { get; set; }

    /// <summary />
    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && ToastInstance is ToastInstance instance)
        {
            if (!Opened)
            {
                Opened = true;
                return InvokeAsync(StateHasChanged);
            }
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Handles the toggle event for the toast component.
    /// </summary>
    /// <param name="args">The event data associated with the dialog toggle action.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnToggleAsync(DialogToggleEventArgs args)
    {
        // Ensure that the event is for the current toast instance by comparing the IDs.
        var expectedId = ToastInstance?.Id ?? Id;
        if (string.CompareOrdinal(args.Id, expectedId) != 0)
        {
            return;
        }

        var toast = ToastInstance as ToastInstance;
        var state = DialogEventArgs.GetDialogState(args.Type, args.OldState, args.NewState);

        // If the toast state is either Open or Closed,
        // update the Opened property
        // and invoke the OpenedChanged callback if necessary.
        if (state == DialogState.Open || state == DialogState.Closed)
        {
            var isOpen = state == DialogState.Open;

            if (Opened != isOpen)
            {
                Opened = isOpen;

                if (OpenedChanged.HasDelegate)
                {
                    await OpenedChanged.InvokeAsync(isOpen);
                }
            }
        }

        if (toast is not null && state == DialogState.Open)
        {
            toast.SetStatus(ToastLifecycleStatus.Visible);
        }

        // If the toast instance is defined and the toast state is Closed,
        // set the result of the ResultCompletion task
        if (toast is not null && state == DialogState.Closed)
        {
            toast.SetStatus(ToastLifecycleStatus.Dismissed);

            if (NotificationService is NotificationService notificationService)
            {
                await notificationService.RemoveToastFromProviderAsync(toast);
            }

            // Set the result of the toast to TimedOut.
            toast.ResultCompletion.TrySetResult(ToastResult.OfTimedOut());
        }
    }

    /// <summary>
    /// Determines the appropriate icon to display based on the current <see cref="Intent"/> of the toast.
    /// If the <see cref="Intent"/> is not set or is <see cref="ToastIntent.Progress"/>, no icon is displayed.
    /// </summary>
    /// <returns>The icon to display, or null if no icon should be displayed.</returns>
    protected virtual Icon? GetIntentIcon()
    {
        if (Intent is null || Intent == ToastIntent.Progress)
        {
            return null;
        }

        var iconColor = Intent switch
        {
            ToastIntent.Success => Inverted ? Color.SuccessInverted : Color.Success,
            ToastIntent.Warning => Inverted ? Color.WarningInverted : Color.Warning,
            ToastIntent.Error => Inverted ? Color.ErrorInverted : Color.Error,
            _ => Inverted ? Color.InfoInverted : Color.Info,
        };

        return Intent switch
        {
            ToastIntent.Success => new CoreIcons.Filled.Size20.CheckmarkCircle().WithColor(iconColor),
            ToastIntent.Warning => new CoreIcons.Filled.Size20.Warning().WithColor(iconColor),
            ToastIntent.Error => new CoreIcons.Filled.Size20.DismissCircle().WithColor(iconColor),
            _ => new CoreIcons.Filled.Size20.Info().WithColor(iconColor),
        };
    }

    /// <summary>
    /// Closes the toast component.
    /// </summary>
    private Task CloseAsync()
    {
        if (!Opened)
        {
            return Task.CompletedTask;
        }

        Opened = false;
        return InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Handles the ToastAction click event, dismissing the toast.
    /// </summary>
    private async Task DismissClickAsync()
    {
        if (ToastInstance is null)
        {
            await CloseAsync();
            return;
        }

        await ToastInstance.CloseAsync(ToastResult.OfDismissed());
    }
}
