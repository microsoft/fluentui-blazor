// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

internal sealed class ViewportBreakpointSource : CascadingValueSource<GridItemSize?>
{
    private GridItemSize? _currentBreakpoint;

    public ViewportBreakpointSource()
        : base(name: FluentBreakpointProvider.CascadingParameterName, value: null, isFixed: false)
    {
    }

    internal Task SetBreakpointAsync(GridItemSize size)
    {
        if (!Enum.IsDefined(size))
        {
            throw new ArgumentOutOfRangeException(nameof(size), size, "A single viewport breakpoint is required.");
        }

        if (_currentBreakpoint == size)
        {
            return Task.CompletedTask;
        }

        _currentBreakpoint = size;
        return NotifyChangedAsync(size);
    }
}
