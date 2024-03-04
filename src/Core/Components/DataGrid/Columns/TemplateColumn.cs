using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a <see cref="FluentDataGrid{TGridItem}"/> column whose cells render a supplied template.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
public class TemplateColumn<TGridItem> : ColumnBase<TGridItem>
{
    private static readonly RenderFragment<TGridItem> EmptyChildContent = _ => builder => { };

    /// <summary>
    /// Gets or sets the content to be rendered for each row in the table.
    /// </summary>
    [Parameter] public RenderFragment<TGridItem> ChildContent { get; set; } = EmptyChildContent;

    /// <inheritdoc />
    [Parameter] public override GridSort<TGridItem>? SortBy { get; set; }

    /// <inheritdoc />
    protected internal override void CellContent(RenderTreeBuilder builder, TGridItem item)
        => builder.AddContent(0, ChildContent(item));

    protected internal override string? RawCellContent(TGridItem item)
    {
        var cachedFunc = ExpressionCache<TGridItem, string?>.CachedCompile(TooltipText!);
        return cachedFunc(item);
    }

    /// <inheritdoc />
    protected override bool IsSortableByDefault()
        => SortBy is not null;
}
