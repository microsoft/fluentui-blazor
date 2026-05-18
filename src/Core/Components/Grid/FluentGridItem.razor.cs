// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Value placed within a <see cref="FluentGrid"/> layout using the <see cref="FluentGridItem"/> component.
/// </summary>
public partial class FluentGridItem : FluentComponentBase
{
    /// <summary />
    public FluentGridItem(LibraryConfiguration configuration) : base(configuration) { }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder.Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("justify-content", Justify.ToAttributeValue(), when: Justify is not null)
        .AddStyle("display", "flex", when: Justify is not null)
        .AddStyle("gap", Gap, when: !string.IsNullOrEmpty(Gap))
        .Build();

    /// <summary>
    /// Gets or sets a reference to the parent grid component.
    /// </summary>
    [CascadingParameter]
    protected FluentGrid? Grid { get; set; }

    /// <summary>
    /// Gets or sets the number of columns (1–12) the item spans on Extra Small devices (portrait phones, less than 600px wide).
    /// Use with <see cref="Sm"/>, <see cref="Md"/>, <see cref="Lg"/>, <see cref="Xl"/>, <see cref="Xxl"/> for responsive layouts.
    /// </summary>
    [Parameter]
    public int? Xs { get; set; }

    /// <summary>
    /// Gets or sets the number of columns (1–12) the item spans on Small devices (landscape phones, less than 960px wide).
    /// Use with <see cref="Xs"/>, <see cref="Md"/>, <see cref="Lg"/>, <see cref="Xl"/>, <see cref="Xxl"/> for responsive layouts.
    /// </summary>
    [Parameter]
    public int? Sm { get; set; }

    /// <summary>
    /// Gets or sets the number of columns (1–12) the item spans on Medium devices (tablets, less than 1280px wide).
    /// Use with <see cref="Xs"/>, <see cref="Sm"/>, <see cref="Lg"/>, <see cref="Xl"/>, <see cref="Xxl"/> for responsive layouts.
    /// </summary>
    [Parameter]
    public int? Md { get; set; }

    /// <summary>
    /// Gets or sets the number of columns (1–12) the item spans on Large devices (desktops, less than 1920px wide).
    /// Use with <see cref="Xs"/>, <see cref="Sm"/>, <see cref="Md"/>, <see cref="Xl"/>, <see cref="Xxl"/> for responsive layouts.
    /// </summary>
    [Parameter]
    public int? Lg { get; set; }

    /// <summary>
    /// Gets or sets the number of columns (1–12) the item spans on Extra Large devices (large desktops, less than 2560px wide).
    /// Use with <see cref="Xs"/>, <see cref="Sm"/>, <see cref="Md"/>, <see cref="Lg"/>, <see cref="Xxl"/> for responsive layouts.
    /// </summary>
    [Parameter]
    public int? Xl { get; set; }

    /// <summary>
    /// Gets or sets the number of columns (1–12) the item spans on Extra Extra Large devices (larger desktops, more than 2560px wide).
    /// Use with <see cref="Xs"/>, <see cref="Sm"/>, <see cref="Md"/>, <see cref="Lg"/>, <see cref="Xl"/> for responsive layouts.
    /// </summary>
    [Parameter]
    public int? Xxl { get; set; }

    /// <summary>
    /// Gets or sets how the browser distributes space between and around content items within this grid item.
    /// </summary>
    [Parameter]
    public JustifyContent? Justify { get; set; }

    /// <summary>
    /// Gets or sets the gaps (gutters) between rows and columns (e.g., <c>Gap="8px"</c>).
    /// See <see href="https://developer.mozilla.org/en-US/docs/Web/CSS/gap">CSS gap</see>.
    /// </summary>
    [Parameter]
    public string? Gap { get; set; }

    /// <summary>
    /// Gets or sets the adaptive rendering behavior: when <c>true</c>, the HTML is not rendered when the item is hidden;
    /// when <c>false</c>, the item is hidden via CSS only. Default is <c>false</c>.
    /// </summary>
    [Parameter]
    public bool? AdaptiveRendering { get; set; }

    /// <summary>
    /// Gets or sets the breakpoint sizes at which this item is hidden (e.g., <c>HiddenWhen="GridItemHidden.Sm | GridItemHidden.Xl"</c>).
    /// See <see cref="GridItemHidden"/> for available values.
    /// </summary>
    [Parameter]
    public GridItemHidden? HiddenWhen { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary />
    private bool NoBreakpointsDefined()
    {
        return Xs is null
            && Sm is null
            && Md is null
            && Lg is null
            && Xl is null
            && Xxl is null;
    }

    /// <summary />
    private string? HiddenAttribute
    {
        get
        {
            return GetHiddenAttribute(HiddenWhen);
        }
    }

    /// <summary />
    private bool RenderChildContent()
    {
        if (Grid != null && Grid.CurrentSize != null && HiddenWhen != null && (Grid.AdaptiveRendering || AdaptiveRendering == true))
        {
            return !HiddenWhen.Value.HasFlag(ConvertToHidden(Grid.CurrentSize.Value));
        }

        return true;
    }

    /// <summary />
    internal static GridItemHidden ConvertToHidden(GridItemSize size)
    {
        return size switch
        {
            GridItemSize.Xs => GridItemHidden.Xs,
            GridItemSize.Sm => GridItemHidden.Sm,
            GridItemSize.Md => GridItemHidden.Md,
            GridItemSize.Lg => GridItemHidden.Lg,
            GridItemSize.Xl => GridItemHidden.Xl,
            GridItemSize.Xxl => GridItemHidden.Xxl,
            _ => GridItemHidden.None,
        };
    }

    /// <summary>
    /// Returns the hidden attribute value for the specified <see cref="GridItemHidden"/> value.
    /// </summary>
    /// <param name="hiddenWhen"></param>
    /// <returns></returns>
    internal static string? GetHiddenAttribute(GridItemHidden? hiddenWhen)
    {
        var selected = new string[]
            {
                (hiddenWhen & GridItemHidden.Xs) == GridItemHidden.Xs ? "xs" : string.Empty,
                (hiddenWhen & GridItemHidden.Sm) == GridItemHidden.Sm ? "sm" : string.Empty,
                (hiddenWhen & GridItemHidden.Md) == GridItemHidden.Md ? "md" : string.Empty,
                (hiddenWhen & GridItemHidden.Lg) == GridItemHidden.Lg ? "lg" : string.Empty,
                (hiddenWhen & GridItemHidden.Xl) == GridItemHidden.Xl ? "xl" : string.Empty,
                (hiddenWhen & GridItemHidden.Xxl) == GridItemHidden.Xxl ? "xxl" : string.Empty,
            };

        var result = string.Join(' ', selected.Where(i => !string.IsNullOrEmpty(i)));

        return string.IsNullOrEmpty(result) ? null : result;
    }
}
