// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Infrastructure;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a <see cref="FluentDataGrid{TGridItem}"/> column whose cells render a selected checkbox updated when the user click on a row.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
public class SelectColumn<TGridItem> : ColumnBase<TGridItem>, IDisposable
{
    /// <summary>
    /// List of keys to press, to select/unselect a row.
    /// </summary>

    internal static readonly string[] KEYBOARD_SELECT_KEYS = ["Enter", "NumpadEnter"];

    private readonly Icon IconUnselectedMultiple = new CoreIcons.Regular.Size20.CheckboxUnchecked();
    private readonly Icon IconSelectedMultiple = new CoreIcons.Filled.Size20.CheckboxChecked().WithColor(Color.Primary);
    private readonly Icon IconUnselectedSingle = new CoreIcons.Regular.Size20.RadioButton();
    private readonly Icon IconSelectedSingle = new CoreIcons.Filled.Size20.RadioButton().WithColor(Color.Primary);

    private DataGridSelectMode _selectMode = DataGridSelectMode.Single;
    private readonly List<TGridItem> _selectedItems = [];

    private readonly EventCallbackSubscriber<object?> _itemsChanged;

    /// <summary>
    /// Initializes a new instance of <see cref="SelectColumn{TGridItem}"/>.
    /// </summary>
    public SelectColumn()
    {
        Width = "50px";
        ChildContent = GetDefaultChildContent();

        _itemsChanged = new(EventCallback.Factory.Create<object?>(this, UpdateSelectedItemsAsync));
    }

    /// <summary />
    protected override void OnInitialized()
    {
        _itemsChanged.SubscribeOrMove(InternalGridContext.ItemsChanged);
        base.OnInitialized();
    }

    /// <summary>
    /// Gets or sets the content to be rendered for each row in the table.
    /// </summary>
    [Parameter]
    public RenderFragment<TGridItem> ChildContent { get; set; }

    /// <summary>
    /// Gets or sets whether the [All] checkbox is disabled (not clickable).
    /// </summary>
    [Parameter]
    public bool SelectAllDisabled { get; set; } = false;

    /// <summary>
    /// Gets or sets whether the selection of rows is restricted to the SelectColumn (false) or if the whole row can be clicked to toggled the selection (true).
    /// Default is True.
    /// </summary>
    [Parameter]
    public bool SelectFromEntireRow { get; set; } = true;

    /// <summary>
    /// Gets or sets the template for the [All] checkbox column template.
    /// </summary>
    [Parameter]
    public RenderFragment<SelectAllTemplateArgs>? SelectAllTemplate { get; set; }

    /// <summary>
    /// Gets or sets the list of selected items.
    /// </summary>
    [Parameter]
#pragma warning disable BL0007 // Component parameters should be auto properties
    public IEnumerable<TGridItem> SelectedItems
#pragma warning restore BL0007 // Component parameters should be auto properties
    {
        get => _selectedItems;
        set
        {
            if (_selectedItems != value)
            {
                _selectedItems.Clear();
                _selectedItems.AddRange(value);
                SelectAll = false;
            }
        }
    }

    /// <summary>
    /// Gets or sets a callback when list of selected items changed.
    /// </summary>
    [Parameter]
    public EventCallback<IEnumerable<TGridItem>> SelectedItemsChanged { get; set; }

    /// <summary>
    /// Gets or sets the selection mode (Single, SingleSticky or Multiple).
    /// </summary>
    [Parameter]
#pragma warning disable BL0007 // Component parameters should be auto properties
    public DataGridSelectMode SelectMode
#pragma warning restore BL0007 // Component parameters should be auto properties
    {
        get => _selectMode;
        set
        {
            _selectMode = value;

            if (value is DataGridSelectMode.Single or DataGridSelectMode.SingleSticky)
            {
                _ = KeepOnlyFirstSelectedItemAsync();
            }

            RefreshHeaderContent();
        }
    }

    /// <summary>
    /// Gets or sets the Icon to be rendered when the row is non selected.
    /// </summary>
    [Parameter]
    public Icon? IconUnchecked { get; set; }

    /// <summary>
    /// Gets or sets the Icon title display as a tooltip and used with Accessibility.
    /// The default text is "Row unselected".
    /// </summary>
    [Parameter]
    public string TitleUnchecked { get; set; } = "Row unselected";

    /// <summary>
    /// Gets or sets the Icon to be rendered when the row is selected.
    /// </summary>
    [Parameter]
    public Icon? IconChecked { get; set; }

    /// <summary>
    /// Gets or sets the Icon title display as a tooltip and used with Accessibility.
    /// The default text is "Row selected".
    /// </summary>
    [Parameter]
    public string TitleChecked { get; set; } = "Row selected.";

    /// <summary>
    /// Gets or sets the Icon to be rendered when some but not all rows are selected.
    /// Only when <see cref="SelectMode"/> is Multiple.
    /// </summary>
    [Parameter]
    public Icon? IconIndeterminate { get; set; } = new CoreIcons.Filled.Size20.CheckboxIndeterminate().WithColor(Color.Primary);

    /// <summary>
    /// Gets or sets the Icon title display as a tooltip and used with Accessibility.
    /// The default text is "All rows are selected.".
    /// </summary>
    [Parameter]
    public string TitleAllChecked { get; set; } = "All rows are selected.";

    /// <summary>
    /// Gets or sets the Icon title display as a tooltip and used with Accessibility.
    /// The default text is "No rows are selected.".
    /// </summary>
    [Parameter]
    public string TitleAllUnchecked { get; set; } = "No rows are selected.";

    /// <summary>
    /// Gets or sets the Icon title display as a tooltip and used with Accessibility.
    /// The default text is "Some rows are selected.".
    /// </summary>
    [Parameter]
    public string TitleAllIndeterminate { get; set; } = "Some rows are selected.";

    /// <summary>
    /// Gets or sets the action to be executed when the row is selected or unselected.
    /// This action is required to update you data model.
    /// </summary>
    [Parameter]
    public EventCallback<(TGridItem Item, bool Selected)> OnSelect { get; set; }

    /// <summary>
    /// Gets or sets the value indicating whether the [All] checkbox is selected.
    /// Null is undefined.
    /// </summary>
    [Parameter]
    public bool? SelectAll { get; set; } = false;

    /// <summary>
    /// Gets or sets the action to be executed when the [All] checkbox is clicked.
    /// When this action is defined, the [All] checkbox is displayed.
    /// This action is required to update you data model.
    /// </summary>
    [Parameter]
    public EventCallback<bool?> SelectAllChanged { get; set; }

    /// <summary>
    /// Gets or sets the function executed to determine if the item can be selected.
    /// </summary>
    [Parameter]
    public Func<TGridItem, bool>? Selectable { get; set; }

    /// <summary>
    /// Gets or sets the function to executed to determine checked/unchecked status.
    /// </summary>
    [Parameter]
    public Func<TGridItem, bool> Property { get; set; } = (item) => false;

    /// <inheritdoc />
    [Parameter]
    public override IGridSort<TGridItem>? SortBy { get; set; } = null;

    /// <summary>
    /// Allows to clear the selection.
    /// </summary>
    public void ClearSelection()
    {
        _selectedItems.Clear();
        RefreshHeaderContent();
    }

    /// <summary>
    /// Allows to clear the selection.
    /// </summary>
    public async Task ClearSelectionAsync()
    {
#pragma warning disable MA0042 // Do not use blocking calls in an async method
        ClearSelection();
#pragma warning restore MA0042 // Do not use blocking calls in an async method
        await Task.CompletedTask;
    }

    /// <summary>
    /// Select on Unselect an item when the row is clicked.
    /// </summary>
    /// <param name="row"></param>
    /// <returns></returns>
    protected internal override Task OnRowClickAsync(FluentDataGridRow<TGridItem> row)
    {
        if (SelectFromEntireRow && row.RowType == DataGridRowType.Default)
        {
            return AddOrRemoveSelectedItemAsync(row.Item);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Select on Unselect an item when the navigation keys are pressed.
    /// </summary>
    /// <param name="row"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    protected internal override Task OnRowKeyDownAsync(FluentDataGridRow<TGridItem> row, KeyboardEventArgs args)
    {
        if (SelectFromEntireRow && row.RowType == DataGridRowType.Default)
        {
            return AddOrRemoveSelectedItemAsync(row.Item);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Select on Unselect an item when the cell is clicked.
    /// </summary>
    /// <param name="cell"></param>
    /// <returns></returns>
    protected internal override Task OnCellClickAsync(FluentDataGridCell<TGridItem> cell)
    {
        // If the cell is a checkbox cell, add or remove the item from the selected items list.
        if (!SelectFromEntireRow && cell.CellType == DataGridCellType.Default)
        {
            return AddOrRemoveSelectedItemAsync(cell.Item);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Select on Unselect an item when the navigation keys are pressed.
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    protected internal override Task OnCellKeyDownAsync(FluentDataGridCell<TGridItem> cell, KeyboardEventArgs args)
    {
        // If the cell is a checkbox cell, add or remove the item from the selected items list.
        if (!SelectFromEntireRow && cell.CellType == DataGridCellType.Default)
        {
            return AddOrRemoveSelectedItemAsync(cell.Item);
        }

        return Task.CompletedTask;
    }

    /// <summary />
    private async Task AddOrRemoveSelectedItemAsync(TGridItem? item)
    {
        if (item != null && (Selectable == null || Selectable.Invoke(item)))
        {
            if (SelectMode is DataGridSelectMode.SingleSticky && _selectedItems.Contains(item))
            {
                return;
            }

            if (SelectedItems.Contains(item))
            {
                _selectedItems.Remove(item);
                SelectAll = false;
                await CallOnSelectAsync(item, isSelected: false);
            }
            else
            {
                if (SelectMode is DataGridSelectMode.Single or DataGridSelectMode.SingleSticky)
                {
                    foreach (var previous in _selectedItems)
                    {
                        await CallOnSelectAsync(previous, isSelected: false);
                    }

                    _selectedItems.Clear();
                }

                _selectedItems.Add(item);
                await CallOnSelectAsync(item, isSelected: true);
            }

            if (SelectedItemsChanged.HasDelegate)
            {
                await SelectedItemsChanged.InvokeAsync(SelectedItems);
            }

            RefreshHeaderContent();
        }

        Task CallOnSelectAsync(TGridItem item, bool isSelected)
        {
            return OnSelect.HasDelegate
                ? OnSelect.InvokeAsync((item, isSelected))
                : Task.CompletedTask;
        }
    }

    private async Task UpdateSelectedItemsAsync()
    {

        if (!SelectedItems.Any() || InternalGridContext == null || InternalGridContext.Items == null)
        {
            return;
        }

        var itemsToRemove = _selectedItems.Where(item => !InternalGridContext.Items.Contains(item)).ToList();
        foreach (var item in itemsToRemove)
        {
            await AddOrRemoveSelectedItemAsync(item);
        }
    }

    private Icon GetIcon(bool? selected)
    {
        if (selected == true)
        {
            return IconChecked ?? SelectMode switch
            {
                DataGridSelectMode.Single => IconSelectedSingle,
                DataGridSelectMode.SingleSticky => IconSelectedSingle,
                _ => IconSelectedMultiple
            };
        }

        return IconUnchecked ?? SelectMode switch
        {
            DataGridSelectMode.Single => IconUnselectedSingle,
            DataGridSelectMode.SingleSticky => IconUnselectedSingle,
            _ => IconUnselectedMultiple
        };
    }

    private async Task KeepOnlyFirstSelectedItemAsync()
    {
        if (_selectedItems.Count <= 1)
        {
            return;
        }

        // Unselect all except the first
        foreach (var item in _selectedItems.Skip(1))
        {
            await OnSelect.InvokeAsync((item, false));
        }

        // Keep the first selected item
        _selectedItems.RemoveRange(1, _selectedItems.Count - 1);

        if (SelectedItemsChanged.HasDelegate)
        {
            await SelectedItemsChanged.InvokeAsync(_selectedItems);
        }

        // Indeterminate
        SelectAll = null;
        if (SelectAllChanged.HasDelegate)
        {
            await SelectAllChanged.InvokeAsync(SelectAll);
        }
    }

    /// <summary />
    private RenderFragment<TGridItem> GetDefaultChildContent()
    {
        return (item) => new RenderFragment((builder) =>
        {
            if (Selectable != null && !Selectable.Invoke(item))
            {
                return;
            }

            var selected = _selectedItems.Contains(item) || Property.Invoke(item);

            // Sync with SelectedItems list
            if (selected && !_selectedItems.Contains(item))
            {
                _selectedItems.Add(item);
                RefreshHeaderContent();
            }
            else if (!selected && _selectedItems.Contains(item))
            {
                _selectedItems.Remove(item);
            }

            builder.OpenComponent<FluentIcon<Icon>>(0);
            builder.AddAttribute(1, "Value", GetIcon(selected));
            builder.AddAttribute(2, "Title", selected ? TitleChecked : TitleUnchecked);
            builder.AddAttribute(3, "row-selected", selected);

            if (!SelectFromEntireRow)
            {
                builder.AddAttribute(4, "style", "cursor: pointer;");
            }

            builder.CloseComponent();
        });
    }

    /// <summary />
    private RenderFragment GetHeaderContent()
    {
        switch (SelectMode)
        {
            case DataGridSelectMode.Single:
                return new RenderFragment((builder) => { });

            case DataGridSelectMode.SingleSticky:
                return new RenderFragment((builder) => { });

            case DataGridSelectMode.Multiple:
                var selectedAll = GetSelectAll();
                var iconAllChecked = (selectedAll == null && IconIndeterminate != null)
                                    ? IconIndeterminate
                                    : GetIcon(selectedAll);

                return new RenderFragment((builder) =>
                {
                    builder.OpenComponent<FluentIcon<Icon>>(0);
                    builder.AddAttribute(1, "Value", iconAllChecked);
                    builder.AddAttribute(2, "all-selected", SelectAll);
                    if (!SelectAllDisabled)
                    {
                        builder.AddAttribute(3, "OnClick", EventCallback.Factory.Create<MouseEventArgs>(this, OnClickAllAsync));
                        builder.AddAttribute(4, "onkeydown", EventCallback.Factory.Create<KeyboardEventArgs>(this, OnKeyAllAsync));
                    }

                    builder.AddAttribute(5, "Title", iconAllChecked == IconIndeterminate
                                                        ? TitleAllIndeterminate
                                                        : (iconAllChecked == GetIcon(selected: true) ? TitleAllChecked : TitleAllUnchecked));
                    builder.CloseComponent();
                });

            default:
                return new RenderFragment((builder) => { });
        }
    }

    /// <summary />
    private void RefreshHeaderContent()
    {
        if (SelectAllTemplate == null)
        {
            HeaderContent = GetHeaderContent();
        }
        else
        {
            HeaderContent = new RenderFragment((builder) =>
            {
                builder.OpenElement(0, "div");
                if (!SelectAllDisabled)
                {
                    builder.AddAttribute(1, "style", "cursor: pointer;");
                    builder.AddAttribute(2, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, OnClickAllAsync));
                    builder.AddAttribute(3, "onkeydown", EventCallback.Factory.Create<KeyboardEventArgs>(this, OnKeyAllAsync));
                }

                builder.AddContent(4, SelectAllTemplate.Invoke(new SelectAllTemplateArgs(GetSelectAll())));
                builder.CloseElement();
            });
        }
    }

    /// <summary />
    private bool? GetSelectAll()
    {
        // Using SelectedItems only
        if (InternalGridContext != null && (Grid.Items != null || Grid.ItemsProvider != null))
        {
            if (!SelectedItems.Any())
            {
                return false;
            }

            if (SelectedItems.Take(InternalGridContext.TotalItemCount + 1).Count() == InternalGridContext.TotalItemCount || SelectAll == true)
            {
                return true;
            }

            return null;
        }

        return null;
    }

    /// <inheritdoc />
    protected internal override void CellContent(RenderTreeBuilder builder, TGridItem item)
        => builder.AddContent(0, ChildContent(item));

    /// <inheritdoc />
    protected internal override string? RawCellContent(TGridItem item)
    {
        return TooltipText?.Invoke(item);
    }

    /// <summary >
    /// A <see cref="FluentDataGrid{TGridItem}"/> cannot be sorted on a <see cref="SelectColumn{TGridItem}"/>.
    /// </summary>
    protected override bool IsSortableByDefault() => false;

    /// <summary />
    internal async Task OnClickAllAsync(MouseEventArgs e)
    {
        if (Grid == null || SelectMode != DataGridSelectMode.Multiple || SelectAllDisabled)
        {
            return;
        }

        // SelectAllChanged
        SelectAll = !(SelectAll == true);
        if (SelectAllChanged.HasDelegate)
        {
            await SelectAllChanged.InvokeAsync(SelectAll);
        }

        var count = SelectedItems.Count();
        // SelectedItems
        _selectedItems.Clear();
        if (SelectAll == true && count != InternalGridContext.TotalItemCount)
        {
            // Only add selectable items
            _selectedItems.AddRange((InternalGridContext.Grid.Items?.ToList() ?? InternalGridContext.Items)
                .Where(item => Selectable?.Invoke(item) ?? true)
            );
        }

        if (SelectedItemsChanged.HasDelegate)
        {
            await SelectedItemsChanged.InvokeAsync(SelectedItems);
        }

        RefreshHeaderContent();
    }

    /// <summary />
    internal async Task OnKeyAllAsync(KeyboardEventArgs e)
    {
        if (KEYBOARD_SELECT_KEYS.Contains(e.Code, StringComparer.OrdinalIgnoreCase))
        {
            await OnClickAllAsync(new MouseEventArgs());
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _itemsChanged.Dispose();

    }
}

/// <summary>
/// Represents the arguments for selecting all items in a template.
/// </summary>
/// <param name="AllSelected">A nullable boolean indicating whether all items are selected.  <see langword="true"/> if all items are selected;
/// <see langword="false"/> if not;  <see langword="null"/> if the selection state is undefined.</param>
#pragma warning disable MA0048 // File name must match type name
public record SelectAllTemplateArgs(bool? AllSelected) { }
#pragma warning restore MA0048 // File name must match type name
