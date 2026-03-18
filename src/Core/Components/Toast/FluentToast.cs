// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

#pragma warning disable CS1591, MA0051, MA0123, CA1822

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A toast is a temporary notification that appears in the corner of the screen.
/// </summary>
public partial class FluentToast : FluentToastComponentBase
{
    /// <summary />
    public FluentToast(LibraryConfiguration configuration) : base(configuration)
    {
    }

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

    internal Icon DismissIcon => new CoreIcons.Regular.Size20.Dismiss();

    public new Task<ToastEventArgs> RaiseOnStateChangeAsync(DialogToggleEventArgs args)
        => base.RaiseOnStateChangeAsync(args);

    public new Task<ToastEventArgs> RaiseOnStateChangeAsync(IToastInstance instance, DialogState state)
        => base.RaiseOnStateChangeAsync(instance, state);

    public new Task OnToggleAsync(DialogToggleEventArgs args)
        => base.OnToggleAsync(args);

    protected override void BuildRenderTree(RenderTreeBuilder __builder)
        => BuildToastShell(__builder, BuildContent);

    private void BuildContent(RenderTreeBuilder __builder)
        => BuildOwnedContent<FluentToastContent>(__builder);

    internal Task OnActionClickedAsync(bool primary)
    {
        return primary ? Instance!.CloseAsync() : Instance!.CancelAsync();
    }

#pragma warning restore CS1591, MA0051, MA0123, CA1822

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
}
