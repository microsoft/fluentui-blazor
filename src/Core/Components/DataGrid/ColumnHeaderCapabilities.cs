// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Runtime.InteropServices;

namespace Microsoft.FluentUI.AspNetCore.Components;

[StructLayout(LayoutKind.Auto)]
internal readonly record struct ColumnHeaderCapabilities(
    bool CanSort,
    bool HasSortOptions,
    bool CanResize,
    bool CanReorder,
    bool HasOptions,
    bool HasHeaderPopupContent)
{
    public bool HasAnyAction => CanSort || CanResize || CanReorder || HasOptions;

    /// <summary>
    /// Gets whether the header offers anything besides sorting, which is what the sort actions are separated from
    /// when they are shown as a group.
    /// </summary>
    public bool HasActionsBesidesSort => CanResize || CanReorder || HasOptions;
}
