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
     * 
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
            public class LineHorizontal3 : Icon { public LineHorizontal3() : base("LineHorizontal3", IconVariant.Regular, IconSize.Size20, "<path d=\"M2 4.5c0-.28.22-.5.5-.5h15a.5.5 0 0 1 0 1h-15a.5.5 0 0 1-.5-.5Zm0 5c0-.28.22-.5.5-.5h15a.5.5 0 0 1 0 1h-15a.5.5 0 0 1-.5-.5Zm.5 4.5a.5.5 0 0 0 0 1h15a.5.5 0 0 0 0-1h-15Z\"></path>") { } };

            public class QuestionCircle : Icon { public QuestionCircle() : base("QuestionCircle", IconVariant.Regular, IconSize.Size20, "<path d=\"M10 2a8 8 0 1 1 0 16 8 8 0 0 1 0-16Zm0 1a7 7 0 1 0 0 14 7 7 0 0 0 0-14Zm0 10.5a.75.75 0 1 1 0 1.5.75.75 0 0 1 0-1.5Zm0-8a2.5 2.5 0 0 1 1.65 4.38l-.15.12-.22.17-.09.07-.16.15c-.33.36-.53.85-.53 1.61a.5.5 0 0 1-1 0 3.2 3.2 0 0 1 1.16-2.62l.25-.19.12-.1A1.5 1.5 0 0 0 10 6.5c-.83 0-1.5.67-1.5 1.5a.5.5 0 0 1-1 0A2.5 2.5 0 0 1 10 5.5Z\"></path>") { } };

            public class Dismiss : Icon { public Dismiss() : base("Dismiss", IconVariant.Regular, IconSize.Size20, "<path d=\"m4.09 4.22.06-.07a.5.5 0 0 1 .63-.06l.07.06L10 9.29l5.15-5.14a.5.5 0 0 1 .63-.06l.07.06c.18.17.2.44.06.63l-.06.07L10.71 10l5.14 5.15c.18.17.2.44.06.63l-.06.07a.5.5 0 0 1-.63.06l-.07-.06L10 10.71l-5.15 5.14a.5.5 0 0 1-.63.06l-.07-.06a.5.5 0 0 1-.06-.63l.06-.07L9.29 10 4.15 4.85a.5.5 0 0 1-.06-.63l.06-.07-.06.07Z\"></path>") { } };
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

            public class DismissCircle : Icon { public DismissCircle() : base("WaDismissCirclerning", IconVariant.Filled, IconSize.Size20, "<path d=\"M10 2a8 8 0 1 1 0 16 8 8 0 0 1 0-16ZM7.8 7.11a.5.5 0 0 0-.63.06l-.06.07a.5.5 0 0 0 .06.64L9.3 10l-2.12 2.12-.06.07a.5.5 0 0 0 .06.64l.07.06c.2.13.47.11.64-.06L10 10.7l2.12 2.12.07.06c.2.13.46.11.64-.06l.06-.07a.5.5 0 0 0-.06-.64L10.7 10l2.12-2.12.06-.07a.5.5 0 0 0-.06-.64l-.07-.06a.5.5 0 0 0-.64.06L10 9.3 7.88 7.17l-.07-.06Z\"></path>") { } }
        }
    }
}
