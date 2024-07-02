using System.Linq.Expressions;

namespace Microsoft.FluentUI.AspNetCore.Components;

public class DataFilterGroup<TItem>
{
    /// <summary>
    /// Logical Operator.
    /// </summary>
    public DataFilterLogicalOperator Operator { get; set; } = DataFilterLogicalOperator.And;

    /// <summary>
    /// Filters
    /// </summary>
    public ICollection<DataFilterProperty<TItem>> Filters { get; set; } = [];

    /// <summary>
    /// Groups
    /// </summary>
    public ICollection<DataFilterGroup<TItem>> Groups { get; set; } = [];

    /// <summary>
    /// Get expression for filter data.
    /// </summary>
    /// <param name="caseSensitivity"></param>
    /// <returns></returns>
    public Expression<Func<TItem, bool>> GenerateExpression(DataFilterCaseSensitivity caseSensitivity)
    {
        var ret = PredicateBuilder.True<TItem>();
        var found = false;

        foreach (var item in Filters.Select(a => a.GenerateExpression(caseSensitivity)))
        {
            ret = Operator switch
            {
                DataFilterLogicalOperator.And or DataFilterLogicalOperator.NotAnd => PredicateBuilder.And(ret, item),
                DataFilterLogicalOperator.Or or DataFilterLogicalOperator.NotOr => PredicateBuilder.Or(ret, item),
                _ => ret,
            };
            found = true;
        }

        foreach (var item in Groups.Select(a => a.GenerateExpression(caseSensitivity)))
        {
            ret = Operator switch
            {
                DataFilterLogicalOperator.And or DataFilterLogicalOperator.NotAnd => PredicateBuilder.And(ret, item),
                DataFilterLogicalOperator.Or or DataFilterLogicalOperator.NotOr => PredicateBuilder.Or(ret, item),
                _ => ret,
            };
            found = true;
        }

        if (found && (Operator == DataFilterLogicalOperator.NotAnd || Operator == DataFilterLogicalOperator.NotOr))
        {
            ret = PredicateBuilder.Not(ret);
        }

        return ret;
    }
    //public string GetQueryString(DataFilterCaseSensitiveModal caseSensitiveModal)
    //{
    //    return "";
    //    //var queries = Filters.Select(a => a.GetQueryString(caseSensitiveModal))
    //    //                     .Where(a => !string.IsNullOrEmpty(a))
    //    //                     .Select(a => $"({a})").ToList();

    //    //queries.AddRange(Groups.Select(a => a.GetQueryString(caseSensitiveModal))
    //    //                       .Where(a => !string.IsNullOrEmpty(a))
    //    //                       .Select(a => $"({a})"));

    //    //var query = string.Join($" {Operator.ToLinq()} ", queries);
    //    //if (!string.IsNullOrEmpty(query))
    //    //{
    //    //    if (Operator == DataFilterLogicalOperator.NotAnd || Operator == DataFilterLogicalOperator.NotOr)
    //    //    {
    //    //        query = $"!({query})";
    //    //    }
    //    //    query = $"({query})";
    //    //}

    //    //return query;
    //}
}
