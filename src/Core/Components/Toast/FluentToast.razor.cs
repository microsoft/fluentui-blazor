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
public partial class FluentToast
{
    /// <summary />
    public FluentToast(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    [Inject]
    private IToastService ToastService { get; set; } = default!;

    /// <summary>
    /// Gets or sets the toast instance associated with this component.
    /// </summary>
    [Parameter]
    public IToastInstance? Instance { get; set; }

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
    /// Gets or sets the duration in milliseconds before the toast automatically closes. Set this to less or equal to 0
    /// to disable automatic closing.
    /// </summary>
    [Parameter]
    public int Timeout { get; set; } = 7000;

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
    /// Gets or sets the type of toast notification to display.
    /// </summary>
    [Parameter]
    public ToastType Type { get; set; } = ToastType.Communication;

    /// <summary>
    /// Gets or sets the <see cref="ToastIntent"/> intent of the toast notification, indicating its purpose or severity.
    /// </summary>
    /// <remarks>
    /// The intent determines the visual styling and icon used for the toast notification. Common intents include
    /// informational, success, warning, and error. Setting the appropriate intent helps users quickly understand the
    /// nature of the message.
    /// </remarks>
    [Parameter]
    public ToastIntent Intent { get; set; } = ToastIntent.Info;

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
    /// Gets or sets a value indicating whether the timeout countdown pauses when the user hovers over the component.
    /// </summary>
    [Parameter]
    public bool PauseOnHover { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the timeout countdown is paused when the browser window loses focus.
    /// </summary>
    [Parameter]
    public bool PauseOnWindowBlur { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the toggle state changes.
    /// </summary>
    /// <remarks>
    /// The callback receives a Boolean value indicating the new state of the toggle. Use this parameter to handle
    /// toggle events in the parent component.
    /// </remarks>
    [Parameter]
    public EventCallback<bool> OnToggle { get; set; }

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
    /// Gets or sets the title displayed in the toast.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the body content displayed in the toast.
    /// </summary>
    [Parameter]
    public string? Body { get; set; }

    /// <summary>
    /// Gets or sets the optional subtitle displayed below the body.
    /// </summary>
    [Parameter]
    public string? Subtitle { get; set; }

    /// <summary>
    /// Gets or sets the first quick action label.
    /// </summary>
    [Parameter]
    public string? QuickAction1 { get; set; }

    /// <summary>
    /// Gets or sets the second quick action label.
    /// </summary>
    [Parameter]
    public string? QuickAction2 { get; set; }

    /// <summary>
    /// Gets or sets whether the toast is dismissable by the user.
    /// </summary>
    [Parameter]
    public bool IsDismissable { get; set; }

    /// <summary>
    /// Gets or sets the dismiss action label
    /// </summary>
    [Parameter]
    public string? DismissAction { get; set; }

    /// <summary>
    /// Gets or sets the icon rendered in the media slot of the toast.
    /// </summary>
    [Parameter]
    public Icon? Icon { get; set; }

    /// <summary>
    /// Gets or sets custom content rendered in the toast body, such as progress content managed through
    /// <see cref="IToastInstance.UpdateAsync(Action{ToastOptions})"/>.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    //
    internal static Icon DismissIcon => new CoreIcons.Regular.Size20.Dismiss();

    internal Icon IntentIcon => Intent switch
    {
        ToastIntent.Success => new CoreIcons.Filled.Size20.CheckmarkCircle(),
        ToastIntent.Warning => new CoreIcons.Filled.Size20.Warning(),
        ToastIntent.Error => new CoreIcons.Filled.Size20.DismissCircle(),
        _ => new CoreIcons.Filled.Size20.Info(),
    };

    internal string? ClassValue => DefaultClassBuilder.Build();

    internal string? StyleValue => DefaultStyleBuilder.Build();

    /// <summary>
    /// Raises the status change event asynchronously using the specified dialog toggle event arguments.
    /// </summary>
    /// <param name="args">The event data associated with the dialog toggle action. Cannot be null.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the event arguments for the toast
    /// status change.
    /// </returns>
    public Task<ToastEventArgs> RaiseOnStatusChangeAsync(DialogToggleEventArgs args)
        => RaiseOnStatusChangeAsync(new ToastEventArgs(this, args));

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

    internal async Task OnDismissActionClickedAsync()
    {
        await Instance!.CloseAsync(ToastCloseReason.Dismissed);

        if (Instance?.Options.DismissActionCallback is not null)
        {
            await Instance.Options.DismissActionCallback();
        }
    }

    internal Task OnQuickAction1ClickedAsync()
        => HandleQuickActionClickedAsync(Instance?.Options.QuickAction1Callback);

    internal Task OnQuickAction2ClickedAsync()
        => HandleQuickActionClickedAsync(Instance?.Options.QuickAction2Callback);

    private async Task HandleQuickActionClickedAsync(Func<Task>? callback)
    {
        await Instance!.CloseAsync(ToastCloseReason.QuickAction);

        if (callback is not null)
        {
            await callback();
        }
    }

    internal static Color GetIntentColor(ToastIntent intent)
    {
        return intent switch
        {
            ToastIntent.Success => Color.Success,
            ToastIntent.Warning => Color.Warning,
            ToastIntent.Error => Color.Error,
            _ => Color.Info,
        };
    }

    /// <inheritdoc />
    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && Instance is ToastInstance instance)
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
        var expectedId = Instance?.Id ?? Id;
        if (string.CompareOrdinal(args.Id, expectedId) != 0)
        {
            return;
        }

        if (Instance is not ToastInstance toastInstance)
        {
            return;
        }

        var toastEventArgs = new ToastEventArgs(this, args);
        if (toastEventArgs.Status == ToastLifecycleStatus.Dismissed)
        {
            toastInstance.LifecycleStatus = ToastLifecycleStatus.Dismissed;
            await RaiseOnStatusChangeAsync(toastEventArgs);
        }

        var toggled = string.Equals(args.NewState, "open", StringComparison.OrdinalIgnoreCase);
        if (Opened != toggled)
        {
            Opened = toggled;

            if (OnToggle.HasDelegate)
            {
                await OnToggle.InvokeAsync(toggled);
            }

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
                await toastService.RemoveToastFromProviderAsync(Instance);
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
