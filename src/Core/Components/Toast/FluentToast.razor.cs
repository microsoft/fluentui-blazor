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
    public EventCallback<ToastEventArgs> OnStateChange { get; set; }

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

    public Task<ToastEventArgs> RaiseOnStateChangeAsync(DialogToggleEventArgs args)
        => RaiseOnStateChangeAsync(new ToastEventArgs(this, args));

    public Task<ToastEventArgs> RaiseOnStateChangeAsync(IToastInstance instance, DialogState state)
        => RaiseOnStateChangeAsync(new ToastEventArgs(instance, state));

    public Task OnToggleAsync(DialogToggleEventArgs args)
        => HandleToggleAsync(args);

    internal Task OnActionClickedAsync(bool primary)
    {
        return primary ? Instance!.CloseAsync() : Instance!.CancelAsync();
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

        var toastEventArgs = await RaiseOnStateChangeAsync(args);
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

        if (Instance is ToastInstance toastInstance)
        {
            switch (toastEventArgs.State)
            {
                case DialogState.Closing:
                    toastInstance.ResultCompletion.TrySetResult(ToastResult.Cancel());
                    break;

                case DialogState.Closed:
                    if (ToastService is ToastService toastService)
                    {
                        await toastService.RemoveToastFromProviderAsync(Instance);
                    }

                    break;
            }
        }
    }

    private async Task<ToastEventArgs> RaiseOnStateChangeAsync(ToastEventArgs args)
    {
        if (OnStateChange.HasDelegate)
        {
            await InvokeAsync(() => OnStateChange.InvokeAsync(args));
        }

        return args;
    }
}

#pragma warning restore CS1591, MA0051, MA0123, CA1822
