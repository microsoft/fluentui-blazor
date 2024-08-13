// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Content placed within a <see cref="FluentGrid"/> layout using the <see cref="FluentGridItem"/> component.
/// </summary>
public partial class FluentGridItem : FluentComponentBase
{
    /// <summary />
    protected string? ClassValue => new CssBuilder(Class).Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder(Style)
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
    /// The number of columns the item should span in the 12-column grid system.
    /// Extra Small (xs) devices (portrait phones, less than 600px wide)
    /// </summary>
    [Parameter]
    public int? Xs { get; set; }

    /// <summary>
    /// The number of columns the item should span in the 12-column grid system.
    /// Small (sm) devices (landscape phones, less than 960px wide)
    /// </summary>
    [Parameter]
    public int? Sm { get; set; }

    /// <summary>
    /// The number of columns the item should span in the 12-column grid system.
    /// Medium (md) devices (tablets, less than 1280px wide)
    /// </summary>
    [Parameter]
    public int? Md { get; set; }

    /// <summary>
    /// The number of columns the item should span in the 12-column grid system.
    /// Large (lg) devices (desktops, less than 1920px wide)
    /// </summary> 
    [Parameter]
    public int? Lg { get; set; }

    /// <summary>
    /// The number of columns the item should span in the 12-column grid system.
    /// Extra large (xl) devices (large desktops, less than 2560px wide)
    /// </summary>
    [Parameter]
    public int? Xl { get; set; }

    /// <summary>
    /// The number of columns the item should span in the 12-column grid system.
    /// Extra extra large (xxl) devices (larger desktops, more than 2560px wide)
    /// </summary>
    [Parameter]
    public int? Xxl { get; set; }

    /// <summary>
    /// Defines how the browser distributes space between and around content items.
    /// </summary>
    [Parameter]
    public JustifyContent? Justify { get; set; }

    /// <summary>
    /// Gets or sets the gaps (gutters) between rows and columns.
    /// See https://developer.mozilla.org/en-US/docs/Web/CSS/gap
    /// </summary>
    [Parameter]
    public string? Gap { get; set; }

    /// <summary>
    /// Gets or sets the adaptive rendering, which not render the HTML code when the item is hidden (true) or only hide the item by CSS (false).
    /// Default is false.
    /// </summary>
    [Parameter]
    public bool? AdaptiveRendering { get; set; }

    /// <summary>
    /// Hide the item on the specified sizes (you can combine multiple values: GridItemHidden.Sm | GridItemHidden.Xl).
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
