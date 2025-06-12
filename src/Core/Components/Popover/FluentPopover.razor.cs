// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The FluentPopover component is used to displays content on top of other content.
/// </summary>
public partial class FluentPopover : FluentComponentBase
{
    /// <summary />
    public FluentPopover(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("width", Width)
        .AddStyle("height", Height)
        .Build();

    /// <summary>
    /// Gets or sets the id of the component the popover is positioned relative to.
    /// </summary>
    [Parameter]
    public required string AnchorId { get; set; }

    /// <summary>
    /// Gets or sets the width of the popover component.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the width of the popover component.
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Gets or sets the vertical offset value (pixels) between the popover and the anchor element.
    /// </summary>
    [Parameter]
    public int OffsetVertical { get; set; }

    /// <summary>
    /// Gets or sets the horizontal offset value (pixels) between the popover and the anchor element.
    /// </summary>
    [Parameter]
    public int OffsetHorizontal { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the component is currently open.
    /// </summary>
    [Parameter]
    public bool Opened { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the opened state changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> OpenedChanged { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary />
    internal async Task OnToggleAsync(DialogToggleEventArgs args)
    {
        if (string.CompareOrdinal(args.Id, Id) != 0)
        {
            return;
        }

        var opened = string.Equals(args.NewState, "open", StringComparison.OrdinalIgnoreCase);
        if (Opened != opened)
        {
            Opened = opened;

            if (OpenedChanged.HasDelegate)
            {
                await OpenedChanged.InvokeAsync(Opened);
            }
        }
    }
}
