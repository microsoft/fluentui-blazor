using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.FluentUI.AspNetCore.Components;

public interface IPropertyFilter<TItem>
{
    //RenderFragment<DataFilterProperty<TItem>> Template { get; set; }

    /// <summary>
    /// Lambda Expression definition property
    /// </summary>
    LambdaExpression LambdaExpression { get; }

    /// <summary>
    /// Property info definition.
    /// </summary>
    PropertyInfo PropertyInfo { get; }

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>The title.</value>
    string Title { get; set; }

    /// <summary>
    /// If true, generates a title and aria-label attribute for the contents
    /// </summary>
    bool Tooltip { get; set; }

    /// <summary>
    /// Gets or sets the value to be used as the tooltip and aria-label in this 
    /// </summary>
    Func<string>? TooltipText { get; set; }

    /// <summary>
    /// Gets or sets the function used to determine which text to display.
    /// </summary>
    Func<object?, string>? ValueDisplayText { get; set; }
}
