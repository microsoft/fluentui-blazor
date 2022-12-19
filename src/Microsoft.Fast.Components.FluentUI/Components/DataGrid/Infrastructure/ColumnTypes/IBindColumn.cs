using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.Fast.Components.FluentUI.DataGrid.Infrastructure;

/// <summary>
/// a column that can bind to a property of model
/// </summary>
public interface IBindColumn
{
   PropertyInfo? PropertyInfo { get; }
}
/// <summary>
/// a column that can bind to a property of model
/// </summary>
/// <typeparam name="TItem">model item type</typeparam>
/// <typeparam name="TValue">type of property</typeparam>
internal interface IBindColumn<TItem, TValue> : IBindColumn
{

    public Expression<Func<TItem, TValue>> Property { get; set; }

}
