// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The Toast component is a window overlaid on either the primary window or another Toast window.
/// Windows under a modal Toast are inert.
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
    [Inject]
    private IToastService? ToastService { get; set; }

    /// <summary>
    /// Gets or sets the instance used by the <see cref="ToastService" />.
    /// </summary>
    [Parameter]
    public IToastInstance? Instance { get; set; }

    /// <summary>
    /// Used when not calling the <see cref="ToastService" /> to show a Toast.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

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

            // TODO
            return Task.CompletedTask; // return ShowAsync();
        }

        return Task.CompletedTask;
    }

    /// <summary />
    internal async Task OnToggleAsync(DialogToggleEventArgs args)
    {
        if (string.CompareOrdinal(args.Id, Instance?.Id) != 0)
        {
            return;
        }

        // TODO
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
