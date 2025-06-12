using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;

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
/// <typeparam name="TProp">Type of property</typeparam>
internal interface IBindableColumn<TItem, TProp> : IBindableColumn
{

    public Expression<Func<TItem, TProp>> Property { get; set; }

}
