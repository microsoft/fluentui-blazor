// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a <see cref="FluentDataGrid{TGridItem}"/> column that combines hierarchical indentation,
/// expand/collapse toggles, a selection checkbox/radio, and user-defined template content.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
public class HierarchicalSelectColumn<TGridItem> : SelectColumn<TGridItem>
{
    private bool _isSyncing; // Add this private field

    /// <summary>
    /// Initializes a new instance of <see cref="HierarchicalSelectColumn{TGridItem}"/>.
    /// </summary>
    public HierarchicalSelectColumn()
    {
        HierarchicalToggle = true;
        Width = "auto";
        MinWidth = "150px";
        SelectMode = DataGridSelectMode.Multiple;
        Property = (item) => item is IHierarchicalGridItem { IsSelected: true };
    }

    /// <summary>
    /// Gets or sets the list of items in an indeterminate state.
    /// </summary>
    [Parameter]
    public IEnumerable<TGridItem> IndeterminateItems { get; set; } = [];

    /// <summary>
    /// Gets or sets a callback when list of indeterminate items changed.
    /// </summary>
    [Parameter]
    public EventCallback<IEnumerable<TGridItem>> IndeterminateItemsChanged { get; set; }

    /// <summary>
    /// Gets or sets the list of items in an unselected state.
    /// </summary>
    [Parameter]
    public IEnumerable<TGridItem> UnselectedItems { get; set; } = [];

    /// <summary>
    /// Gets or sets a callback when list of unselected items changed.
    /// </summary>
    [Parameter]
    public EventCallback<IEnumerable<TGridItem>> UnselectedItemsChanged { get; set; }

    /// <inheritdoc />
    public override void ClearSelection()
    {
        base.ClearSelection();

        if (IndeterminateItemsChanged.HasDelegate)
        {
            _ = IndeterminateItemsChanged.InvokeAsync(IndeterminateItems);
        }

        if (UnselectedItemsChanged.HasDelegate)
        {
            _ = UnselectedItemsChanged.InvokeAsync(UnselectedItems);
        }
    }

    /// <inheritdoc />
    public override async Task ClearSelectionAsync()
    {
        await base.ClearSelectionAsync();

        if (IndeterminateItemsChanged.HasDelegate)
        {
            await IndeterminateItemsChanged.InvokeAsync(IndeterminateItems);
        }

        if (UnselectedItemsChanged.HasDelegate)
        {
            await UnselectedItemsChanged.InvokeAsync(UnselectedItems);
        }
    }

    /// <inheritdoc />
    protected internal override void CellContent(RenderTreeBuilder builder, TGridItem item)
    {
        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "style", "display: flex; align-items: center; gap: 8px;");

        // 1. Selection Icon (Checkbox)
        // We reuse the base icon rendering logic which also handles selection state sync.
        builder.AddContent(2, GetDefaultChildContent()(item));

        // 2. User Template Content
        if (ChildContent is not null)
        {
            builder.AddContent(3, ChildContent(item));
        }

        builder.CloseElement();
    }

    /// <inheritdoc />
    protected override async Task AddOrRemoveSelectedItemAsync(TGridItem? item)
    {
        if (item is IHierarchicalGridItem hierarchicalItem)
        {
            // Toggle logic: false or indeterminate (null) -> true, true -> false
            bool? isSelected = hierarchicalItem.IsSelected != true;

            try
            {
                _isSyncing = true;

                // Update state. If using HierarchicalGridItem, this propagates up and down.
                hierarchicalItem.IsSelected = isSelected;

                // If it's a custom implementation that doesn't propagate, we perform manual sync
                SyncSelectedItemsFromHierarchy();
            }
            finally
            {
                _isSyncing = false;
            }

            if (SelectAllChanged.HasDelegate)
            {
                await SelectAllChanged.InvokeAsync(SelectAll);
            }

            if (SelectedItemsChanged.HasDelegate)
            {
                await SelectedItemsChanged.InvokeAsync(SelectedItems);
            }

            if (IndeterminateItemsChanged.HasDelegate)
            {
                await IndeterminateItemsChanged.InvokeAsync(IndeterminateItems);
            }

            if (UnselectedItemsChanged.HasDelegate)
            {
                await UnselectedItemsChanged.InvokeAsync(UnselectedItems);
            }

            // Force a grid-wide refresh so all affected rows are re-rendered
            await Grid.RefreshDataAsync();
        }
        else
        {
            await base.AddOrRemoveSelectedItemAsync(item);
        }
    }

    /// <inheritdoc />
    protected override async Task UpdateSelectedItemsAsync()
    {
        if (InternalGridContext?.Items == null || InternalGridContext.Items.Count == 0)
        {
            return;
        }

        var oldSelected = SelectedItems;
        var oldIndeterminate = IndeterminateItems;
        var oldUnselected = UnselectedItems;

        // Perform hierarchical sync without calling base reconciler (which avoids infinite loops)
        try
        {
            _isSyncing = true;
            SyncSelectedItemsFromHierarchy();
        }
        finally
        {
            _isSyncing = false;
        }

        // Notify bindings that changed during the sync
        if (!ReferenceEquals(oldUnselected, UnselectedItems) && UnselectedItemsChanged.HasDelegate)
        {
            await UnselectedItemsChanged.InvokeAsync(UnselectedItems);
        }

        if (!ReferenceEquals(oldIndeterminate, IndeterminateItems) && IndeterminateItemsChanged.HasDelegate)
        {
            await IndeterminateItemsChanged.InvokeAsync(IndeterminateItems);
        }

        if (!ReferenceEquals(oldSelected, SelectedItems) && SelectedItemsChanged.HasDelegate)
        {
            await SelectedItemsChanged.InvokeAsync(SelectedItems);
        }
    }

    /// <summary />
    protected override RenderFragment GetHeaderContent()
    {
        var selectedAll = GetSelectAll();
        var iconAllChecked = (selectedAll == null && IconIndeterminate != null)
                            ? IconIndeterminate
                            : GetIcon(selectedAll);

        return new RenderFragment((builder) =>
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "style", "display: flex; padding: 0 34px;");
            builder.OpenComponent<FluentIcon<Icon>>(2);
            builder.AddAttribute(3, "Value", iconAllChecked);
            builder.AddAttribute(4, "all-selected", SelectAll);
            if (!SelectAllDisabled)
            {
                builder.AddAttribute(5, "OnClick", EventCallback.Factory.Create<MouseEventArgs>(this, OnClickAllAsync));
                builder.AddAttribute(6, "onkeydown", EventCallback.Factory.Create<KeyboardEventArgs>(this, OnKeyAllAsync));
            }

            builder.AddAttribute(7, "Title", iconAllChecked == IconIndeterminate
                                                ? TitleAllIndeterminate
                                                : (iconAllChecked == GetIcon(selected: true) ? TitleAllChecked : TitleAllUnchecked));
            builder.CloseComponent();

            builder.OpenElement(8, "div");
            builder.AddAttribute(9, "class", "col-title");
            builder.AddAttribute(10, "style", "padding-left: 6px;");
            builder.OpenElement(11, "div");
            builder.AddAttribute(12, "class", "col-title-text");
            builder.AddAttribute(13, "title", HeaderTooltip);
            builder.AddContent(14, HeaderTitleContent);
            builder.CloseElement();
            builder.CloseElement();
            builder.CloseElement();

        });

    }

    /// <inheritdoc />
    protected override async Task OnClickAllAsync(MouseEventArgs e)
    {
        if (Grid == null || SelectMode != DataGridSelectMode.Multiple || SelectAllDisabled)
        {
            return;
        }

        // Toggle logic: false or indeterminate (null) -> true, true -> false
        SelectAll = !(SelectAll == true);

        if (SelectAllChanged.HasDelegate)
        {
            await SelectAllChanged.InvokeAsync(SelectAll);
        }

        try
        {
            _isSyncing = true;

            // Apply to all root items in InternalGridContext.Items
            foreach (var item in InternalGridContext.Items)
            {
                if (item is IHierarchicalGridItem h && h.Depth == 0)
                {
                    h.IsSelected = SelectAll;
                }
            }

            // Sync the _selectedItems list to match the new state
            SyncSelectedItemsFromHierarchy();
        }
        finally
        {
            _isSyncing = false;
        }

        if (SelectedItemsChanged.HasDelegate)
        {
            await SelectedItemsChanged.InvokeAsync(SelectedItems);
        }

        if (IndeterminateItemsChanged.HasDelegate)
        {
            await IndeterminateItemsChanged.InvokeAsync(IndeterminateItems);
        }

        if (UnselectedItemsChanged.HasDelegate)
        {
            await UnselectedItemsChanged.InvokeAsync(UnselectedItems);
        }

        // Force a grid-wide refresh so all affected rows are re-rendered
        await Grid.RefreshDataAsync();
    }

    /// <inheritdoc />
    protected override async Task OnKeyAllAsync(KeyboardEventArgs e)
    {
        if (KEYBOARD_SELECT_KEYS.Contains(e.Code, StringComparer.OrdinalIgnoreCase))
        {
            await OnClickAllAsync(new MouseEventArgs());
        }
    }

    /// <inheritdoc />
    protected override bool? GetSelectAll()
    {
        if (InternalGridContext?.Items == null || InternalGridContext.Items.Count == 0)
        {
            return false;
        }

        var hasSelected = false;
        var hasUnselected = false;
        var hasIndeterminate = false;

        foreach (var item in InternalGridContext.Items)
        {
            if (item is IHierarchicalGridItem h && h.Depth <= 0)
            {
                if (h.IsSelected == true)
                {
                    hasSelected = true;
                }
                else if (h.IsSelected == false)
                {
                    hasUnselected = true;
                }
                else
                {
                    hasIndeterminate = true;
                }
            }
            else if (item is not IHierarchicalGridItem)
            {
                var selectionStatus = SelectedItems.Contains(item, Comparer) || (Property?.Invoke(item) ?? false);
                if (selectionStatus)
                {
                    hasSelected = true;
                }
                else
                {
                    hasUnselected = true;
                }
            }
        }

        if (hasIndeterminate || (hasSelected && hasUnselected))
        {
            return null;
        }

        return hasSelected;
    }

    /// <inheritdoc />
    protected override Icon GetIcon(bool? selected)
    {
        if (selected == null && IconIndeterminate != null)
        {
            return IconIndeterminate;
        }

        return base.GetIcon(selected);
    }

    /// <inheritdoc />

    [SuppressMessage("Design", "MA0051:Method is too long", Justification = "This rule needs to be changed to 75")]
    protected override RenderFragment<TGridItem> GetDefaultChildContent()
    {
        return (item) => new RenderFragment((builder) =>
        {
            if (Selectable != null && !Selectable.Invoke(item))
            {
                return;
            }

            var selected = item is IHierarchicalGridItem h ? h.IsSelected : (SelectedItems.Contains(item, Comparer) || (Property?.Invoke(item) ?? false));
            var contained = SelectedItems.Contains(item, Comparer);

            // Sync with SelectedItems list
            if (selected == true && !contained)
            {
                var selectedItems = SelectedItems.ToList();
                selectedItems.Add(item);
                SelectedItems = selectedItems;
            }
            else if (selected != true && contained)
            {
                var selectedItems = SelectedItems.ToList();
                selectedItems.Remove(item);
                SelectedItems = selectedItems;
            }

            builder.OpenComponent<FluentIcon<Icon>>(0);
            builder.AddAttribute(1, "Value", GetIcon(selected));
            builder.AddAttribute(2, "Title", selected == true ? TitleChecked : TitleUnchecked);
            builder.AddAttribute(3, "row-selected", selected == true);

            if (!SelectFromEntireRow)
            {
                builder.AddAttribute(4, "style", "cursor: pointer;");
            }

            builder.CloseComponent();
        });
    }

    /// <inheritdoc />
    protected override void OnSelectedItemsSet()
    {
        if (_isSyncing)
        {
            return;
        }

        try
        {
            _isSyncing = true;
            SyncHierarchyFromSelectedItems();
            SyncSelectedItemsFromHierarchy();
        }
        finally
        {
            _isSyncing = false;
        }
    }

    private void SyncHierarchyFromSelectedItems()
    {
        if (InternalGridContext?.Items == null)
        {
            return;
        }

        var selectedItems = SelectedItems;
        var indeterminateItems = IndeterminateItems;
        foreach (var item in InternalGridContext.Items)
        {
            if (item is IHierarchicalGridItem h && h.Depth == 0)
            {
                SyncItemFromSelectedRecursive(item, selectedItems, indeterminateItems);
            }
        }
    }

    private void SyncItemFromSelectedRecursive(TGridItem item, IEnumerable<TGridItem> selectedItems, IEnumerable<TGridItem> indeterminateItems)
    {
        if (item is IHierarchicalGridItem h)
        {
            bool? targetState = false;
            if (selectedItems.Contains(item, Comparer))
            {
                targetState = true;
            }
            else if (indeterminateItems.Contains(item, Comparer))
            {
                targetState = null;
            }

            if (h.IsSelected != targetState)
            {
                h.IsSelected = targetState;
            }

            foreach (var child in h.Children)
            {
                if (child is TGridItem gridChild)
                {
                    SyncItemFromSelectedRecursive(gridChild, selectedItems, indeterminateItems);
                }
            }
        }
    }

    private void SyncSelectedItemsFromHierarchy()
    {
        if (InternalGridContext?.Items == null)
        {
            return;
        }

        List<TGridItem> selectedItems = [];
        List<TGridItem> indeterminateItems = [];
        List<TGridItem> unselectedItems = [];

        foreach (var item in InternalGridContext.Items)
        {
            // Only start recursion from root items (or all items if not hierarchical)
            if (item is not IHierarchicalGridItem h || h.Depth == 0)
            {
                SyncItemRecursive(item, selectedItems, indeterminateItems, unselectedItems);
            }
        }

        if (!unselectedItems.SequenceEqual(UnselectedItems, Comparer))
        {
            UnselectedItems = unselectedItems;
        }

        if (!indeterminateItems.SequenceEqual(IndeterminateItems, Comparer))
        {
            IndeterminateItems = indeterminateItems;
        }

        if (!selectedItems.SequenceEqual(SelectedItems, Comparer))
        {
            SelectedItems = selectedItems;
        }

        RefreshHeaderContent();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0051:Method is too long", Justification = "<Pending>")]
    private bool? SyncItemRecursive(TGridItem item, List<TGridItem> selectedItems, List<TGridItem> indeterminateItems, List<TGridItem> unselectedItems)
    {
        bool? isSelected;
        if (item is IHierarchicalGridItem hierarchicalItem)
        {
            if (hierarchicalItem.HasChildren)
            {
                var hasSelected = false;
                var hasUnselected = false;
                var hasIndeterminate = false;

                foreach (var child in hierarchicalItem.Children)
                {
                    if (child is TGridItem gridChild)
                    {
                        var childState = SyncItemRecursive(gridChild, selectedItems, indeterminateItems, unselectedItems);
                        if (childState == true)
                        {
                            hasSelected = true;
                        }
                        else if (childState == false)
                        {
                            hasUnselected = true;
                        }
                        else
                        {
                            hasIndeterminate = true;
                        }
                    }
                }

                if (hasIndeterminate || (hasSelected && hasUnselected))
                {
                    isSelected = null;
                }
                else
                {
                    isSelected = hasSelected;
                }

                if (hierarchicalItem.IsSelected != isSelected)
                {
                    hierarchicalItem.IsSelected = isSelected;
                }
            }
            else
            {
                isSelected = hierarchicalItem.IsSelected;
            }

            if (isSelected == true)
            {
                if (!selectedItems.Contains(item, Comparer))
                {
                    selectedItems.Add(item);
                }
            }
            else if (isSelected == null)
            {
                if (!indeterminateItems.Contains(item, Comparer))
                {
                    indeterminateItems.Add(item);
                }
            }
            else
            {
                if (!unselectedItems.Contains(item, Comparer))
                {
                    unselectedItems.Add(item);
                }
            }
        }
        else
        {
            isSelected = Property?.Invoke(item) ?? false;
            if (isSelected == true)
            {
                if (!selectedItems.Contains(item, Comparer))
                {
                    selectedItems.Add(item);
                }
            }
            else
            {
                if (!unselectedItems.Contains(item, Comparer))
                {
                    unselectedItems.Add(item);
                }
            }
        }

        return isSelected;
    }
}
