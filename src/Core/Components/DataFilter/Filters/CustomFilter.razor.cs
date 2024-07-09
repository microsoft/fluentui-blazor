using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class CustomFilter<TItem, TProp>
{
    /// <summary>
    /// Get or set Template filter custom.
    /// </summary>
    [Parameter, EditorRequired]
    //public RenderFragment<DataFilterDescriptorCondition<TItem>> TemplateFilter { get; set; } = default!;
    public RenderFragment<DataFilterDescriptorCondition<TItem>> TemplateFilter { get; set; } = default!;

    /// <summary>
    /// Gets or sets the function used for custom filter expression.
    /// </summary>
    [Parameter, EditorRequired]
    public Func<object?, DataFilterCaseSensitivity, Expression<Func<TItem, bool>>> ExpressionFilter { get; set; } = default!;

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

    protected override void OnParametersSet()
    {
        Type = typeof(TProp);
        base.OnParametersSet();
    }
}
