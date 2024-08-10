// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

public interface IMenuService : IDisposable
{
    /// <summary>
    /// Action to be called when the menu is updated.
    /// </summary>
    Action OnMenuUpdated { get; set; }

    /// <summary>
    /// Gets the list of menus.
    /// </summary>
    IEnumerable<FluentMenu> Menus { get; }

    /// <summary>
    /// Add a menu to the list.
    /// </summary>
    /// <param name="menu"></param>
    void Add(FluentMenu menu);

    /// <summary>
    /// Clear the list of menus.
    /// </summary>
    void Clear();

    /// <summary>
    /// Remove a menu from the list.
    /// </summary>
    /// <param name="menu"></param>
    void Remove(FluentMenu menu);

    /// <summary>
    /// Refresh the menu.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="isOpen"></param>
    Task RefreshMenuAsync(string id, bool isOpen);
}
