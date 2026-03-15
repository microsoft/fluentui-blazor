// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A toast is a temporary notification that appears in the corner of the screen.
/// </summary>
public partial class FluentToast : FluentComponentBase
{
    /// <summary />
    public FluentToast(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .Build();

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
    public int Timeout { get; set; } = 50000;

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

    internal async Task OnToggleAsync(DialogToggleEventArgs args)
    {
        var newState = string.Empty;
        var argsId = string.Empty;

        if (args is DialogToggleEventArgs toggleArgs)
        {
            newState = toggleArgs.NewState;
            argsId = toggleArgs.Id;
        }

        if (string.CompareOrdinal(argsId, Id) != 0)
        {
            return;
        }

        var toggled = string.Equals(newState, "open", StringComparison.OrdinalIgnoreCase);
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
    }
}
