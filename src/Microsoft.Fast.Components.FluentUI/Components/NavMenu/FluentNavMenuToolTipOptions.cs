namespace Microsoft.Fast.Components.FluentUI;

public record class FluentNavMenuToolTipOptions
(
    NavMenuShowToolTipsOption ShowToolTips,
    AutoUpdateMode? AutoUpdateMode,
    int? Delay,
    bool HorizontalViewportLock,
    TooltipPosition? Position,
    bool VerticalViewportLock
);

