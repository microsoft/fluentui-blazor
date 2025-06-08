// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentPopover : FluentComponentBase
{
    /// <summary />
    public FluentPopover(LibraryConfiguration configuration) : base(configuration) { }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("z-index", ZIndex.Popover.ToString(CultureInfo.InvariantCulture))
        .Build();

    /// <summary />
    [Parameter]
    public required string AnchorId { get; set; }

    /// <summary />
    [Parameter]
    public string? TriggerId { get; set; }

    /// <summary />
    [Parameter]
    public bool Opened { get; set; }

    /// <summary />
    [Parameter]
    public EventCallback<bool> OpenedChanged { get; set; }

    /// <summary />
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
