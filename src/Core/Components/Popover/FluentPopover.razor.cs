// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentPopover : FluentComponentBase
{
    /// <summary />
    public FluentPopover(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    [Parameter]
    public required string AnchorId { get; set; }

    /// <summary />
    [Parameter]
    public string? TriggerId { get; set; }

    ///// <summary />
    //protected override async Task OnAfterRenderAsync(bool firstRender)
    //{
    //    if (firstRender)
    //    {
    //        var options = new
    //        {
    //            Id = Id,
    //            AnchorId = AnchorId,
    //            TriggerId = TriggerId ?? AnchorId,
    //            DialogId = Id,
    //            OffsetVertical = 10,
    //        };

    //        await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.Popover.Initialize", options);
    //    }
    //}
}
