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
/// </remarks>
public class TreeViewThreeStateSelection
{
    /// <summary>
    /// Gets or sets the tree's root items used to recalculate ancestor selection.
    /// </summary>
    public IEnumerable<ITreeViewItem>? Items { get; set; } = [];

    /// <summary>
    /// Gets or sets the selected items. A <see langword="null"/> value represents an empty selection.
    /// </summary>
    public IEnumerable<ITreeViewItem>? SelectedItems { get; set; } = [];

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
    /// </remarks>
    public bool? GetCheckState(ITreeViewItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        var selection = SelectedItems?.ToHashSet() ?? [];
        return GetState(item);

        bool? GetState(ITreeViewItem current)
        {
            if (selection.Contains(current))
            {
                return true;
            }

            return current.Items?.Any(child => GetState(child) != false) == true
                ? null
                : false;
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
    public void OnSelectedItemsChanged(IEnumerable<ITreeViewItem>? newSelectedItems)
    {
        var previousSelection = SelectedItems?.ToHashSet() ?? [];
        var selection = newSelectedItems?.ToHashSet() ?? [];
        var added = selection.Except(previousSelection).ToArray();
        var removed = previousSelection.Except(selection).ToArray();

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

    private static void SetSelected(ITreeViewItem item, bool selected, HashSet<ITreeViewItem> selection)
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

    private static void UpdateParentSelection(ITreeViewItem item, HashSet<ITreeViewItem> selection)
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