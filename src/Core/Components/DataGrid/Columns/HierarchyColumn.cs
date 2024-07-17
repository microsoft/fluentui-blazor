using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a <see cref="FluentDataGrid{TGridItem}"/> column whose cells render a expand/collapse when the user click on a row.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
public class HierarchyColumn<TGridItem> : ColumnBase<TGridItem>
{
    private DataGridExpandMode _expandMode = DataGridExpandMode.Single;
    private readonly List<TGridItem> _expandedItems = [];

    /// <summary>
    /// Initializes a new instance of <see cref="HierarchyColumn{TGridItem}"/>.
    /// </summary>
    public HierarchyColumn()
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
    /// Gets or sets whether the [All] expand is disabled (not clickable).
    /// </summary>
    [Parameter]
    public bool ExpandAllDisabled { get; set; } = false;

    /// <summary>
    /// Gets or sets the template for the [All] icon column template.
    /// </summary>
    [Parameter]
    public RenderFragment<ExpandAllTemplateArgs>? ExpandAllTemplate { get; set; }

    /// <summary>
    /// Gets or sets the list of expanded items.
    /// </summary>
    [Parameter]
    public IEnumerable<TGridItem> ExpandedItems
    {
        get => _expandedItems;
        set
        {
            if (_expandedItems != value)
            {
                _expandedItems.Clear();
                _expandedItems.AddRange(value);
            }
        }
    }

    /// <summary>
    /// Gets or sets a callback when list of expanded items changed.
    /// </summary>
    [Parameter]
    public EventCallback<IEnumerable<TGridItem>> ExpandedItemsChanged { get; set; }

    /// <summary>
    /// Gets or sets the expand mode (Single or Multiple).
    /// </summary>
    [Parameter]
    public DataGridExpandMode ExpandMode
    {
        get => _expandMode;
        set
        {
            _expandMode = value;

            if (value == DataGridExpandMode.Single)
            {
                KeepOnlyFirstExpandedItemAsync().Wait();
            }

            RefreshHeaderContent();
        }
    }

    /// <summary>
    /// Gets or sets the Icon to be rendered when the row is collapsed.
    /// </summary>
    [Parameter]
    public Icon IconCollapsed { get; set; } = new CoreIcons.Regular.Size12.ChevronRight().WithColor(Color.FillInverse);

    /// <summary>
    /// Gets or sets the Icon title display as a tooltip and used with Accessibility.
    /// The default text is "Row collapsed".
    /// </summary>
    [Parameter]
    public string TitleCollapsed { get; set; } = "Row collapsed";

    /// <summary>
    /// Gets or sets the Icon to be rendered when the row is expanded.
    /// </summary>
    [Parameter]
    public Icon IconExpanded { get; set; } = new CoreIcons.Regular.Size12.ChevronDown();

    /// <summary>
    /// Gets or sets the Icon title display as a tooltip and used with Accessibility.
    /// The default text is "Row expanded".
    /// </summary>
    [Parameter]
    public string TitleExpanded { get; set; } = "Row expanded.";

    /// <summary>
    /// Gets or sets the Icon title display as a tooltip and used with Accessibility.
    /// The default text is "All rows are expanded.".
    /// </summary>
    [Parameter]
    public string TitleAllExpanded { get; set; } = "All rows are expanded.";

    /// <summary>
    /// Gets or sets the Icon title display as a tooltip and used with Accessibility.
    /// The default text is "No rows are expanded.".
    /// </summary>
    [Parameter]
    public string TitleAllCollapsed { get; set; } = "No rows are expanded.";

    /// <summary>
    /// Gets or sets the action to be executed when the row is expanded or collapsed.
    /// This action is required to update you data model.
    /// </summary>
    [Parameter]
    public EventCallback<(TGridItem Item, bool Expanded)> OnSelect { get; set; }

    /// <summary>
    /// Gets or sets the value indicating whether the [All] icon is expanded.
    /// </summary>
    [Parameter]
    public bool ExpandAll { get; set; } = false;

    /// <summary>
    /// Gets or sets the action to be executed when the [All] icon is clicked.
    /// When this action is defined, the [All] icon is displayed.
    /// This action is required to update you data model.
    /// </summary>
    [Parameter]
    public EventCallback<bool?> ExpandAllChanged { get; set; }

    /// <summary>
    /// Gets or sets the function to executed to determine Expanded/Collapsed status.
    /// </summary>
    [Parameter]
    public Func<TGridItem, bool> Property { get; set; } = (item) => false;

    /// <inheritdoc />
    [Parameter]
    public override GridSort<TGridItem>? SortBy { get; set; }

    /// <summary>
    /// Gets or sets the function to executed to determine the row can be expanded.
    /// </summary>
    [Parameter]
    public Func<TGridItem, bool>? CanBeExpanded { get; set; }

    /// <summary>
    /// Allows to collapse all.
    /// </summary>
    public void CollapseAll()
    {
        _expandedItems.Clear();
        RefreshHeaderContent();
    }

    /// <summary>
    /// Allows to collapse all.
    /// </summary>
    public async Task CollapseAllAsync()
    {
        CollapseAll();
        await Task.CompletedTask;
    }

    /// <inheritdoc />
    protected internal override Task OnCellClickAsync(FluentDataGridCell<TGridItem> cell)
    {
        // If the cell is a icon cell, add or remove the item from the expanded items list.
        if (cell.CellType == DataGridCellType.Default)
        {
            return AddOrRemoveExpandedItemAsync(cell.Item);
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    protected internal override Task OnCellKeyDownAsync(FluentDataGridCell<TGridItem> cell, KeyboardEventArgs args)
    {
        // If the cell is a icon cell, add or remove the item from the selected items list.
        if (cell.CellType == DataGridCellType.Default)
        {
            return AddOrRemoveExpandedItemAsync(cell.Item);
        }

        return Task.CompletedTask;
    }

    /// <summary />
    private async Task AddOrRemoveExpandedItemAsync(TGridItem? item)
    {
        if (item != null)
        {
            if (ExpandedItems.Contains(item))
            {
                _expandedItems.Remove(item);
                ExpandAll = false;
                await CallOnSelectAsync(item, false);
            }
            else
            {
                if (ExpandMode == DataGridExpandMode.Single)
                {
                    foreach (var previous in _expandedItems)
                    {
                        await CallOnSelectAsync(previous, false);
                    }
                    _expandedItems.Clear();
                }

                _expandedItems.Add(item);
                await CallOnSelectAsync(item, true);
            }

            if (ExpandedItemsChanged.HasDelegate)
            {
                await ExpandedItemsChanged.InvokeAsync(ExpandedItems);
            }

            RefreshHeaderContent();
        }

        Task CallOnSelectAsync(TGridItem item, bool isExpanded)
        {
            return OnSelect.HasDelegate
                ? OnSelect.InvokeAsync((item, isExpanded))
                : Task.CompletedTask;
        }
    }

    private Icon GetIcon(bool expanded) =>
        expanded
            ? IconExpanded
            : IconCollapsed;

    private async Task KeepOnlyFirstExpandedItemAsync()
    {
        if (_expandedItems.Count <= 1)
        {
            return;
        }

        // collapse all except the first
        foreach (var item in _expandedItems.Skip(1))
        {
            await OnSelect.InvokeAsync((item, false));
        }

        // Keep the first expanded item
        _expandedItems.RemoveRange(1, _expandedItems.Count - 1);

        if (ExpandedItemsChanged.HasDelegate)
        {
            await ExpandedItemsChanged.InvokeAsync(_expandedItems);
        }

        // Indeterminate
        ExpandAll = false;
        if (ExpandAllChanged.HasDelegate)
        {
            await ExpandAllChanged.InvokeAsync(ExpandAll);
        }
    }

    /// <summary />
    private RenderFragment<TGridItem> GetDefaultChildContent()
    {
        return (item) => new RenderFragment((builder) =>
        {
            if (CanBeExpanded == null || CanBeExpanded.Invoke(item))
            {
                var expanded = _expandedItems.Contains(item) || Property.Invoke(item);

                // Sync with ExpandedItems list
                if (expanded && !_expandedItems.Contains(item))
                {
                    _expandedItems.Add(item);
                    RefreshHeaderContent();
                }
                else if (!expanded && _expandedItems.Contains(item))
                {
                    _expandedItems.Remove(item);
                }

                builder.OpenComponent<FluentIcon<Icon>>(0);
                builder.AddAttribute(1, "Value", GetIcon(expanded));
                builder.AddAttribute(2, "Title", expanded ? TitleExpanded : TitleCollapsed);
                builder.AddAttribute(3, "row-expanded", expanded);
                builder.AddAttribute(4, "style", "cursor: pointer;");
                builder.CloseComponent();
            }
        });
    }

    /// <summary />
    private RenderFragment GetHeaderContent()
    {
        switch (ExpandMode)
        {
            case DataGridExpandMode.Single:
                return new RenderFragment((builder) => { });

            case DataGridExpandMode.Multiple:
                var expandedAll = GetExpandAll();
                var iconAllExpanded = GetIcon(expandedAll);

                return new RenderFragment((builder) =>
                {
                    builder.OpenComponent<FluentIcon<Icon>>(0);
                    builder.AddAttribute(1, "Value", iconAllExpanded);
                    builder.AddAttribute(2, "all-expanded", ExpandAll);
                    if (!ExpandAllDisabled)
                    {
                        builder.AddAttribute(3, "OnClick", EventCallback.Factory.Create<MouseEventArgs>(this, OnClickAllAsync));
                    }
                    builder.AddAttribute(5, "Style", "margin-left: 12px;");
                    builder.AddAttribute(6, "Title", iconAllExpanded == GetIcon(true) ? TitleAllExpanded : TitleAllCollapsed);
                    builder.CloseComponent();
                });

            default: return new RenderFragment((builder) => { });
        }
    }

    /// <summary />
    private void RefreshHeaderContent()
    {
        if (ExpandAllTemplate == null)
        {
            HeaderContent = GetHeaderContent();
        }
        else
        {
            HeaderContent = new RenderFragment((builder) =>
            {
                builder.OpenElement(0, "div");
                if (!ExpandAllDisabled)
                {
                    builder.AddAttribute(1, "style", "cursor: pointer; margin-left: 12px;");
                    builder.AddAttribute(2, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, OnClickAllAsync));
                }
                builder.AddContent(4, ExpandAllTemplate.Invoke(new ExpandAllTemplateArgs(GetExpandAll())));
                builder.CloseElement();
            });
        }
    }

    /// <summary />
    private bool GetExpandAll()
    {
        // Using ExpandedItems only
        if (InternalGridContext != null && (Grid.Items != null || Grid.ItemsProvider != null))
        {
            if (!ExpandedItems.Any())
            {
                return false;
            }
            else if (ExpandedItems.Count() == InternalGridContext.TotalItemCount || ExpandAll)
            {
                return true;
            }
        }
        return false;
    }

    /// <inheritdoc />
    protected internal override void CellContent(RenderTreeBuilder builder, TGridItem item) => builder.AddContent(0, ChildContent(item));

    /// <inheritdoc />
    protected internal override string? RawCellContent(TGridItem item) => TooltipText?.Invoke(item);

    /// <inheritdoc />
    protected override bool IsSortableByDefault() => SortBy is not null;

    /// <summary />
    internal async Task OnClickAllAsync(MouseEventArgs e)
    {
        if (Grid == null || ExpandMode != DataGridExpandMode.Multiple || ExpandAllDisabled)
        {
            return;
        }

        // ExpandAllChanged
        ExpandAll = !ExpandAll;
        if (ExpandAllChanged.HasDelegate)
        {
            await ExpandAllChanged.InvokeAsync(ExpandAll);
        }

        // ExpandedItems
        _expandedItems.Clear();
        if (ExpandAll)
        {
            var data = CanBeExpanded == null
                        ? InternalGridContext.Items
                        : InternalGridContext.Items.Where(a => CanBeExpanded.Invoke(a));

            _expandedItems.AddRange(data);
        }

        if (ExpandedItemsChanged.HasDelegate)
        {
            await ExpandedItemsChanged.InvokeAsync(ExpandedItems);
        }

        RefreshHeaderContent();
    }
}

public record ExpandAllTemplateArgs(bool AllExpanded) { }
