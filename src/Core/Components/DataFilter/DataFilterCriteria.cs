using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Criteria descriptor.
/// </summary>
/// <typeparam name="TItem"></typeparam>
public class DataFilterCriteria<TItem>
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        Converters =
        {
            new JsonStringEnumConverter()
        },
        WriteIndented = true,
    };

    /// <summary>
    /// Logical Operator.
    /// </summary>
    public DataFilterLogicalOperator Operator { get; set; } = DataFilterLogicalOperator.And;

    /// <summary>
    /// Filters
    /// </summary>
    [JsonIgnore]
    public IList<DataFilterCriteriaCondition<TItem>> Conditions { get; set; } = [];

    [JsonInclude]
    [JsonPropertyName("Conditions")]
    internal IEnumerable<DataFilterCriteriaCondition<TItem>> JsonConditions => Conditions.Where(a => !string.IsNullOrEmpty(a.Field));

    /// <summary>
    /// Groups
    /// </summary>
    public IList<DataFilterCriteria<TItem>> Groups { get; set; } = [];

    /// <summary>
    /// Is used.
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public bool IsUsed(string field) => Conditions.Any(a => a.Field == field) || Groups.Any(a => a.IsUsed(field));

    /// <summary>
    /// Clear
    /// </summary>
    public void Clear()
    {
        Conditions.Clear();
        Groups.Clear();
    }

    /// <summary>
    /// Generate JSON.
    /// </summary>
    /// <returns></returns>
    public string ToJson() => JsonSerializer.Serialize(this, _jsonOptions);

    /// <summary>
    /// To expression.
    /// </summary>
    /// <param name="caseSensitivity"></param>
    /// <returns></returns>
    public Expression<Func<TItem, bool>> ToExpression(DataFilterCaseSensitivity caseSensitivity)
    {
        var ret = Operator == DataFilterLogicalOperator.And || Operator == DataFilterLogicalOperator.NotAnd
                    ? DataFilterPredicateBuilder.True<TItem>()
                    : DataFilterPredicateBuilder.False<TItem>();

        foreach (var item in Conditions.Select(a => a.ToExpression(caseSensitivity)))
        {
            ret = DataFilterHelper.SetOperator(Operator, ret, item);
        }

        foreach (var item in Groups.Select(a => a.ToExpression(caseSensitivity)))
        {
            ret = DataFilterHelper.SetOperator(Operator, ret, item);
        }

        if (Operator == DataFilterLogicalOperator.NotAnd || Operator == DataFilterLogicalOperator.NotOr)
        {
            ret = DataFilterPredicateBuilder.Not(ret);
        }
        return ret;
    }

    /// <summary>
    /// Clone object.
    /// </summary>
    /// <returns></returns>
    public DataFilterCriteria<TItem> Clone()
        => new()
        {
            Operator = Operator,
            Conditions = Conditions.Select(a => a.Clone()).ToList(),
            Groups = Groups.Select(a => a.Clone()).ToList(),
        };

    /// <summary>
    /// Is empty.
    /// </summary>
    public bool IsEmpty => !Conditions.Any() && !Groups.Any();
}
