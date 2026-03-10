// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A toast is a temporary notification that appears in the corner of the screen.
/// </summary>
public partial class FluentToast : FluentComponentBase
{
    /// <summary />
    [DynamicDependency(nameof(OnToggleAsync))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(DialogToggleEventArgs))]
    public FluentToast(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        // TDOD: Remode these styles (only for testing purposes)
        .AddStyle("border", "1px solid #ccc;")
        .AddStyle("padding", "16px")
        .AddStyle("position", "fixed")
        .AddStyle("top", "50%")
        .AddStyle("left", "50%")
        .Build();

    /// <summary />
    [Inject]
    private IToastService? ToastService { get; set; }

    /// <summary>
    /// Gets or sets the instance used by the <see cref="ToastService" />.
    /// </summary>
    [Parameter]
    public IToastInstance? Instance { get; set; }

    /// <summary>
    /// Gets or sets the content to be displayed in the toast.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets whether the toast is opened.
    /// </summary>
    [Parameter]
    public bool Opened { get; set; }

    /// <summary>
    /// Gets or sets the event callback for when the opened state changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> OpenedChanged { get; set; }

    /// <summary>
    /// Gets or sets the timeout in milliseconds. Default is 5000ms.
    /// Set to 0 to disable auto-dismiss.
    /// </summary>
    [Parameter]
    public int Timeout { get; set; } = 7000;

    /// <summary>
    /// Gets or sets the toast position.
    /// Default is TopRight.
    /// </summary>
    [Parameter]
    public ToastPosition? Position { get; set; }

    /// <summary>
    /// Gets or sets the vertical offset for stacking multiple toasts.
    /// </summary>
    [Parameter]
    public int VerticalOffset { get; set; } = 16;

    /// <summary>
    /// Gets or sets the horizontal offset.
    /// </summary>
    [Parameter]
    public int HorizontalOffset { get; set; } = 16;

    /// <summary>
    /// Gets or sets the intent of the toast.
    /// Default is Info.
    /// </summary>
    [Parameter]
    public ToastIntent Intent { get; set; } = ToastIntent.Info;

    /// <summary>
    /// Gets or sets the event callback for when the toast is toggled.
    /// </summary>
    [Parameter]
    public EventCallback<bool> OnToggle { get; set; }

    /// <summary>
    /// Command executed when the user clicks on the button.
    /// </summary>
    [Parameter]
    public EventCallback<ToastEventArgs> OnStateChange { get; set; }

    /// <summary />
    private bool LaunchedFromService => Instance is not null;

    /// <summary />
    internal Task<ToastEventArgs> RaiseOnStateChangeAsync(DialogToggleEventArgs args) => RaiseOnStateChangeAsync(new ToastEventArgs(this, args));

    /// <summary />
    internal Task<ToastEventArgs> RaiseOnStateChangeAsync(IToastInstance instance, DialogState state) => RaiseOnStateChangeAsync(new ToastEventArgs(instance, state));

    internal async Task OnToggleAsync(DialogToggleEventArgs args)
    {
        // Validate that the event belongs to this toast. For service-launched toasts, the DOM id
        // is set to Instance.Id. For standalone usage it's the component Id.
        var expectedId = Instance?.Id ?? Id;
        if (string.CompareOrdinal(args.Id, expectedId) != 0)
        {
            return;
        }

        // Raise the event received from the Web Component
        var toastEventArgs = await RaiseOnStateChangeAsync(args);

        // Keep the Opened parameter in sync for both standalone and service usage.
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

        if (LaunchedFromService)
        {
            switch (toastEventArgs.State)
            {
                case DialogState.Closing:
                    (Instance as ToastInstance)?.ResultCompletion.TrySetResult(ToastResult.Cancel());
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

    /// <summary />
    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && LaunchedFromService)
        {
            var instance = Instance as ToastInstance;
            if (instance is not null)
            {
                instance.FluentToast = this;
            }

            if (!Opened)
            {
                Opened = true;
                return InvokeAsync(StateHasChanged);
            }
        }

        return Task.CompletedTask;
    }

    /// <summary />
    private async Task<ToastEventArgs> RaiseOnStateChangeAsync(ToastEventArgs args)
    {
        if (OnStateChange.HasDelegate)
        {
            await InvokeAsync(() => OnStateChange.InvokeAsync(args));
        }

        return args;
    }
}
