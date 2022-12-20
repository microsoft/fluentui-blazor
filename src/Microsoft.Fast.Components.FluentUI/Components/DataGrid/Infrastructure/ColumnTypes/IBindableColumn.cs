using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.Fast.Components.FluentUI.DataGrid.Infrastructure;

/// <summary>
/// a column that can bind to a property of model
/// </summary>
public interface IBindableColumn
{
   PropertyInfo? PropertyInfo { get; }
}
/// <summary>
/// a column that can bind to a property of model
/// </summary>
/// <typeparam name="TItem">model item type</typeparam>
/// <typeparam name="TValue">type of property</typeparam>
internal interface IBindableColumn<TItem, TValue> : IBindableColumn
{

    public Expression<Func<TItem, TValue>> Property { get; set; }

}
