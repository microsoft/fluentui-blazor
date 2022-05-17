using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentDataGridRow<TItem> : FluentComponentBase
{
    /// <summary>
    /// Gets or sets the index of this row
    /// </summary>
    [Parameter]
    public int? RowIndex { get; set; }

    /// <summary>
    /// Gets or sets the value that gets applied to the css gridTemplateColumns attribute of child rows
    /// </summary>
    [Parameter]
    public string? GridTemplateColumns { get; set; } = null;

    /// <summary>
    /// Gets or sets the type of row. Se <see cref="DataGridRowType"/>
    /// </summary>
    [Parameter]
    public DataGridRowType? RowType { get; set; } = DataGridRowType.Default;

    /// <summary>
    /// Gets or sets the row data
    /// </summary>
    [Parameter]
    public TItem? RowData { get; set; }

    /// <summary>
    /// Gets or sets the column definitions for the row. See <see cref="ColumnDefinition{TItem}"/>
    /// </summary>
    [Parameter]
    public IEnumerable<ColumnDefinition<TItem>>? ColumnDefinitions { get; set; }
}