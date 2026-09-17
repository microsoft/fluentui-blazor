// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The dialog component is a window overlaid on either the primary window or another dialog window.
/// Windows under a modal dialog are inert. 
/// </summary>
public partial class FluentDialogBody : FluentComponentBase
{
    /// <summary />
    public FluentDialogBody(LibraryConfiguration configuration) : base(configuration) { }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .Build();

    /// <summary />
    [CascadingParameter]
    private IDialogInstance? Instance { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the content for the title element.
    /// For an action button in the title area, use <see cref="TitleActionTemplate"/> instead.
    /// </summary>
    [Parameter]
    public RenderFragment? TitleTemplate { get; set; }

    /// <summary>
    /// Gets or sets the content for the action button rendered inside the title area.
    /// For the primary title content, use <see cref="TitleTemplate"/>.
    /// For footer-level actions, use <see cref="ActionTemplate"/>.
    /// </summary>
    [Parameter]
    public RenderFragment? TitleActionTemplate { get; set; }

    /// <summary>
    /// Gets or sets the content for the footer action area (e.g., confirm/cancel buttons).
    /// For an action button inside the title bar, use <see cref="TitleActionTemplate"/> instead.
    /// </summary>
    [Parameter]
    public RenderFragment? ActionTemplate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the header and footer are fixed.
    /// Only the content will scroll when the content overflows.
    /// Default is true.
    /// </summary>
    [Parameter]
    public bool FixedHeaderFooter { get; set; } = true;

    /// <summary />
    internal async Task ActionClickHandlerAsync(DialogOptionsFooterAction item)
    {
        if (item.Disabled || Instance is null)
        {
            return;
        }

        if (item.OnClickAsync is not null)
        {
            await item.OnClickAsync(Instance);
        }
        else
        {
            var result = item.Appearance == ButtonAppearance.Primary ? DialogResult.Ok() : DialogResult.Cancel();
            await Instance.CloseAsync(result);
        }
    }

    /// <summary />
    internal async Task ActionClickHandlerAsync(DialogOptionsHeaderAction item)
    {
        if (!item.Visible || Instance is null)
        {
            return;
        }

        if (item.OnClickAsync is not null)
        {
            await item.OnClickAsync(Instance);
        }
        else if (item.IsClosedAction)
        {
            await Instance.CloseAsync(DialogResult.Cancel());
        }
    }

    /// <summary />
    private bool IsDrawer() => FluentDialog.IsDrawer(Instance);

}
