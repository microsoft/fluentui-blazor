using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.Fast.Components.FluentUI.DataGrid.Infrastructure;

public interface IBindColumn
{
    public PropertyInfo? PropertyInfo { get; }
}

internal interface IBindColumn<TGridItem, TValue> : IBindColumn
{

    public Expression<Func<TGridItem, TValue>> Property { get; set; }

}
