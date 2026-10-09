// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Maintains recursive tree selection and calculates checked, unchecked, and indeterminate states.
/// </summary>
/// <remarks>
/// Set <see cref="Items"/> to the tree's root items. All descendant data must be available;
/// this class does not load missing children.
/// Derive from this class to customize checkbox states or recursive selection behavior.
/// </remarks>
public class TreeViewThreeStateSelection
{
    private readonly FluentTreeView? _treeView;
    private Dictionary<ITreeViewItem, bool?> _states = [];
    private IEnumerable<ITreeViewItem>? _rootItems = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="TreeViewThreeStateSelection"/> class.
    /// </summary>
    public TreeViewThreeStateSelection()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TreeViewThreeStateSelection"/> class with the specified tree view.
    /// </summary>
    /// <param name="treeView"></param>
    internal TreeViewThreeStateSelection(FluentTreeView treeView)
    {
        _treeView = treeView;
    }

    /// <summary>
    /// Gets or sets the tree's root items used to recalculate ancestor selection.
    /// </summary>
    /// <remarks>
    /// Assigning this property invalidates cached checkbox states, even for the same collection.
    /// Call <see cref="Refresh"/> after modifying the collection or its descendants in place.
    /// </remarks>
    public IEnumerable<ITreeViewItem>? Items
    {
        get => _rootItems;
        set
        {
            _rootItems = value;
            Refresh();
        }
    }

    /// <summary>
    /// Gets the selected items or replaces the selection with a copy of the supplied items.
    /// Assigning <see langword="null"/> clears the selection.
    /// </summary>
    /// <remarks>
    /// The supplied collection is enumerated once during assignment and is not retained.
    /// Reassign this property to apply later changes to that collection.
    /// The getter enumerates selected items without duplicates by filtering the stored states.
    /// Assigning this property invalidates calculated checkbox states.
    /// </remarks>
    public IEnumerable<ITreeViewItem>? SelectedItems
    {
        get => _states.Where(entry => entry.Value == true).Select(entry => entry.Key);
        set
        {
            var states = new Dictionary<ITreeViewItem, bool?>();
            foreach (var item in value ?? [])
            {
                states[item] = true;
            }

            _states = states;
        }
    }

    /// <summary>
    /// Gets whether the owning tree controls checkbox states.
    /// </summary>
    internal bool HasCheckState => IsRecursive
                                || (_treeView?.SelectionMode == TreeSelectionMode.Multiple && _treeView?.CheckState is not null);

    /// <summary>
    /// Gets whether the owning tree uses recursive multiple selection.
    /// </summary>
    internal bool IsRecursive => _treeView?.SelectionMode == TreeSelectionMode.MultipleRecursive;

    /// <summary>
    /// Gets the checkbox state of an item using the recursive selection maintained by <see cref="OnSelectedItemsChanged"/>.
    /// </summary>
    /// <param name="item">The item whose state is evaluated.</param>
    /// <returns>
    /// <see langword="true"/> if the item is selected, <see langword="null"/> if only descendants
    /// are selected, or <see langword="false"/> if neither the item nor its descendants are selected.
    /// </returns>
    /// <remarks>
    /// This method does not modify the selection or load missing descendants.
    /// Use it with <see cref="FluentTreeView.CheckState"/> when all descendant data is available.
    /// Node states are cached alongside the selection until <see cref="Items"/> or
    /// <see cref="SelectedItems"/> is assigned, or <see cref="Refresh"/> is called.
    /// </remarks>
    public virtual bool? GetCheckState(ITreeViewItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (_treeView is not null && !IsRecursive)
        {
            return _treeView.CheckState is null
                ? _treeView.SelectedItems?.Contains(item) == true
                : _treeView.CheckState(item);
        }

        if (_states.TryGetValue(item, out var state))
        {
            return state;
        }

        state = item.Items?.Any(child => GetCheckState(child) != false) == true
            ? null
            : false;

        _states.Add(item, state);
        return state;
    }

    /// <summary>
    /// Invalidates calculated checkbox states after in-place changes to the tree, preserving the selection.
    /// </summary>
    /// <remarks>
    /// States are recalculated on subsequent calls to <see cref="GetCheckState(ITreeViewItem)"/>.
    /// This method does not modify the selection or request a component render.
    /// </remarks>
    public virtual void Refresh()
    {
        foreach (var entry in _states)
        {
            if (entry.Value != true)
            {
                _states.Remove(entry.Key);
            }
        }
    }

    /// <summary>
    /// Applies a checkbox selection change recursively and recalculates the selection of ancestors.
    /// </summary>
    /// <param name="newSelectedItems">The selection supplied by <see cref="FluentTreeView.SelectedItemsChanged"/>, or <see langword="null"/> to clear the selection.</param>
    /// <remarks>
    /// Replaces <see cref="SelectedItems"/> with a new selection containing selected descendants
    /// and fully selected ancestors, without duplicates. The input collections are not modified.
    /// All descendant data must be available; this method does not load missing children.
    /// Use this method with <see cref="FluentTreeView.SelectedItemsChanged"/> to opt into recursive selection.
    /// </remarks>
    public virtual void OnSelectedItemsChanged(IEnumerable<ITreeViewItem>? newSelectedItems)
    {
        var selection = newSelectedItems?.ToHashSet() ?? [];
        var added = selection.Where(item => !_states.TryGetValue(item, out var state) || state != true).ToArray();
        var removed = (SelectedItems ?? []).Where(item => !selection.Contains(item)).ToArray();

        foreach (var item in added)
        {
            SetSelected(item, true, selection);
        }

        foreach (var item in removed)
        {
            SetSelected(item, false, selection);
        }

        foreach (var item in Items ?? [])
        {
            UpdateParentSelection(item, selection);
        }

        SelectedItems = selection;
    }

    /// <summary>
    /// Adds or removes an item and its descendants from the working selection.
    /// </summary>
    /// <param name="item">The item to update.</param>
    /// <param name="selected">Whether to select or deselect the item.</param>
    /// <param name="selection">The working selection, applied when the change completes.</param>
    protected virtual void SetSelected(ITreeViewItem item, bool selected, ISet<ITreeViewItem> selection)
    {
        if (selected)
        {
            selection.Add(item);
        }
        else
        {
            selection.Remove(item);
        }

        foreach (var child in item.Items ?? [])
        {
            SetSelected(child, selected, selection);
        }
    }

    /// <summary>
    /// Recalculates selection from the descendants up, selecting parents whose children are all selected.
    /// </summary>
    /// <param name="item">The root of the subtree to recalculate.</param>
    /// <param name="selection">The working selection, applied when the change completes.</param>
    protected virtual void UpdateParentSelection(ITreeViewItem item, ISet<ITreeViewItem> selection)
    {
        var children = item.Items?.ToArray() ?? [];
        if (children.Length == 0)
        {
            return;
        }

        foreach (var child in children)
        {
            UpdateParentSelection(child, selection);
        }

        if (children.All(selection.Contains))
        {
            selection.Add(item);
        }
        else
        {
            selection.Remove(item);
        }
    }
}