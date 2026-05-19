// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A horizontal or vertical rule used to visually separate content.
/// </summary>
public partial class FluentDivider : FluentComponentBase, ITooltipComponent
{
    /// <summary />
    public FluentDivider(LibraryConfiguration configuration) : base(configuration) { }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .Build();

    /// <summary>
    /// Gets or sets the alignment of any child content within the divider (e.g., <c>AlignContent="DividerAlignContent.Center"</c>).
    /// See <see cref="DividerAlignContent"/> for available values.
    /// </summary>
    [Parameter]
    public DividerAlignContent? AlignContent { get; set; }

    /// <summary>
    /// Gets or sets the visual appearance of the divider (e.g., <c>Appearance="DividerAppearance.Strong"</c>).
    /// See <see cref="DividerAppearance"/> for available values.
    /// </summary>
    [Parameter]
    public DividerAppearance? Appearance { get; set; }

    /// <summary>
    /// Gets or sets whether padding is added to the beginning and end of the divider (e.g., <c>Inset="true"</c>).
    /// </summary>
    [Parameter]
    public bool? Inset { get; set; }

    /// <summary>
    /// Gets or sets whether the divider is vertical (<c>true</c>) or horizontal (<c>false</c>, default).
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
