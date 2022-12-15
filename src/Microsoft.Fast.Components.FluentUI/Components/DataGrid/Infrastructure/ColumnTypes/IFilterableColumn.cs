using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.Fast.Components.FluentUI.DataGrid.Infrastructure;

internal interface IFilterableColumn<TGridItem>
{
    public IQueryable<TGridItem> ApplyFilter(IQueryable<TGridItem> Source);

    public PropertyInfo? PropertyInfo { get; }
}

internal interface IFilterableColumn<TGridItem, TValue> : IFilterableColumn<TGridItem>
{

    public Expression<Func<TGridItem, TValue>>? FilterProperty { get; set; }

}
