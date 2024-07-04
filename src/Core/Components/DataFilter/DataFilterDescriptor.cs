using System.Linq.Expressions;

namespace Microsoft.FluentUI.AspNetCore.Components;

public class DataFilterDescriptor<TItem>
{
    /// <summary>
    /// Logical Operator.
    /// </summary>
    public DataFilterLogicalOperator Operator { get; set; } = DataFilterLogicalOperator.And;

    /// <summary>
    /// Filters
    /// </summary>
    public ICollection<DataFilterDescriptorProperty<TItem>> Filters { get; set; } = [];

    /// <summary>
    /// Groups
    /// </summary>
    public ICollection<DataFilterDescriptor<TItem>> Groups { get; set; } = [];

    /// <summary>
    /// Exists
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    public bool Exists(PropertyFilterBase<TItem> property)
    {
        if (Filters.Any(a => a.Property == property))
        {
            return true;
        }

        if (Groups.Any(a => a.Exists(property)))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Get expression for filter data.
    /// </summary>
    /// <param name="caseSensitivity"></param>
    /// <returns></returns>
    public Expression<Func<TItem, bool>> GetFilter(DataFilterCaseSensitivity caseSensitivity)
    {
        var ret = PredicateBuilder.True<TItem>();
        var found = false;

        foreach (var item in Filters.Select(a => a.GetFilter(caseSensitivity)))
        {
            ret = Operator switch
            {
                DataFilterLogicalOperator.And or DataFilterLogicalOperator.NotAnd => PredicateBuilder.And(ret, item),
                DataFilterLogicalOperator.Or or DataFilterLogicalOperator.NotOr => PredicateBuilder.Or(ret, item),
                _ => ret,
            };
            found = true;
        }

        foreach (var item in Groups.Select(a => a.GetFilter(caseSensitivity)))
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
}
