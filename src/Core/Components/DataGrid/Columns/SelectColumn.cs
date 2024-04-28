using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a <see cref="FluentDataGrid{TGridItem}"/> column whose cells render a selected checkbox updated when the user click on a row.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
public class SelectColumn<TGridItem> : ColumnBase<TGridItem>
{
    private DataGridSelectMode _selectMode = DataGridSelectMode.Single;
    private readonly List<TGridItem> _selectedItems = new List<TGridItem>();

    public SelectColumn()
    {
        Width = "50px";
        ChildContent = GetDefaultChildContent();
    }

    /// <summary>
    /// Gets or sets the content to be rendered for each row in the table.
    /// </summary>
    [Parameter]
    public RenderFragment<TGridItem> ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the list of selected items.
    /// </summary>
    [Parameter]
    public IEnumerable<TGridItem> SelectedItems
    {
        get => _selectedItems;
        set
        {
            if (_selectedItems != value)
            {
                _selectedItems.Clear();
                _selectedItems.AddRange(value);
            }
        }
    }

    [Parameter]
    public EventCallback<IEnumerable<TGridItem>> SelectedItemsChanged { get; set; }

    /// <summary>
    /// Gets or sets the selection mode (Single or Multiple).
    /// </summary>
    [Parameter]
    public DataGridSelectMode SelectMode
    {
        get => _selectMode;
        set
        {
            _selectMode = value;

            if (value == DataGridSelectMode.Single)
            {
                KeepOnlyFirstSelectedItemAsync().Wait();
            }

            RefreshHeaderContent();
        }
    }

    /// <summary>
    /// Gets or sets the Icon to be rendered when the row is non selected.
    /// </summary>
    [Parameter]
    public required Icon IconUnchecked { get; set; } = new CoreIcons.Regular.Size20.CheckboxUnchecked().WithColor(Color.FillInverse);

    /// <summary>
    /// Gets or sets the Icon to be rendered when the row is selected.
    /// </summary>
    [Parameter]
    public required Icon IconChecked { get; set; } = new CoreIcons.Filled.Size20.CheckboxChecked();

    /// <summary>
    /// Gets or sets the Icon to be rendered when some but not all rows are selected.
    /// </summary>
    [Parameter]
    public Icon? IconIndeterminate { get; set; } = new CoreIcons.Filled.Size20.CheckboxIndeterminate();

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
    /// Gets or sets the function to be executed to display the checked/unchecked icon, depending of you data model.
    /// </summary>
    [Parameter]
    public Func<TGridItem, bool> Property { get; set; } = (item) => false;

    /// <inheritdoc />
    [Parameter]
    public override GridSort<TGridItem>? SortBy { get; set; }

    /// <summary />
    internal async Task AddOrRemoveSelectedItemAsync(TGridItem? item)
    {
        if (item != null)
        {
            if (SelectedItems.Contains(item))
            {
                _selectedItems.Remove(item);
                await CallOnSelect(item, false);
            }
            else
            {
                if (SelectMode == DataGridSelectMode.Single)
                {
                    foreach (var previous in _selectedItems)
                    {
                        await CallOnSelect(previous, false);
                    }
                    _selectedItems.Clear();
                }

                _selectedItems.Add(item);
                await CallOnSelect(item, true);
            }

            if (SelectedItemsChanged.HasDelegate)
            {
                await SelectedItemsChanged.InvokeAsync(SelectedItems);
            }

            SelectAll = null;
            RefreshHeaderContent();
        }

        Task CallOnSelect(TGridItem item, bool isSelected)
        {
            return OnSelect.HasDelegate
                ? OnSelect.InvokeAsync((item, isSelected))
                : Task.CompletedTask;
        }
    }

    private async Task KeepOnlyFirstSelectedItemAsync()
    {
        if (_selectedItems.Count() <= 1)
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
    }

    /// <summary />
    private RenderFragment<TGridItem> GetDefaultChildContent()
    {
        return (item) => new RenderFragment((builder) =>
        {
            var selected = SelectedItems.Contains(item) || Property.Invoke(item);

            builder.OpenComponent<FluentIcon<Icon>>(0);
            builder.AddAttribute(1, "Value", selected ? IconChecked : IconUnchecked);
            builder.AddAttribute(2, "row-selected", selected);
            builder.CloseComponent();
        });
    }

    /// <summary />
    private RenderFragment GetHeaderContent()
    {
        SelectAll = SelectedItems.Any() ? null : false;

        switch (SelectMode)
        {
            case DataGridSelectMode.Single:
                return new RenderFragment((builder) => { });

            case DataGridSelectMode.Multiple:
                var iconAllChecked = (SelectAll == null && IconIndeterminate != null)
                                    ? IconIndeterminate
                                    : (SelectAll == true ? IconChecked : IconUnchecked);

                return new RenderFragment((builder) =>
                {
                    builder.OpenComponent<FluentIcon<Icon>>(0);
                    builder.AddAttribute(1, "Value", iconAllChecked);
                    builder.AddAttribute(2, "all-selected", SelectAll);
                    builder.AddAttribute(3, "OnClick", EventCallback.Factory.Create<MouseEventArgs>(this, OnClickAllAsync));
                    builder.CloseComponent();
                });

            default:
                return new RenderFragment((builder) => { });
        }
    }

    /// <summary />
    private void RefreshHeaderContent()
    {
        HeaderContent = GetHeaderContent();
    }

    /// <inheritdoc />
    protected internal override void CellContent(RenderTreeBuilder builder, TGridItem item)
        => builder.AddContent(0, ChildContent(item));

    /// <inheritdoc />
    protected internal override string? RawCellContent(TGridItem item)
    {
        return TooltipText?.Invoke(item);
    }

    /// <inheritdoc />
    protected override bool IsSortableByDefault() => SortBy is not null;

    /// <summary />
    private async Task OnClickAllAsync(MouseEventArgs e)
    {
        if (Grid.Items == null || SelectMode != DataGridSelectMode.Multiple)
        {
            return;
        }

        // SelectAllChanged
        SelectAll = !(SelectAll == true);
        if (SelectAllChanged.HasDelegate)
        {
            await SelectAllChanged.InvokeAsync(SelectAll);
        }

        // SelectedItems
        _selectedItems.Clear();
        if (SelectAll == true)
        {
            _selectedItems.AddRange(Grid.Items);
        }

        RefreshHeaderContent();
    }
}
