using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.FluentUI.AspNetCore.Components;

public interface IPropertyFilter<TItem>
{
    //RenderFragment<DataFilterProperty<TItem>> Template { get; set; }
    LambdaExpression LambdaExpression { get; }
    PropertyInfo PropertyInfo { get; }
    string Title { get; set; }
    bool Tooltip { get; set; }
    Func<string>? TooltipText { get; set; }
    Func<object?, string>? ValueDisplayText { get; set; }
}
