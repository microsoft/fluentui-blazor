using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.Fast.Components.FluentUI.DataGrid.Infrastructure;

/// <summary>
/// A column that can bind to a property of model
/// </summary>
public interface IBindableColumn
{
   PropertyInfo? PropertyInfo { get; }
}
/// <summary>
/// A column that can bind to a property of model
/// </summary>
/// <typeparam name="TItem">Model item type</typeparam>
/// <typeparam name="TValue">Type of property</typeparam>
internal interface IBindableColumn<TItem, TValue> : IBindableColumn
{

    public Expression<Func<TItem, TValue>> Property { get; set; }

}
