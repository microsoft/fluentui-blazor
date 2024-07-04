using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class CustomFilter<TItem, TProp>
{
    /// <summary>
    /// Get or set Template filter custom.
    /// </summary>
    [Parameter, EditorRequired]
    public RenderFragment<DataFilterDescriptorProperty<TItem>> TemplateFilter { get; set; } = default!;

    /// <summary>
    /// Gets or sets the function used for custom filter expression.
    /// </summary>
    [Parameter, EditorRequired]
    public Func<object?, DataFilterCaseSensitivity, Expression<Func<TItem, bool>>> ExpressionFilter { get; set; } = default!;

    /// <summary>
    /// Get or set serialize value function.
    /// </summary>
    [Parameter]
    public Func<object?, string>? CustomSerializeValue { get; set; } 

    /// <summary>
    /// Generate expression
    /// </summary>
    /// <param name="value"></param>
    /// <param name="operator"></param>
    /// <param name="caseSensitivity"></param>
    /// <returns></returns>
    public override Expression<Func<TItem, bool>> GetFilter(object? value,
                                                            DataFilterComparisonOperator @operator,
                                                            DataFilterCaseSensitivity caseSensitivity)
        => ExpressionFilter(value, caseSensitivity);

    public override IEnumerable<DataFilterComparisonOperator> Operators { get; } = [DataFilterComparisonOperator.Custom];

    /// <summary>
    /// Serialize value.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public override string SerializeValue(object? value)
        => CustomSerializeValue == null
                ? base.SerializeValue(value)
                : CustomSerializeValue(value);

    protected override void OnParametersSet()
    {
        Type = typeof(TProp);
        base.OnParametersSet();
    }
}
