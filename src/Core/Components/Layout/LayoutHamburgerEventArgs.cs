// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// 
/// </summary>
public class LayoutHamburgerEventArgs
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="opened"></param>
    public LayoutHamburgerEventArgs(string id, bool opened)
    {
        Id = id;
        Opened = opened;
    }

    /// <summary>
    /// Gets the ID of the hamburger menu.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets whether the hamburger menu is opened or closed.
    /// </summary>
    public bool Opened { get; }
}
