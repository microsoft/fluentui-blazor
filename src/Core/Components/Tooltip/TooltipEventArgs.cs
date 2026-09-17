// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Event arguments for the FluentTooltip component.
/// </summary>
public class TooltipEventArgs : EventArgs
{
    /// <summary />
    internal TooltipEventArgs(string id, bool opened)
    {
        Id = id;
        Opened = opened;
    }

    /// <summary>
    /// Gets the component identifier associated with the tooltip.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets a value indicating whether the tooltip is opened or closed.
    /// </summary>
    public bool Opened { get; }
}
