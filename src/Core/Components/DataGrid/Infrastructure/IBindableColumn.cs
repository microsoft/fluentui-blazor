// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;

/// <summary>
/// A column that can bind to a property of model
/// </summary>
internal interface IBindableColumn
{
    /// <summary>
    /// The info for the property that this column binds to.
    /// </summary>
    PropertyInfo? PropertyInfo { get; }
}
/// <summary>
/// A column that can bind to a property of model
/// </summary>
/// <typeparam name="TItem">Model item type</typeparam>
/// <typeparam name="TProp">Type of property</typeparam>
internal interface IBindableColumn<TItem, TProp> : IBindableColumn
{
    /// <summary>
    /// The expression that represents the property to which this column binds.
    /// </summary>  
    public Expression<Func<TItem, TProp>> Property { get; set; }
}
