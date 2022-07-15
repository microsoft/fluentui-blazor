using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

[CascadingTypeParameter(nameof(TItem))]
public partial class FluentDataGrid<TItem> : FluentComponentBase
{
    private readonly Dictionary<string, FluentDataGridRow<TItem>> rows = new();
    /// <summary>
    /// Gets or sets the <see cref="GenerateHeaderOption"/>
    /// </summary>
    [Parameter]
    public GenerateHeaderOption? GenerateHeader { get; set; } = GenerateHeaderOption.Default;

    /// <summary>
    /// Gets or sets the value that gets applied to the css gridTemplateColumns attribute of child rows
    /// </summary>
    [Parameter]
    public string? GridTemplateColumns { get; set; } = "";

    /// <summary>
    /// Gets or sets the rows data
    /// </summary>
    [Parameter]
    public List<TItem>? RowsData { get; set; }

    /// <summary>
    /// Gets or sets the column definitions. See <see cref="ColumnDefinition{TItem}"/>
    /// </summary>
    [Parameter]
    public IEnumerable<ColumnDefinition<TItem>>? ColumnDefinitions { get; set; }

    /// <summary>
    /// Gets or sets the header cell template. See <see cref="ColumnDefinition{TItem}"/>
    /// </summary>
    [Parameter]
    public RenderFragment<ColumnDefinition<TItem>>? HeaderCellTemplate { get; set; } = null;

    /// <summary>
    /// Gets or sets the row item template
    /// </summary>
    [Parameter]
    public RenderFragment<TItem>? RowItemTemplate { get; set; } = null;

    /// <summary>
    /// Gets or sets a callback when a row is focused
    /// </summary>
    [Parameter]
    public EventCallback<FluentDataGridRow<TItem>> OnRowFocus { get; set; }

    private async Task HandleOnRowFocus(DataGridRowFocusEventArgs args)
    {
        string? rowId = args.RowId;
        if (rows.TryGetValue(rowId!, out FluentDataGridRow<TItem>? row))
        {
            await OnRowFocus.InvokeAsync(row);
        }
    }

    internal void Register(FluentDataGridRow<TItem> row)
    {
        rows.Add(row.RowId, row);
    }

    internal void Unregister(FluentDataGridRow<TItem> row)
    {
        rows.Remove(row.RowId);
    }
}