using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class PropertyFilter<TItem, TProp>
{
    /// <summary>
    /// Get or set property filter.
    /// </summary>
    [Parameter, EditorRequired] public Expression<Func<TItem, TProp>> Property { get; set; } = default!;

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
        => DataFilterHelper.GenerateExpression<TItem>(Property, value, @operator, caseSensitivity);

    protected override void OnParametersSet()
    {
        if (Property.Body is MemberExpression memberExpression)
        {
            var propertyInfo = memberExpression.Member as PropertyInfo;
            Type = propertyInfo!.PropertyType;
            Title ??= propertyInfo.Name;
            Id ??= string.Join(".", Property.ToString().Split('.').Skip(1));
        }
        else
        {
            throw new ArgumentException("Property not valid!.");
        }

        base.OnParametersSet();
    }
}
