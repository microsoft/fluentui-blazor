// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Runtime.InteropServices;

namespace Microsoft.FluentUI.AspNetCore.Components;

[StructLayout(LayoutKind.Auto)]
internal readonly record struct ColumnHeaderCapabilities(
    bool CanSort,
    bool CanResize,
    bool CanReorder,
    bool HasOptions,
    bool HasHeaderPopupContent)
{
    public bool HasAnyAction => CanSort || CanResize || CanReorder || HasOptions;
}
