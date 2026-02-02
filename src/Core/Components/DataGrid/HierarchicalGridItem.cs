// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A base class for a grid item that can be part of a hierarchy.
/// </summary>
/// <typeparam name="TItem">The type of the item being displayed.</typeparam>
/// <typeparam name="TGridItem">The type of the grid item wrapper class.</typeparam>
[DebuggerDisplay("Item = {Item}, Children = {Children.Count}, Depth = {Depth}, IsHidden = {IsHidden}")]
public class HierarchicalGridItem<TItem, TGridItem> : IHierarchicalGridItem
    where TItem : notnull
    where TGridItem : HierarchicalGridItem<TItem, TGridItem>
{
    /// <summary>
    /// Gets or sets the reference to the item that holds this row's values.
    /// This name aligns with <see cref="FluentDataGridRow{TGridItem}.Item"/>.
    /// </summary>
    public required TItem Item { get; init; }

    /// <summary>
    /// The children of this item in the hierarchy.
    /// </summary>
    public IList<TGridItem> Children { get; } = [];

    /// <summary>
    /// Gets a value indicating whether this item has children.
    /// </summary>
    public bool HasChildren => Children.Count > 0;

    /// <summary>
    /// The depth of this item in the hierarchy (0 for root).
    /// </summary>
    public int Depth { get; set; }

    /// <summary>
    /// Whether this item is currently hidden (e.g. because a parent is collapsed).
    /// </summary>
    public bool IsHidden { get; set; }

    private bool _isCollapsed;

    /// <summary>
    /// Whether this item's children are currently collapsed.
    /// </summary>
    public bool IsCollapsed
    {
        get => _isCollapsed;
        set
        {
            _isCollapsed = value;
            UpdateHidden();
        }
    }

    /// <summary>
    /// Updates the <see cref="IsHidden" /> property for this item's children.
    /// </summary>
    private void UpdateHidden()
    {
        var shouldHideChildren = IsHidden || IsCollapsed;
        foreach (var child in Children)
        {
            child.IsHidden = shouldHideChildren;
            child.UpdateHidden();
        }
    }
}
