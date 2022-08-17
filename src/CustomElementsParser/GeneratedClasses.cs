using Microsoft.AspNetCore.Components;


namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentAnchor
{
    [Parameter]
    public string Download { get; set; }

    [Parameter]
    public string Href { get; set; }

    [Parameter]
    public string Hreflang { get; set; }

    [Parameter]
    public string Ping { get; set; }

    [Parameter]
    public string Referrerpolicy { get; set; }

    [Parameter]
    public string Rel { get; set; }

    [Parameter]
    // "_self" | "_blank" | "_parent" | "_top" 
    public string Target { get; set; }

    [Parameter]
    public string Type { get; set; }

}

public partial class FluentAnchoredRegion
{
    [Parameter]
    public string Anchor { get; set; }

    [Parameter]
    public string Viewport { get; set; }

    [Parameter]
    public AxisPositioningMode HorizontalPositioningMode { get; set; }

    [Parameter]
    public HorizontalPosition HorizontalDefaultPosition { get; set; }

    [Parameter]
    public bool HorizontalViewportLock { get; set; }

    [Parameter]
    public bool HorizontalInset { get; set; }

    [Parameter]
    public int HorizontalThreshold { get; set; }

    [Parameter]
    public AxisScalingMode HorizontalScaling { get; set; }

    [Parameter]
    public AxisPositioningMode VerticalPositioningMode { get; set; }

    [Parameter]
    public VerticalPosition VerticalDefaultPosition { get; set; }

    [Parameter]
    public bool VerticalViewportLock { get; set; }

    [Parameter]
    public bool VerticalInset { get; set; }

    [Parameter]
    public int VerticalThreshold { get; set; }

    [Parameter]
    public AxisScalingMode VerticalScaling { get; set; }

    [Parameter]
    public bool FixedPlacement { get; set; }

    [Parameter]
    public AutoUpdateMode AutoUpdateMode { get; set; }

}

public partial class FluentAvatar
{
    [Parameter]
    public string Link { get; set; }

}

public partial class FluentAccordion
{
    [Parameter]
    public AccordionExpandMode ExpandMode { get; set; }

}

public partial class FluentAccordionItem
{
    [Parameter]
    // 1 | 2 | 3 | 4 | 5 | 6
    public string HeadingLevel { get; set; }

    [Parameter]
    public string Id { get; set; }

}

public partial class FluentBadge
{
}

public partial class FluentBreadcrumb
{
}

public partial class FluentCalendar
{
    [Parameter]
    public string Locale { get; set; }

    [Parameter]
    public DayFormat DayFormat { get; set; }

    [Parameter]
    public WeekdayFormat WeekdayFormat { get; set; }

    [Parameter]
    public MonthFormat MonthFormat { get; set; }

    [Parameter]
    public YearFormat YearFormat { get; set; }

    [Parameter]
    public int MinWeeks { get; set; }

    [Parameter]
    public string DisabledDates { get; set; }

    [Parameter]
    public string SelectedDates { get; set; }

}

public partial class FluentCard
{
}

public partial class FluentDataGridCell
{
    [Parameter]
    public DataGridCellType CellType { get; set; }

    [Parameter]
    public string GridColumn { get; set; }

}

public partial class FluentDataGridRow
{
    [Parameter]
    public string GridTemplateColumns { get; set; }

    [Parameter]
    public DataGridRowType RowType { get; set; }

}

public partial class FluentDataGrid
{
    [Parameter]
    public bool NoTabbing { get; set; }

    [Parameter]
    public GenerateHeaderOption GenerateHeader { get; set; }

    [Parameter]
    public string GridTemplateColumns { get; set; }

}

public partial class FluentDialog
{
    [Parameter]
    public bool NoFocusTrap { get; set; }

    [Parameter]
    public string AriaDescribedby { get; set; }

    [Parameter]
    public string AriaLabelledby { get; set; }

    [Parameter]
    public string AriaLabel { get; set; }

}

public partial class FluentDisclosure
{
    [Parameter]
    public string Summary { get; set; }

}

public partial class FluentDivider
{
    [Parameter]
    public DividerRole Role { get; set; }

    [Parameter]
    public Orientation Orientation { get; set; }

}

public partial class FluentFlipper
{
    [Parameter]
    public bool AriaHidden { get; set; }

    [Parameter]
    public FlipperDirection Direction { get; set; }

}

public partial class FluentHorizontalScroll
{
    [Parameter]
    public string Duration { get; set; }

    [Parameter]
    public ScrollEasing Easing { get; set; }

    [Parameter]
    public bool FlippersHiddenFromAt { get; set; }

    [Parameter]
    public HorizontalScrollView View { get; set; }

}

public partial class FluentListbox
{
}

public partial class FluentListboxOption
{
    [Parameter]
    public bool Selected { get; set; }

    [Parameter]
    public string Value { get; set; }

}

public partial class FluentMenu
{
}

public partial class FluentMenuItem
{
    [Parameter]
    public MenuItemRole Role { get; set; }

}

public partial class FluentPickerListItem
{
    [Parameter]
    public string Value { get; set; }

}

public partial class FluentPickerList
{
}

public partial class FluentPickerMenuOption
{
    [Parameter]
    public string Value { get; set; }

}

public partial class FluentPickerMenu
{
}

public partial class FluentBaseProgress
{
}

public partial class FluentRadioGroup
{
    [Parameter]
    public bool Readonly { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public string Name { get; set; }

    [Parameter]
    public string Value { get; set; }

    [Parameter]
    // | "horizontal" | "vertical"
    public Orientation Orientation { get; set; }

}

public partial class FluentSkeleton
{
    [Parameter]
    public string Fill { get; set; }

    [Parameter]
    public SkeletonShape Shape { get; set; }

    [Parameter]
    public string Pattern { get; set; }

}

public partial class FluentSliderLabel
{
    [Parameter]
    public string Position { get; set; }

    [Parameter]
    public bool HideMark { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

}

public partial class FluentTab
{
}

public partial class FluentTabPanel
{
}

public partial class FluentTabs
{
    [Parameter]
    public TabsOrientation Orientation { get; set; }

    [Parameter]
    public string Activeid { get; set; }

    [Parameter]
    public bool HideActiveIndicator { get; set; }

}

public partial class FluentToolbar
{
    [Parameter]
    public Orientation Orientation { get; set; }

}

public partial class FluentTooltip
{
    [Parameter]
    public string Anchor { get; set; }

    [Parameter]
    public int Delay { get; set; }

    [Parameter]
    public TooltipPosition Position { get; set; }

    [Parameter]
    public AutoUpdateMode AutoUpdateMode { get; set; }

    [Parameter]
    public bool HorizontalViewportLock { get; set; }

    [Parameter]
    public bool VerticalViewportLock { get; set; }

}

public partial class FluentTreeItem
{
}

public partial class FluentTreeView
{
    [Parameter]
    public bool RenderCollapsedNodes { get; set; }

}