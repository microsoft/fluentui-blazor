// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

#pragma warning disable CS1591, MA0051, MA0123, CA1822

public partial class FluentToast
{
    /// <summary />
    public FluentToast(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    [Parameter]
    public IToastInstance? Instance { get; set; }

    [Parameter]
    public bool Opened { get; set; }

    [Parameter]
    public EventCallback<bool> OpenedChanged { get; set; }

    [Parameter]
    public int Timeout { get; set; } = 7000;

    [Parameter]
    public ToastPosition? Position { get; set; }

    [Parameter]
    public int VerticalOffset { get; set; } = 16;

    [Parameter]
    public int HorizontalOffset { get; set; } = 20;

    [Parameter]
    public ToastIntent Intent { get; set; } = ToastIntent.Info;

    [Parameter]
    public ToastPoliteness? Politeness { get; set; }

    [Parameter]
    public bool PauseOnHover { get; set; }

    [Parameter]
    public bool PauseOnWindowBlur { get; set; }

    [Parameter]
    public EventCallback<bool> OnToggle { get; set; }

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
    /// Gets or sets whether to render the dismiss button.
    /// </summary>
    [Parameter]
    public bool ShowDismissButton { get; set; } = true;

    [Parameter]
    public string? Status { get; set; }

    [Parameter]
    public bool ShowProgress { get; set; }

    [Parameter]
    public bool Indeterminate { get; set; } = true;

    [Parameter]
    public int? Value { get; set; }

    [Parameter]
    public int? Max { get; set; } = 100;

    [Parameter]
    public RenderFragment? Media { get; set; }

    [Parameter]
    public RenderFragment? TitleContent { get; set; }

    [Parameter]
    public RenderFragment? SubtitleContent { get; set; }

    [Parameter]
    public RenderFragment? FooterContent { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public RenderFragment? DismissContent { get; set; }

    internal Icon DismissIcon => new CoreIcons.Regular.Size20.Dismiss();

    internal Icon IntentIcon => Intent switch
    {
        ToastIntent.Success => new CoreIcons.Filled.Size20.CheckmarkCircle(),
        ToastIntent.Warning => new CoreIcons.Filled.Size20.Warning(),
        ToastIntent.Error => new CoreIcons.Filled.Size20.DismissCircle(),
        _ => new CoreIcons.Filled.Size20.Info(),
    };

    internal string? ClassValue => DefaultClassBuilder.Build();

    internal string? StyleValue => DefaultStyleBuilder.Build();

    [Inject]
    private IToastService? ToastService { get; set; }

    public Task<ToastEventArgs> RaiseOnStatusChangeAsync(DialogToggleEventArgs args)
        => RaiseOnStatusChangeAsync(new ToastEventArgs(this, args));

    public Task<ToastEventArgs> RaiseOnStatusChangeAsync(IToastInstance instance, ToastStatus status)
        => RaiseOnStatusChangeAsync(new ToastEventArgs(instance, status));

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

    internal Task OnQuickAction1ClickedAsync()
        => HandleActionClickedAsync(Instance?.Options.QuickAction1Callback);

    internal Task OnQuickAction2ClickedAsync()
        => HandleActionClickedAsync(Instance?.Options.QuickAction2Callback);

    private async Task HandleActionClickedAsync(Func<Task>? callback)
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
        if (toastEventArgs.Status == ToastStatus.Dismissed)
        {
            toastInstance.Status = ToastStatus.Dismissed;
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
            toastInstance.Status = ToastStatus.Unmounted;

            if (ToastService is ToastService toastService)
            {
                await toastService.RemoveToastFromProviderAsync(Instance);
            }

            await RaiseOnStatusChangeAsync(toastInstance, ToastStatus.Unmounted);
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

#pragma warning restore CS1591, MA0051, MA0123, CA1822
