using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;
public partial class FluentDataGrid<TItem> : FluentComponentBase
{
    /// <summary>
    /// Gets or sets the <see cref="GenerateHeaderOptions"/>
    /// </summary>
    [Parameter]
    public GenerateHeaderOptions? GenerateHeader { get; set; } = GenerateHeaderOptions.Default;

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
}