using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Grid;

namespace Microsoft.Fast.Components.FluentUI;

[CascadingTypeParameter(nameof(TGridItem))]
public partial class FluentDataGrid<TGridItem> : FluentComponentBase
{
    private readonly Dictionary<string, FluentDataGridRow<TGridItem>> rows = new();

    private IEnumerable<TGridItem> _items = Array.Empty<TGridItem>();

    internal IEnumerable<TGridItem> InternalRowsData
    {
        get => _items;
        set
        {
            _items = value ?? Array.Empty<TGridItem>();
        }
    }

    /// <summary>
    /// When true the component will not add itself to the tab queue. Default is false.
    /// </summary>
    [Parameter]
    public bool NoTabbing { get; set; }

    /// <summary>
    /// Whether the grid should automatically generate a header row and its type
    /// See <see cref="GenerateHeaderOption"/>
    /// </summary>
    [Parameter]
    public GenerateHeaderOption? GenerateHeader { get; set; } = GenerateHeaderOption.Default;

    /// <summary>
    /// Gets or sets the value that gets applied to the css gridTemplateColumns attribute of child rows
    /// </summary>
    [Parameter]
    public string? GridTemplateColumns { get; set; } = "";

    /// <summary>
    /// A queryable source of data for the grid.
    ///
    /// This could be in-memory data converted to queryable using the
    /// <see cref="System.Linq.Queryable.AsQueryable(System.Collections.IEnumerable)"/> extension method,
    /// or an <see cref="IQueryable"/> derived from it.
    ///
    /// You should supply either <see cref="RowsData"/> or <see cref="RowsDataProvider"/>, but not both.
    /// </summary>
    [Parameter]
    public IQueryable<TGridItem>? RowsData { get; set; }

    /// <summary>
    /// A callback that supplies data for the rid.
    ///
    /// You should supply either <see cref="RowsData"/> or <see cref="RowsDataProvider"/>, but not both.
    /// </summary>
    [Parameter] public GridItemsProvider<TGridItem>? RowsDataProvider { get; set; }

    /// <summary>
    /// Gets or sets the column definitions. See <see cref="ColumnDefinition{TGridItem}"/>
    /// </summary>
    [Parameter]
    public IEnumerable<ColumnDefinition<TGridItem>>? ColumnDefinitions { get; set; }

    /// <summary>
    /// Gets or sets the header cell template. See <see cref="ColumnDefinition{TGridItem}"/>
    /// </summary>
    [Parameter]
    public RenderFragment<ColumnDefinition<TGridItem>>? HeaderCellTemplate { get; set; } = null;

    /// <summary>
    /// Gets or sets the row item template
    /// </summary>
    [Parameter]
    public RenderFragment<TGridItem>? RowItemTemplate { get; set; } = null;

    /// <summary>
    /// If true, the results are displayed in a Virtualize component, 
    /// allowing for better rendering speed with a large number of rows.
    /// </summary>
    [Parameter]
    public bool Virtualize { get; set; } = false;

    /// <summary>
    /// Gets or sets a callback when a row is focused
    /// </summary>
    [Parameter]
    public EventCallback<FluentDataGridRow<TGridItem>> OnRowFocus { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        if (RowsDataProvider != null)
        {
            if (RowsData != null)
            {
                throw new InvalidOperationException(
                    $"{GetType()} can only accept one item source from its parameters. " +
                    $"Do not supply both '{nameof(RowsData)}' and '{nameof(RowsDataProvider)}'.");
            }
            await InvokeRowsDataProvider();

        }
        if (RowsData != null)
        {
            InternalRowsData = RowsData;
        }
    }

    /// <summary />
    private async Task InvokeRowsDataProvider()
    {
        if (RowsDataProvider == null)
            return;

        //Loading = true;
        StateHasChanged();

        await RowsDataProvider.Invoke(_items);

        //Loading = false;
        StateHasChanged();
    }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await InvokeRowsDataProvider();

        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task HandleOnRowFocus(DataGridRowFocusEventArgs args)
    {
        string? rowId = args.RowId;
        if (rows.TryGetValue(rowId!, out FluentDataGridRow<TGridItem>? row))
        {
            await OnRowFocus.InvokeAsync(row);
        }
    }

    internal void Register(FluentDataGridRow<TGridItem> row)
    {
        rows.Add(row.RowId, row);
    }

    internal void Unregister(FluentDataGridRow<TGridItem> row)
    {
        rows.Remove(row.RowId);
    }
}