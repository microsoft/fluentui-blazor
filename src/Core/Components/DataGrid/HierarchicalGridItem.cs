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
    private bool? _isSelected = false;
    private bool _isCollapsed;

    /// <summary>
    /// Gets the children of this item in the hierarchy.
    /// </summary>
    IEnumerable<IHierarchicalGridItem> IHierarchicalGridItem.Children => Children;

    /// <summary>
    /// Gets or sets the reference to the item that holds this row's values.
    /// This name aligns with <see cref="FluentDataGridRow{TGridItem}.Item"/>.
    /// </summary>
    public required TItem Item { get; init; }

    /// <summary>
    /// Gets or sets the parent of this item in the hierarchy.
    /// </summary>
    public TGridItem? Parent { get; set; }

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
    /// Gets or sets whether this item is currently selected.
    /// </summary>
    public bool? IsSelected
    {
        get => _isSelected;
        set => SetIsSelected(value, updateChildren: true, updateParent: true);
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

    /// <summary>
    /// Sets the <see cref="IsSelected" /> property for this item.
    /// </summary>
    /// <param name="value">Selection state.</param>
    /// <param name="updateChildren">True to update all children.</param>
    /// <param name="updateParent">True to update the parent.</param>
    private void SetIsSelected(bool? value, bool updateChildren, bool updateParent)
    {
        if (_isSelected == value)
        {
            return;
        }

        _isSelected = value;

        if (updateChildren && value.HasValue)
        {
            foreach (var child in Children)
            {
                child.SetIsSelected(value, updateChildren: true, updateParent: false);
            }
        }

        if (updateParent)
        {
            Parent?.UpdateSelectionFromChildren();
        }
    }

    /// <summary>
    /// Updates the <see cref="IsSelected" /> property based on the children's state.
    /// </summary>
    private void UpdateSelectionFromChildren()
    {
        if (Children.Count == 0)
        {
            return;
        }

        var firstChildSelected = Children[0].IsSelected;
        if (Children.All(c => c.IsSelected == firstChildSelected))
        {
            SetIsSelected(firstChildSelected, updateChildren: false, updateParent: true);
        }
        else
        {
            SetIsSelected(value: null, updateChildren: false, updateParent: true);
        }
    }
}
