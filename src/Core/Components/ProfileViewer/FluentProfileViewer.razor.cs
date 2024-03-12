// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentProfileViewer : FluentComponentBase
{
    public FluentProfileViewer()
    {
        Id = Identifier.NewId();
    }

    private bool PopoverVisible { get; set; } = false;

    [Parameter]
    public string? Image { get; set; }
}
