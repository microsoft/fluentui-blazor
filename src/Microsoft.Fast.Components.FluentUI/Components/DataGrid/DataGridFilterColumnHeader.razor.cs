using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.DataGrid.Infrastructure;
using System;
using System.Buffers;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;

namespace Microsoft.Fast.Components.FluentUI;

partial class DataGridFilterColumnHeader<TGridItem, TValue> : DataGridFilterColumnHeaderBase<TGridItem> where TGridItem : class
{

    #region Fileds

    bool hasFilter { get; set; } = false;

    private Type propertyType = default!;
    private bool IsNullable = false;
    private bool IsSortable = false;
    MethodInfo ReplaceMethod;
    MethodInfo ToLowerMethod;
    MethodInfo ContainsMethod;
    ConstantExpression OneSpace;
    ConstantExpression Empty;

    #endregion

    #region Initialization

    public DataGridFilterColumnHeader()
    {
        ReplaceMethod = typeof(string).GetMethod("Replace", new[] { typeof(string), typeof(string) })!;
        ContainsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
        ToLowerMethod = typeof(string).GetMethod("ToLower", new Type[0])!;

        OneSpace = Expression.Constant(" ");
        Empty = Expression.Constant("");
    }

    #endregion

    #region Properties

    [CascadingParameter]
    private InternalGridContext<TGridItem> Owner { get; set; } = default!;

    [CascadingParameter]
    private ColumnBase<TGridItem> OwnerColumn { get; set; } = default!;

    private IFilterableColumn<TGridItem, TValue> COwnerColumn { get; set; } = default!;


    #region Filters

    public string? TextFilter { get; set; }

    public Nullable<DateTime> FromDate { get; set; }

    public Nullable<DateTime> ToDate { get; set; }

    public Nullable<double> FromNumber { get; set; }

    public Nullable<double> ToNumber { get; set; }

    public Nullable<bool> Bool { get; set; }

    private IEnumerable<TValue>? DistinctList { get; set; }

    private TValue? SelectedItem { get; set; }

    #endregion


    #endregion

    #region Function

    private void openPopupClicked()
    {
        if (!dropDownIsOpen)
        {
            dropDownIsOpen = true;
            Owner.Grid.ShowColumnOptions((OwnerColumn as ColumnBase<TGridItem>)!);
        }
        else
            Owner.Grid.CloseColumnOptions();
    }

    internal override IQueryable<TGridItem> ApplyFilter(IQueryable<TGridItem> Source)
    {
        if (propertyType is null)
            return Source;
        if (propertyType == typeof(string))
            return applyTextFilter(Source);
        else if (propertyType == typeof(DateTime))
            return applyDateTimeFilter(Source);
        else if (propertyType == typeof(int) || propertyType == typeof(short) || propertyType == typeof(decimal) || propertyType == typeof(double)
                || propertyType == typeof(Single))
            return applyNumberFilter(Source);
        else if (propertyType == typeof(bool))
            return applyBoolFilter(Source);
        else if (propertyType.IsClass)
            return applySelectedItemFilter(applyTextFilter(Source));
        else
        {
            hasFilter = false;
            return Source;
        }
    }

    private void getPropertyType()
    {
        if (OwnerColumn is null)
            return;
        propertyType = typeof(TValue);
        if (typeof(Nullable<>).IsAssignableFrom(propertyType))
        {
            propertyType = propertyType.GetGenericArguments()[0];
            IsNullable = true;
        }
        if (OwnerColumn.Sortable.HasValue ? OwnerColumn.Sortable.Value : OwnerColumn.IsSortableByDefault())
            IsSortable = true;
        COwnerColumn = (IFilterableColumn<TGridItem, TValue>)OwnerColumn;
    }

    private void prepaireDistictList()
    {
        if (COwnerColumn is null)
            return;
        var filterFunc = COwnerColumn.FilterProperty?.Compile();
        if (filterFunc is not null)
            DistinctList = Owner.Grid.RowsData?.Select(s => filterFunc(s)).Distinct();
    }

    private void RemoveSort()
    {
        if (typeof(ISortableColumn<TGridItem>).IsAssignableFrom(OwnerColumn.GetType()))
        {
            var ccol = (ISortableColumn<TGridItem>)OwnerColumn;
            if (ccol is not null)
            {
                ccol.SortDirection = null;
                ccol.SortOrder = 0;
                Owner.Grid.ApplySort(ccol);
                Owner.Grid.CloseColumnOptions();
            }
        }
    }

    private void ApplySort()
    {
        if (typeof(ISortableColumn<TGridItem>).IsAssignableFrom(OwnerColumn.GetType()))
        {
            var ccol = (ISortableColumn<TGridItem>)OwnerColumn;
            if (ccol is not null)
            {
                ccol.SortDirection = System.ComponentModel.ListSortDirection.Ascending;
                Owner.Grid.ApplySort(ccol);
                Owner.Grid.CloseColumnOptions();
            }
        }
    }

    private void ApplySortDescending()
    {
        if (typeof(ISortableColumn<TGridItem>).IsAssignableFrom(OwnerColumn.GetType()))
        {
            var ccol = (ISortableColumn<TGridItem>)OwnerColumn;
            if (ccol is not null)
            {
                ccol.SortDirection = System.ComponentModel.ListSortDirection.Descending;
                Owner.Grid.ApplySort(ccol);
                Owner.Grid.CloseColumnOptions();
            }
        }
    }

    private void RemoveFilter()
    {
        TextFilter = null;
        FromDate = null;
        ToDate = null;
        FromNumber = null;
        ToNumber = null;
        SelectedItem = default(TValue);
        Bool = null;
        Owner.Grid.ApplyFilter(COwnerColumn);
        Owner.Grid.CloseColumnOptions();
    }

    #region Filter Request

    private void OnTextFilterSetted(ChangeEventArgs e)
    {
        if (e.Value is not null)
            TextFilter = e.Value.ToString();
        else
            TextFilter = null;
        Owner.Grid.ApplyFilter(COwnerColumn);
        Owner.Grid.CloseColumnOptions();
    }

    private void DateRangeSelected((Nullable<DateTime> fromDate, Nullable<DateTime> toDate) arg)
    {
        FromDate = arg.fromDate;
        ToDate = arg.toDate;
        Owner.Grid.ApplyFilter(COwnerColumn);
        Owner.Grid.CloseColumnOptions();
    }

    private void NumberRangeSelected((Nullable<double> fromDate, Nullable<double> toDate) arg)
    {
        FromNumber = arg.fromDate;
        ToNumber = arg.toDate;
        Owner.Grid.ApplyFilter(COwnerColumn);
        Owner.Grid.CloseColumnOptions();
    }

    private void OnBoolSelectedChanged(string e)
    {
        if (bool.TryParse(e, out bool tmpBool))
            Bool = tmpBool;
        else
            Bool = null;
        Owner.Grid.ApplyFilter((OwnerColumn as IFilterableColumn<TGridItem>)!);
        Owner.Grid.CloseColumnOptions();
    }

    private void OnSelectedItemChanged(TValue selectedItem)
    {
        this.SelectedItem = selectedItem;
        Owner.Grid.ApplyFilter(COwnerColumn);
        Owner.Grid.CloseColumnOptions();
    }

    #endregion

    #region Filters Apply

    private IQueryable<TGridItem> applyTextFilter(IQueryable<TGridItem> source)
    {
        if (string.IsNullOrEmpty(TextFilter))
        {
            hasFilter = false;
            return source;
        }
        hasFilter = true;
        return source.Provider.CreateQuery<TGridItem>(CreateWhereClause(source, TextFilter.Replace(" ", "").ToLower()));
    }

    private IQueryable<TGridItem> applyDateTimeFilter(IQueryable<TGridItem> source)
    {
        if ((!FromDate.HasValue || FromDate == default) && (!ToDate.HasValue || ToDate == default))
        {
            hasFilter = false;
            return source;
        }
        hasFilter = true;
        object? fromValue = null;
        object? toValue = null;
        if (FromDate.HasValue && FromDate != default)
            fromValue = FromDate.Value;
        if (ToDate.HasValue && ToDate != default)
            toValue = ToDate.Value;
        return source.Provider.CreateQuery<TGridItem>(CreateRangeWhereClause(source,fromValue,ToDate));
    }

    private IQueryable<TGridItem> applyNumberFilter(IQueryable<TGridItem> source)
    {
        if ((!FromNumber.HasValue || double.IsNaN(FromNumber.Value)) && (!ToNumber.HasValue || double.IsNaN(ToNumber.Value)))
        {
            hasFilter = false;
            return source;
        }
        object? fromValue = null;
        object? toValue = null;
        if (FromNumber.HasValue && !double.IsNaN(FromNumber.Value))
            fromValue = FromNumber.Value;
        if (ToNumber.HasValue && !double.IsNaN(ToNumber.Value))
            toValue = ToNumber.Value;
        hasFilter = true;

        return source.Provider.CreateQuery<TGridItem>(CreateRangeWhereClause(source,fromValue,toValue));
    }

    private IQueryable<TGridItem> applyBoolFilter(IQueryable<TGridItem> source)
    {
        if (!IsNullable && !Bool.HasValue)
        {
            hasFilter = false;
            return source;
        }
        hasFilter = true;
        return source.Provider.CreateQuery<TGridItem>(CreateWhereClause(source, Bool!.Value));
    }

    private IQueryable<TGridItem> applySelectedItemFilter(IQueryable<TGridItem> source)
    {
        if (SelectedItem is null || DistinctList is null)
        {
            hasFilter = false;
            return source;
        }
        hasFilter = true;
        return source.Provider.CreateQuery<TGridItem>(CreateWhereClause(source, SelectedItem));
    }

    private Expression CreateWhereClause(IQueryable<TGridItem> source, object value)
    {
        var pInfo = COwnerColumn.PropertyInfo!;
        ParameterExpression ExpParam = Expression.Parameter(typeof(TGridItem), "w");
        var RightExp = Expression.Constant(value, pInfo.PropertyType);

        Expression Result;
        if (pInfo.PropertyType == typeof(string))
        {
            //remove spaces
            var LeftExp = Expression.Call(Expression.Property(ExpParam, pInfo), ToLowerMethod);
            //run tolower method
            LeftExp = Expression.Call(LeftExp, ReplaceMethod, new ConstantExpression[] { OneSpace, Empty });
            Result = Expression.Call(LeftExp, ContainsMethod, new ConstantExpression[] { RightExp });
        }
        else
        {
            var LeftExp = (Expression)Expression.Property(ExpParam, pInfo);
            Result = Expression.Equal(LeftExp, RightExp);
        }
        var Deleg = typeof(Func<,>).MakeGenericType(source.ElementType, typeof(bool));
        return Expression.Call(typeof(Queryable), "Where", new Type[] { source.ElementType }, source.Expression, Expression.Lambda(Deleg, Result, new ParameterExpression[] { ExpParam }));
        //return Result;
    }

    private Expression CreateRangeWhereClause(IQueryable<TGridItem> source, object? fromValue,object? toValue)
    {
        var pInfo = COwnerColumn.PropertyInfo!;
        ParameterExpression ExpParam = Expression.Parameter(typeof(TGridItem), "w");
        var propertyExpr = (Expression)Expression.Property(ExpParam, pInfo);
        Expression? Result = null;

        if (fromValue is not null)
        {
            var fromExpr = Expression.Constant(Convert.ChangeType(fromValue, pInfo.PropertyType), pInfo.PropertyType);
            Result = Expression.GreaterThanOrEqual(propertyExpr, fromExpr);
        }
        if (toValue is not null)
        {
            var toExpr = Expression.Constant(Convert.ChangeType(toValue, pInfo.PropertyType), pInfo.PropertyType);
            var toWehreC = Expression.LessThanOrEqual(propertyExpr, toExpr);
            if (Result is null)
                Result = toWehreC;
            else
                Result = Expression.AndAlso(Result, toWehreC);
        }
        var Deleg = typeof(Func<,>).MakeGenericType(source.ElementType, typeof(bool));
        return Expression.Call(typeof(Queryable), "Where", new Type[] { source.ElementType }, source.Expression, Expression.Lambda(Deleg, Result!, new ParameterExpression[] { ExpParam }));
    }

    #endregion

    #endregion


}

public abstract class DataGridFilterColumnHeaderBase<TGridItem> : ComponentBase where TGridItem : class
{
    #region Fileds

    protected bool dropDownIsOpen { get; set; } = false;

    #endregion

    internal void CloseDropDown()
    {
        dropDownIsOpen = false;
        StateHasChanged();
    }

    internal abstract IQueryable<TGridItem> ApplyFilter(IQueryable<TGridItem> Source);

}
