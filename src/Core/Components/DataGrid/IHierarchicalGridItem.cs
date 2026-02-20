// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// An interface for a grid item that can be part of a hierarchy.
/// </summary>
public interface IHierarchicalGridItem
{
    /// <summary>
    /// Gets or sets the depth of this item in the hierarchy (0 for root).
    /// </summary>
    int Depth { get; set; }

    /// <summary>
    /// Gets or sets whether this item is currently hidden (e.g. because a parent is collapsed).
    /// </summary>
    bool IsHidden { get; set; }

    /// <summary>
    /// Gets or sets whether this item's children are currently collapsed.
    /// </summary>
    bool IsCollapsed { get; set; }

    /// <summary>
    /// Gets a value indicating whether this item has children.
    /// </summary>
    bool HasChildren { get; }

    /// <summary>
    /// Gets or sets whether this item is currently selected.
    /// </summary>
    bool? IsSelected { get; set; }

    /// <summary>
    /// Gets the children of this item in the hierarchy.
    /// </summary>
    IEnumerable<IHierarchicalGridItem> Children { get; }
}
