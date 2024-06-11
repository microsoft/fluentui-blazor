// --------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
// --------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentGridItem : FluentComponentBase
{
#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable SA1300 // Element should begin with upper-case letter

    /// <summary>
    /// The number of columns the item should span in the 12-column grid system.
    /// Extra Small (xs) devices (portrait phones, less than 600px wide)
    /// </summary>
    [Parameter]
    public int? xs { get; set; }

    /// <summary>
    /// The number of columns the item should span in the 12-column grid system.
    /// Small (sm) devices (landscape phones, less than 960px wide)
    /// </summary>
    [Parameter]
    public int? sm { get; set; }

    /// <summary>
    /// The number of columns the item should span in the 12-column grid system.
    /// Medium (md) devices (tablets, less than 1280px wide)
    /// </summary>
    [Parameter]
    public int? md { get; set; }

    /// <summary>
    /// The number of columns the item should span in the 12-column grid system.
    /// Large (lg) devices (desktops, less than 1920px wide)
    /// </summary> 
    [Parameter]
    public int? lg { get; set; }

    /// <summary>
    /// The number of columns the item should span in the 12-column grid system.
    /// Extra large (xl) devices (large desktops, less than 2560px wide)
    /// </summary>
    [Parameter]
    public int? xl { get; set; }

    /// <summary>
    /// The number of columns the item should span in the 12-column grid system.
    /// Extra extra large (xxl) devices (larger desktops, more than 2560px wide)
    /// </summary>
    [Parameter]
    public int? xxl { get; set; }

#pragma warning restore SA1300
#pragma warning restore IDE1006

    [CascadingParameter]
    internal FluentGrid? Grid { get; set; }

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

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class).Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder(Style)
        .AddStyle("justify-content", Justify.ToAttributeValue(), when: Justify is not null)
        .AddStyle("display", "flex", when: Justify is not null)
        .AddStyle("gap", Gap, when: !string.IsNullOrEmpty(Gap))
        .Build();

    /// <summary />
    private bool NoBreakpointsDefined()
    {
        return xs is null
            && sm is null
            && md is null
            && lg is null
            && xl is null
            && xxl is null;
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
        if (Grid != null && Grid.CurrentSize != null && HiddenWhen != null && (Grid.AdaptiveRendering == true || AdaptiveRendering == true))
        {
            return !HiddenWhen.Value.HasFlag(ConvertToHidden(Grid.CurrentSize.Value));
        }

        return true;
    }

    /// <summary />
    private GridItemHidden ConvertToHidden(GridItemSize size)
    {
        switch (size)
        {
            case GridItemSize.Xs:
                return GridItemHidden.Xs;
            case GridItemSize.Sm:
                return GridItemHidden.Sm;
            case GridItemSize.Md:
                return GridItemHidden.Md;
            case GridItemSize.Lg:
                return GridItemHidden.Lg;
            case GridItemSize.Xl:
                return GridItemHidden.Xl;
            case GridItemSize.Xxl:
                return GridItemHidden.Xxl;
            default:
                return GridItemHidden.None;
        }
    }

    /// <summary>
    /// Returns the hidden attribute value for the specified <see cref="GridItemHidden"/> value.
    /// </summary>
    /// <param name="hiddenWhen"></param>
    /// <returns></returns>
    public static string? GetHiddenAttribute(GridItemHidden? hiddenWhen)
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

        var result = string.Join(" ", selected.Where(i => !string.IsNullOrEmpty(i)));

        return string.IsNullOrEmpty(result) ? null : result;
    }
}
