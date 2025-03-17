// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
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
