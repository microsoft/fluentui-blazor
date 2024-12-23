// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The dialog component is a window overlaid on either the primary window or another dialog window.
/// Windows under a modal dialog are inert. 
/// </summary>
public partial class FluentDialogBody : FluentComponentBase
{
    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder(Style)
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
    /// </summary>
    [Parameter]
    public RenderFragment? TitleTemplate { get; set; }

    /// <summary>
    /// Gets or sets the content for the action element in the title.
    /// </summary>
    [Parameter]
    public RenderFragment? TitleActionTemplate { get; set; }

    /// <summary>
    /// Gets or sets the content for the action element.
    /// </summary>
    [Parameter]
    public RenderFragment? ActionTemplate { get; set; }

    /// <summary />
    private async Task ActionClickHandlerAsync(DialogOptionsFooterAction item)
    {
        if (item.Disabled || Instance is null)
        {
            return;
        }

        if (item.OnClickAsync is not null )
        {
            await item.OnClickAsync(Instance);
        }
        else
        {
            var result = item.Appearance == ButtonAppearance.Primary ? DialogResult.Ok() : DialogResult.Cancel();
            await Instance.CloseAsync(result);
        }
    }
}
