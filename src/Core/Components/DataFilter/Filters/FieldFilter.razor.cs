using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FieldFilter<TItem>
{
    /// <summary>
    /// Get or set property filter.
    /// </summary>
    [Parameter, EditorRequired] public string Field { get; set; } = default!;

    /// <summary>
    /// Get or set Template filter custom.
    /// </summary>
    [Parameter]
    public RenderFragment<DataFilterCriteriaCondition<TItem>>? TemplateFilter { get; set; }

    /// <summary>
    /// Generate expression
    /// </summary>
    /// <param name="value"></param>
    /// <param name="operator"></param>
    /// <param name="caseSensitivity"></param>
    /// <returns></returns>
    public override Expression<Func<TItem, bool>> ToExpression(object? value,
                                                            DataFilterComparisonOperator @operator,
                                                            DataFilterCaseSensitivity caseSensitivity)
        => DataFilterHelper.GenerateExpression<TItem>(TypeHelper.CreateExpression(typeof(TItem), Field), value, @operator, caseSensitivity);

    protected override void OnParametersSet()
    {
        var prop = GetPropertyInfo(Field);
        Type = prop.PropertyType;
        Title ??= prop.Name;
        Id ??= Field;

        base.OnParametersSet();
    }
}
