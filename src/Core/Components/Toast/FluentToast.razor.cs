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
    internal static readonly Icon DismissIcon = new CoreIcons.Regular.Size20.Dismiss();

    /// <summary />
    public FluentToast(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    [Inject]
    private IToastService ToastService { get; set; } = default!;

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder.Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder.Build();

    /// <summary>
    /// Gets the instance, if the toast is rendered using the <see cref="IToastService"/>. Otherwise, returns null.
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
    /// When set, this overrides the default icon determined by the <see cref="Intent"/>.
    /// </summary>
    [Parameter]
    public Icon? Icon { get; set; }

    /// <summary>
    /// Gets or sets the title displayed in the toast.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the toast can be dismissed by the user. Default is <see langword="true"/>.
    /// When <see langword="true"/>, a dismiss button is rendered; use <see cref="DismissAction"/> to customize its label.
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

    /// <summary />
    private Icon GetTitleIcon()
    {
        if (Icon is not null)
        {
            return Icon;
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
    /// Raises the status change event asynchronously using the specified dialog toggle event arguments.
    /// </summary>
    /// <param name="args">The event data associated with the dialog toggle action. Cannot be null.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the event arguments for the toast
    /// status change.
    /// </returns>
    public Task<ToastEventArgs> RaiseOnStatusChangeAsync(DialogToggleEventArgs args)
        => RaiseOnStatusChangeAsync(new ToastEventArgs(this.ToastInstance, args));

    /// <summary>
    /// Raises the status change event for the specified toast instance asynchronously.
    /// </summary>
    /// <param name="instance">
    /// The toast instance for which the status change event is being raised. Cannot be null.
    /// </param>
    /// <param name="status">The new status to associate with the toast instance.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the event arguments for the status
    /// change.
    /// </returns>
    public Task<ToastEventArgs> RaiseOnStatusChangeAsync(IToastInstance instance, ToastLifecycleStatus status)
        => RaiseOnStatusChangeAsync(new ToastEventArgs(instance, status));

    /// <summary>
    /// Raises the toggle event asynchronously using the specified dialog toggle event arguments.
    /// </summary>
    /// <param name="args">The event data associated with the dialog toggle action. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task OnToggleAsync(DialogToggleEventArgs args)
        => HandleToggleAsync(args);

    internal Task RequestCloseAsync()
    {
        if (!Opened)
        {
            return Task.CompletedTask;
        }

        Opened = false;
        return InvokeAsync(StateHasChanged);
    }

    internal async Task DismissClickAsync()
    {
        await ToastInstance!.DismissAsync();

        if (ToastInstance?.Options.DismissActionCallback is not null)
        {
            await ToastInstance.Options.DismissActionCallback();
        }
    }

    internal Task OnQuickAction1ClickedAsync()
        => HandleQuickActionClickedAsync(ToastInstance?.Options.QuickAction1Callback);

    internal Task OnQuickAction2ClickedAsync()
        => HandleQuickActionClickedAsync(ToastInstance?.Options.QuickAction2Callback);

    private async Task HandleQuickActionClickedAsync(Func<Task>? callback)
    {
        await ToastInstance!.CloseAsync(ToastCloseReason.QuickAction);

        if (callback is not null)
        {
            await callback();
        }
    }

    /// <inheritdoc />
    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && ToastInstance is ToastInstance instance)
        {
            instance.FluentToast = this;

            if (!Opened)
            {
                Opened = true;
                return InvokeAsync(StateHasChanged);
            }
        }

        return Task.CompletedTask;
    }

    private async Task HandleToggleAsync(DialogToggleEventArgs args)
    {
        var expectedId = ToastInstance?.Id ?? Id;
        if (string.CompareOrdinal(args.Id, expectedId) != 0)
        {
            return;
        }

        if (ToastInstance is not ToastInstance toastInstance)
        {
            return;
        }

        var toastEventArgs = new ToastEventArgs(this.ToastInstance, args);
        if (toastEventArgs.Status == ToastLifecycleStatus.Dismissed)
        {
            toastInstance.LifecycleStatus = ToastLifecycleStatus.Dismissed;
            await RaiseOnStatusChangeAsync(toastEventArgs);
        }

        var toggled = string.Equals(args.NewState, "open", StringComparison.OrdinalIgnoreCase);
        if (Opened != toggled)
        {
            Opened = toggled;

            if (OpenedChanged.HasDelegate)
            {
                await OpenedChanged.InvokeAsync(toggled);
            }
        }

        if (string.Equals(args.Type, "toggle", StringComparison.OrdinalIgnoreCase)
            && string.Equals(args.NewState, "closed", StringComparison.OrdinalIgnoreCase))
        {
            toastInstance.ResultCompletion.TrySetResult(toastInstance.PendingCloseReason ?? ToastCloseReason.TimedOut);
            toastInstance.PendingCloseReason = null;
            toastInstance.LifecycleStatus = ToastLifecycleStatus.Unmounted;

            if (ToastService is ToastService toastService)
            {
                await toastService.RemoveToastFromProviderAsync(ToastInstance);
            }

            await RaiseOnStatusChangeAsync(toastInstance, ToastLifecycleStatus.Unmounted);
        }
    }

    private async Task<ToastEventArgs> RaiseOnStatusChangeAsync(ToastEventArgs args)
    {
        if (OnStatusChange.HasDelegate)
        {
            await InvokeAsync(() => OnStatusChange.InvokeAsync(args));
        }

        return args;
    }
}
