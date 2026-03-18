// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

#pragma warning disable CS1591

using ComponentModel = System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

[ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never)]
public partial class FluentToastComponentBase : FluentComponentBase
{
    [DynamicDependency(nameof(OnToggleAsync))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(DialogToggleEventArgs))]
    public FluentToastComponentBase(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    [Parameter]
    public FluentToastComponentBase? Owner { get; set; }

    [Parameter]
    public IToastInstance? Instance { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

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
    public bool ShowIntentIcon { get; set; }

    [Parameter]
    public RenderFragment? Media { get; set; }

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

    [Inject]
    private IToastService? ToastService { get; set; }

    internal Icon IntentIcon => Intent switch
    {
        ToastIntent.Success => new CoreIcons.Filled.Size20.CheckmarkCircle(),
        ToastIntent.Warning => new CoreIcons.Filled.Size20.Warning(),
        ToastIntent.Error => new CoreIcons.Filled.Size20.DismissCircle(),
        _ => new CoreIcons.Filled.Size20.Info(),
    };

    internal string? ClassValue => DefaultClassBuilder
        .Build();

    internal string? StyleValue => DefaultStyleBuilder
        .AddStyle("border", "1px solid #ccc;")
        .AddStyle("padding", "16px")
        .AddStyle("position", "fixed")
        .AddStyle("top", "50%")
        .AddStyle("left", "50%")
        .Build();

    private bool LaunchedFromService => Instance is not null;

    internal Task<ToastEventArgs> RaiseOnStateChangeAsync(DialogToggleEventArgs args)
        => RaiseOnStateChangeAsync(new ToastEventArgs(this, args));

    internal Task<ToastEventArgs> RaiseOnStateChangeAsync(IToastInstance instance, DialogState state)
        => RaiseOnStateChangeAsync(new ToastEventArgs(instance, state));

    protected void BuildToastShell(RenderTreeBuilder builder, RenderFragment childContent)
    {
        builder.OpenComponent<FluentToastComponentBase>(0);
        builder.AddAttribute(1, nameof(Owner), this);
        builder.AddAttribute(2, nameof(ChildContent), childContent);
        builder.CloseComponent();
    }

    protected void BuildOwnedContent<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TContent>(RenderTreeBuilder builder)
        where TContent : Microsoft.AspNetCore.Components.IComponent
    {
        builder.OpenComponent<TContent>(0);
        builder.AddAttribute(1, "Owner", this);
        builder.CloseComponent();
    }

    internal async Task OnToggleAsync(DialogToggleEventArgs args)
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

    protected override void OnParametersSet()
    {
        if (GetType().Equals(typeof(FluentToastComponentBase)) && (Owner is null || ChildContent is null))
        {
            throw new InvalidOperationException($"{nameof(FluentToastComponentBase)} must be used as a shell with child content and cannot be rendered directly.");
        }

        base.OnParametersSet();
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

#pragma warning restore CS1591
