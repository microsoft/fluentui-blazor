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
        }
    }
}
