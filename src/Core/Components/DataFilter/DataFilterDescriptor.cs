using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Descriptor filter.
/// </summary>
/// <typeparam name="TItem"></typeparam>
public class DataFilterDescriptor<TItem>
{
    /// <summary>
    /// Logical Operator.
    /// </summary>
    public DataFilterLogicalOperator Operator { get; set; } = DataFilterLogicalOperator.And;

    /// <summary>
    /// Filters
    /// </summary>
    [JsonIgnore]
    public IList<DataFilterDescriptorCondition<TItem>> Conditions { get; set; } = [];

    [JsonInclude]
    [JsonPropertyName("Conditions")]
    internal IEnumerable<DataFilterDescriptorCondition<TItem>> ConditionsForJson => Conditions.Where(a => a.Field != null);

    /// <summary>
    /// Groups
    /// </summary>
    public IList<DataFilterDescriptor<TItem>> Groups { get; set; } = [];

    internal bool Exists(FilterBase<TItem> filter)
        => Conditions.Any(a => a.Filter == filter) || Groups.Any(a => a.Exists(filter));

    /// <summary>
    /// Get expression for filter data.
    /// </summary>
    /// <param name="caseSensitivity"></param>
    /// <returns></returns>
    public Expression<Func<TItem, bool>> GetFilter(DataFilterCaseSensitivity caseSensitivity)
    {
        var ret = PredicateBuilder.True<TItem>();

        foreach (var item in Conditions.Select(a => a.GetFilter(caseSensitivity)))
        {
            ret = Operator switch
            {
                DataFilterLogicalOperator.And or DataFilterLogicalOperator.NotAnd => PredicateBuilder.And(ret, item),
                DataFilterLogicalOperator.Or or DataFilterLogicalOperator.NotOr => PredicateBuilder.Or(ret, item),
                _ => ret,
            };
        }

        foreach (var item in Groups.Select(a => a.GetFilter(caseSensitivity)))
        {
            ret = Operator switch
            {
                DataFilterLogicalOperator.And or DataFilterLogicalOperator.NotAnd => PredicateBuilder.And(ret, item),
                DataFilterLogicalOperator.Or or DataFilterLogicalOperator.NotOr => PredicateBuilder.Or(ret, item),
                _ => ret,
            };
        }

        if (Operator == DataFilterLogicalOperator.NotAnd || Operator == DataFilterLogicalOperator.NotOr)
        {
            ret = PredicateBuilder.Not(ret);
        }
        return ret;
    }

    /// <summary>
    /// Generate JSON.
    /// </summary>
    /// <returns></returns>
    public string ToJson() => JsonSerializer.Serialize(this, CreateJsonOptions());

    private IEnumerable<JsonConverter<object>> GetConverters()
    {
        var filters = Conditions.Where(a => a.Filter?.JsonConverter != null)
                                .Select(a => a.Filter.JsonConverter!)
                                .ToList();
        filters.AddRange(Groups.Select(a => GetConverters()).SelectMany(a => a));
        return filters.Distinct();
    }
    
    private JsonSerializerOptions CreateJsonOptions()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
        };

        options.Converters.Add(new JsonStringEnumConverter());
        foreach (var item in GetConverters())
        {
            options.Converters.Add(item);
        }

        return options;
    }
}
