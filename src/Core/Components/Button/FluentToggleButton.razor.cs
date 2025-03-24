// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The FluentToggleButton component allows users to commit a change or trigger a toggle action via a single click or tap and
/// is often found inside forms, dialogs, drawers (panels) and/or pages.
/// </summary>
public partial class FluentToggleButton : FluentButton
{
    private bool _emptyContent => ChildContent is null && Label is null;

    /// <summary>
    /// Gets or sets the mixed state of the component.
    /// </summary>
    [Parameter]
    public bool Mixed { get; set; } = false;

    /// <summary>
    /// Gets or sets the pressed state of the button.
    /// </summary>
    [Parameter]
    public bool Pressed { get; set; }

    /// <summary />
    protected new async Task OnClickHandlerAsync(MouseEventArgs e)
    {
        Pressed = !Pressed;
        await base.OnClickHandlerAsync(e);
    }
}
