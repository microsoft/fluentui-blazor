// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// FluentDivider component
/// </summary>
public partial class FluentDivider : FluentComponentBase, ITooltipComponent
{
    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .Build();

    /// <summary>
    /// Gets or sets the alignment of the content.
    /// </summary>
    [Parameter]
    public DividerAlignContent? AlignContent { get; set; }

    /// <summary>
    /// Gets or sets the appearance of the content.
    /// </summary>
    [Parameter]
    public DividerAppearance? Appearance { get; set; }

    /// <summary>
    /// Adds padding to the beginning and end of the divider.
    /// </summary>
    [Parameter]
    public bool? Inset { get; set; }

    /// <summary>
    /// A divider can be horizontal (default) or vertical.
    /// </summary>
    [Parameter]
    public bool? Vertical { get; set; }

    /// <summary>
    /// Gets or sets the content to be shown.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <inheritdoc cref="ITooltipComponent.Tooltip" />
    [Parameter]
    public string? Tooltip { get; set; }

    /// <summary />
    protected override async Task OnInitializedAsync()
    {
        await base.RenderTooltipAsync(Tooltip);
    }

    private string? Orientation
        => Vertical.HasValue
            ? Vertical.Value
                ? "vertical"
                : "horizontal"
            : null;
}
