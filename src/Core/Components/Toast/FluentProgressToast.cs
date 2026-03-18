// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

#pragma warning disable CS1591, MA0051, MA0123, CA1822

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A toast that displays progress for an ongoing operation.
/// </summary>
public partial class FluentProgressToast : FluentToastComponentBase
{
    /// <summary />
    public FluentProgressToast(LibraryConfiguration configuration) : base(configuration)
    {
    }

    /// <summary>
    /// Gets or sets the title of the toast.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the body text of the toast.
    /// </summary>
    [Parameter]
    public string? Body { get; set; }

    /// <summary>
    /// Gets or sets the status text of the toast.
    /// </summary>
    [Parameter]
    public string? Status { get; set; }

    /// <summary>
    /// Gets or sets the maximum progress value.
    /// </summary>
    [Parameter]
    public int? Max { get; set; } = 100;

    /// <summary>
    /// Gets or sets the current progress value.
    /// </summary>
    [Parameter]
    public int? Value { get; set; }

    /// <summary>
    /// Gets or sets whether the progress is indeterminate.
    /// </summary>
    [Parameter]
    public bool Indeterminate { get; set; } = true;

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
    /// Gets or sets whether the dismiss button is shown.
    /// </summary>
    [Parameter]
    public bool ShowDismissButton { get; set; }

    internal static Icon DismissIcon => new CoreIcons.Regular.Size20.Dismiss();

    public new Task<ToastEventArgs> RaiseOnStateChangeAsync(DialogToggleEventArgs args)
        => base.RaiseOnStateChangeAsync(args);

    public new Task<ToastEventArgs> RaiseOnStateChangeAsync(IToastInstance instance, DialogState state)
        => base.RaiseOnStateChangeAsync(instance, state);

    public new Task OnToggleAsync(DialogToggleEventArgs args)
        => base.OnToggleAsync(args);

    protected override void BuildRenderTree(RenderTreeBuilder __builder)
        => BuildToastShell(__builder, BuildContent);

    private void BuildContent(RenderTreeBuilder __builder)
        => BuildOwnedContent<FluentProgressToastContent>(__builder);
    internal Task OnActionClickedAsync(bool primary)
    {
        return primary ? Instance!.CloseAsync() : Instance!.CancelAsync();
    }
}

#pragma warning restore CS1591, MA0051, MA0123, CA1822
