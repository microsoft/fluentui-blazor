using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentLabel : FluentComponentBase
{
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass($"fluent-typography")
        .AddClass("fluent-typo-left", () => Alignment == HorizontalAlignment.Left || Alignment == HorizontalAlignment.Start)
        .AddClass("fluent-typo-center", () => Alignment == HorizontalAlignment.Center)
        .AddClass("fluent-typo-right", () => Alignment == HorizontalAlignment.Right || Alignment == HorizontalAlignment.End)
        .Build();

    protected string? StyleValue => new StyleBuilder(Style)
        .AddStyle("color", Color.ToAttributeValue(), () => Color != null)
        .AddStyle("margin-block", MarginBlock, () => !string.IsNullOrEmpty(MarginBlock) && !DefaultMarginBlock)
        .Build();

    /// <summary>
    /// Applies the theme typography styles.
    /// </summary>
    [Parameter]
    public Typography Typo { get; set; } = Typography.Body;

    /// <summary>
    /// Activates or deactivates the component (changes the color).
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; } = false;

    /// <summary>
    /// Gets or sets the text-align on the component.
    /// </summary>
    [Parameter]
    public HorizontalAlignment? Alignment { get; set; }

    /// <summary>
    /// Gets or sets the color of the component. It supports the theme colors.
    /// </summary>
    [Parameter]
    public Color? Color { get; set; }

    /// <summary>
    /// Gets or sets the font weight of the component:
    /// Normal (400), Bold (600) or Bolder (800).
    /// </summary>
    [Parameter]
    public FontWeight Weight { get; set; } = FontWeight.Normal;

    /// <summary>
    /// Gets or sets the margin block of the component.
    /// "default" to use the margin-block prefefined by browser.
    /// If not set, the MarginBlock will be 0px.
    /// </summary>
    [Parameter]
    public string? MarginBlock { get; set; }

    /// <summary>
    /// Gets or sets the child content of component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private bool? Bold => Weight == FontWeight.Bold;

    private bool? Bolder => Weight == FontWeight.Bolder;

    private bool DefaultMarginBlock => string.Compare(MarginBlock, "default", StringComparison.OrdinalIgnoreCase) == 0;
}
