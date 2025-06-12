// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// List of icons used and embedded in the FluentUI components.
/// </summary>
internal static partial class CoreIcons
{
    /*
     * *************************** NOTES ***************************
     *
     * Try to use only the Regular.Size20 and Filled.Size20 icons,
     * to avoid duplicating the same icon in different sizes.
     * *************************************************************
     */

    /// <summary>
    /// Regular icons
    /// </summary>
    internal static partial class Regular
    {
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        internal static partial class Size20
        {
            public class Add : Icon { public Add() : base("Add", IconVariant.Regular, IconSize.Size20, "<path d=\"M10 2.5a.5.5 0 0 0-1 0V9H2.5a.5.5 0 0 0 0 1H9v6.5a.5.5 0 0 0 1 0V10h6.5a.5.5 0 0 0 0-1H10V2.5Z\"/>") { } }

            public class ArrowReset : Icon { public ArrowReset() : base("ArrowReset", IconVariant.Regular, IconSize.Size20, "<path d=\"M5.85 2.65c.2.2.2.5 0 .7L4.21 5H11a6 6 0 1 1-6 6 .5.5 0 0 1 1 0 5 5 0 1 0 5-5H4.2l1.65 1.65a.5.5 0 1 1-.7.7l-2.5-2.5a.5.5 0 0 1 0-.7l2.5-2.5c.2-.2.5-.2.7 0Z\"/>") { } }

            public class ArrowSort : Icon { public ArrowSort() : base("ArrowSort", IconVariant.Regular, IconSize.Size20, "<path d=\"M2.35 7.35 5 4.71V16.5a.5.5 0 0 0 1 0V4.7l2.65 2.65a.5.5 0 0 0 .7-.7l-3.49-3.5A.5.5 0 0 0 5.5 3a.5.5 0 0 0-.39.18L1.65 6.65a.5.5 0 1 0 .7.7Zm15.3 5.3L15 15.29V3.5a.5.5 0 0 0-1 0v11.8l-2.65-2.65a.5.5 0 0 0-.7.7l3.49 3.5a.5.5 0 0 0 .36.15.5.5 0 0 0 .39-.18l3.46-3.47a.5.5 0 1 0-.7-.7Z\"/>") { } }

            public class ArrowSortDown : Icon { public ArrowSortDown() : base("ArrowSortDown", IconVariant.Regular, IconSize.Size20, "<path d=\"m10 15.29 2.65-2.64a.5.5 0 0 1 .7.7L9.9 16.82a.5.5 0 0 1-.74.03h-.01l-3.5-3.5a.5.5 0 1 1 .71-.7L9 15.29V3.5a.5.5 0 0 1 1 0v11.79Z\"/>") { } }

            public class ArrowSortUp : Icon { public ArrowSortUp() : base("ArrowSortUp", IconVariant.Regular, IconSize.Size20, "<path d=\"M9 4.71 6.35 7.35a.5.5 0 1 1-.7-.7L9.1 3.18a.5.5 0 0 1 .74-.03h.01l3.5 3.5a.5.5 0 1 1-.71.7L10 4.71V16.5a.5.5 0 0 1-1 0V4.71Z\"/>") { } }

            public class ChevronDown : Icon { public ChevronDown() : base("ChevronDown", IconVariant.Regular, IconSize.Size20, "<path d=\"M15.85 7.65c.2.2.2.5 0 .7l-5.46 5.49a.55.55 0 0 1-.78 0L4.15 8.35a.5.5 0 1 1 .7-.7L10 12.8l5.15-5.16c.2-.2.5-.2.7 0Z\"/>") { } }

            public class ChevronDoubleLeft : Icon { public ChevronDoubleLeft() : base("ChevronDoubleLeft", IconVariant.Regular, IconSize.Size20, "<path d=\"M11.35 15.85a.5.5 0 0 1-.7 0L5.16 10.4a.55.55 0 0 1 0-.78l5.49-5.46a.5.5 0 1 1 .7.7L6.2 10l5.16 5.15c.2.2.2.5 0 .7Zm4 0a.5.5 0 0 1-.7 0L9.16 10.4a.55.55 0 0 1 0-.78l5.49-5.46a.5.5 0 1 1 .7.7L10.2 10l5.16 5.15c.2.2.2.5 0 .7Z\"/>") { } }

            public class ChevronDoubleRight : Icon { public ChevronDoubleRight() : base("ChevronDoubleRight", IconVariant.Regular, IconSize.Size20, "<path d=\"M8.65 4.15c.2-.2.5-.2.7 0l5.49 5.46c.21.22.21.57 0 .78l-5.49 5.46a.5.5 0 0 1-.7-.7L13.8 10 8.65 4.85a.5.5 0 0 1 0-.7Zm-4 0c.2-.2.5-.2.7 0l5.49 5.46c.21.22.21.57 0 .78l-5.49 5.46a.5.5 0 0 1-.7-.7L9.8 10 4.65 4.85a.5.5 0 0 1 0-.7Z\"/>") { } }

            public class Dismiss : Icon { public Dismiss() : base("Dismiss", IconVariant.Regular, IconSize.Size20, "<path d=\"m4.09 4.22.06-.07a.5.5 0 0 1 .63-.06l.07.06L10 9.29l5.15-5.14a.5.5 0 0 1 .63-.06l.07.06c.18.17.2.44.06.63l-.06.07L10.71 10l5.14 5.15c.18.17.2.44.06.63l-.06.07a.5.5 0 0 1-.63.06l-.07-.06L10 10.71l-5.15 5.14a.5.5 0 0 1-.63.06l-.07-.06a.5.5 0 0 1-.06-.63l.06-.07L9.29 10 4.15 4.85a.5.5 0 0 1-.06-.63l.06-.07-.06.07Z\"></path>") { } };

            public class Filter : Icon { public Filter() : base("Filter", IconVariant.Regular, IconSize.Size20, "<path d=\"M7.5 13h5a.5.5 0 0 1 .09 1H7.5a.5.5 0 0 1-.09-1h5.09-5Zm-2-4h9a.5.5 0 0 1 .09 1H5.5a.5.5 0 0 1-.09-1h9.09-9Zm-2-4h13a.5.5 0 0 1 .09 1H3.5a.5.5 0 0 1-.09-1H16.5h-13Z\"/>") { } }

            public class FilterDismiss : Icon { public FilterDismiss() : base("FilterDismiss", IconVariant.Regular, IconSize.Size20, "<path d=\"M9.2 7H2.42a.5.5 0 0 0 .09 1h7.1c-.16-.32-.3-.65-.4-1Zm2.8 8.5a.5.5 0 0 0-.5-.5H6.41a.5.5 0 0 0 .09 1h5.09a.5.5 0 0 0 .41-.5Zm1.5-4.5H4.41a.5.5 0 0 0 .09 1h9.09a.5.5 0 0 0-.09-1Zm1-1a4.5 4.5 0 1 0 0-9 4.5 4.5 0 0 0 0 9Zm1.85-6.35c.2.2.2.5 0 .7L15.21 5.5l1.14 1.15a.5.5 0 0 1-.7.7L14.5 6.21l-1.15 1.14a.5.5 0 0 1-.7-.7l1.14-1.15-1.14-1.15a.5.5 0 0 1 .7-.7l1.15 1.14 1.15-1.14c.2-.2.5-.2.7 0Z\"/>") { } }

            public class LineHorizontal3 : Icon { public LineHorizontal3() : base("LineHorizontal3", IconVariant.Regular, IconSize.Size20, "<path d=\"M2 4.5c0-.28.22-.5.5-.5h15a.5.5 0 0 1 0 1h-15a.5.5 0 0 1-.5-.5Zm0 5c0-.28.22-.5.5-.5h15a.5.5 0 0 1 0 1h-15a.5.5 0 0 1-.5-.5Zm.5 4.5a.5.5 0 0 0 0 1h15a.5.5 0 0 0 0-1h-15Z\"></path>") { } };

            public class QuestionCircle : Icon { public QuestionCircle() : base("QuestionCircle", IconVariant.Regular, IconSize.Size20, "<path d=\"M10 2a8 8 0 1 1 0 16 8 8 0 0 1 0-16Zm0 1a7 7 0 1 0 0 14 7 7 0 0 0 0-14Zm0 10.5a.75.75 0 1 1 0 1.5.75.75 0 0 1 0-1.5Zm0-8a2.5 2.5 0 0 1 1.65 4.38l-.15.12-.22.17-.09.07-.16.15c-.33.36-.53.85-.53 1.61a.5.5 0 0 1-1 0 3.2 3.2 0 0 1 1.16-2.62l.25-.19.12-.1A1.5 1.5 0 0 0 10 6.5c-.83 0-1.5.67-1.5 1.5a.5.5 0 0 1-1 0A2.5 2.5 0 0 1 10 5.5Z\"></path>") { } };

            public class Subtract : Icon { public Subtract() : base("Subtract", IconVariant.Regular, IconSize.Size20, "<path d=\"M3 10c0-.28.22-.5.5-.5h13a.5.5 0 0 1 0 1h-13A.5.5 0 0 1 3 10Z\"/>") { } }

            public class TableResizeColumn : Icon { public TableResizeColumn() : base("TableResizeColumn", IconVariant.Regular, IconSize.Size20, "<path d=\"M7.35 8.15c.2.2.2.5 0 .7l-.64.65h6.58l-.64-.65a.5.5 0 0 1 .7-.7l1.5 1.5c.2.2.2.5 0 .7l-1.5 1.5a.5.5 0 0 1-.7-.7l.64-.65H6.71l.64.65a.5.5 0 0 1-.7.7l-1.5-1.5a.5.5 0 0 1 0-.7l1.5-1.5c.2-.2.5-.2.7 0ZM17 6a3 3 0 0 0-3-3H6a3 3 0 0 0-3 3v8a3 3 0 0 0 3 3h8a3 3 0 0 0 3-3V6Zm-4-2v3c.36 0 .72.13 1 .38V4a2 2 0 0 1 2 2v8a2 2 0 0 1-2 2v-3.38a1.5 1.5 0 0 1-1 .38v3H7v-3a1.5 1.5 0 0 1-1-.38V16a2 2 0 0 1-2-2V6c0-1.1.9-2 2-2v3.38A1.5 1.5 0 0 1 7 7V4h6Z\"/>") { } }
        }
    }

    internal static partial class Regular
    {
        [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        internal static partial class Size24
        {
            public class ChevronLeft : Icon { public ChevronLeft() : base("ChevronLeft", IconVariant.Regular, IconSize.Size24, "<path d=\"M15.53 4.22c.3.3.3.77 0 1.06L8.81 12l6.72 6.72a.75.75 0 1 1-1.06 1.06l-7.25-7.25a.75.75 0 0 1 0-1.06l7.25-7.25c.3-.3.77-.3 1.06 0Z\"/>") { } }
            public class ChevronRight : Icon { public ChevronRight() : base("ChevronRight", IconVariant.Regular, IconSize.Size24, "<path d=\"M8.47 4.22c-.3.3-.3.77 0 1.06L15.19 12l-6.72 6.72a.75.75 0 1 0 1.06 1.06l7.25-7.25c.3-.3.3-.77 0-1.06L9.53 4.22a.75.75 0 0 0-1.06 0Z\"/>") { } }
        }
    }
    /// <summary>
    /// Filled icons
    /// </summary>
    internal static partial class Filled
    {
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        internal static partial class Size20
        {
            public class CheckmarkCircle : Icon { public CheckmarkCircle() : base("CheckmarkCircle", IconVariant.Filled, IconSize.Size20, "<path d=\"M10 2a8 8 0 1 1 0 16 8 8 0 0 1 0-16Zm3.36 5.65a.5.5 0 0 0-.64-.06l-.07.06L9 11.3 7.35 9.65l-.07-.06a.5.5 0 0 0-.7.7l.07.07 2 2 .07.06c.17.11.4.11.56 0l.07-.06 4-4 .07-.08a.5.5 0 0 0-.06-.63Z\"/>") { } }

            public class Info : Icon { public Info() : base("Info", IconVariant.Filled, IconSize.Size20, "<path d=\"M18 10a8 8 0 1 0-16 0 8 8 0 0 0 16 0ZM9.5 8.91a.5.5 0 0 1 1 0V13.6a.5.5 0 0 1-1 0V8.9Zm-.25-2.16a.75.75 0 1 1 1.5 0 .75.75 0 0 1-1.5 0Z\"/>") { } }

            public class Warning : Icon { public Warning() : base("Warning", IconVariant.Filled, IconSize.Size20, "<path d=\"M8.68 2.79a1.5 1.5 0 0 1 2.64 0l6.5 12A1.5 1.5 0 0 1 16.5 17h-13a1.5 1.5 0 0 1-1.32-2.21l6.5-12ZM10.5 7.5a.5.5 0 0 0-1 0v4a.5.5 0 0 0 1 0v-4Zm.25 6.25a.75.75 0 1 0-1.5 0 .75.75 0 0 0 1.5 0Z\"/>") { } }

            public class DismissCircle : Icon { public DismissCircle() : base("DismissCircle", IconVariant.Filled, IconSize.Size20, "<path d=\"M10 2a8 8 0 1 1 0 16 8 8 0 0 1 0-16ZM7.8 7.11a.5.5 0 0 0-.63.06l-.06.07a.5.5 0 0 0 .06.64L9.3 10l-2.12 2.12-.06.07a.5.5 0 0 0 .06.64l.07.06c.2.13.47.11.64-.06L10 10.7l2.12 2.12.07.06c.2.13.46.11.64-.06l.06-.07a.5.5 0 0 0-.06-.64L10.7 10l2.12-2.12.06-.07a.5.5 0 0 0-.06-.64l-.07-.06a.5.5 0 0 0-.64.06L10 9.3 7.88 7.17l-.07-.06Z\"></path>") { } }
        }
    }
}
