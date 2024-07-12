using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FieldFilter<TItem>
{
    /// <summary>
    /// Get or set filed filter.
    /// </summary>
    [Parameter, EditorRequired]
    public string Field { get; set; } = default!;

    /// <summary>
    /// Get or set template value custom.
    /// </summary>
    [Parameter]
    public RenderFragment<DataFilterCriteriaCondition<TItem>>? ValueTemplate { get; set; }

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
        => DataFilterHelper.GenerateExpression<TItem>(ExpressionDef, value, @operator, caseSensitivity);

    protected internal override LambdaExpression ExpressionDef => TypeHelper.CreateExpression(typeof(TItem), Field);

    protected override void OnParametersSet()
    {
        var prop = TypeHelper.GetPropertyInfo<TItem>(Field);
        Type = prop.PropertyType;
        Title ??= prop.Name;
        FieldPath = Field;
        Id ??= Field;

        base.OnParametersSet();
    }
}
