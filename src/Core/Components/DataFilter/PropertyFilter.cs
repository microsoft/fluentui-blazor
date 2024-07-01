using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.FluentUI.AspNetCore.Components;

public class PropertyFilter<TItem, TProp> : FluentComponentBase, IPropertyFilter<TItem>
{
    private Expression<Func<TItem, TProp>>? _lastAssignedProperty;

    [CascadingParameter]
    internal FluentDataFilter<TItem> DataFilter { get; set; } = default!;

    public PropertyInfo PropertyInfo { get; protected set; } = default!;

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>The title.</value>
    [Parameter]
    public string Title { get; set; } = default!;

    /// <summary>
    /// If true, generates a title and aria-label attribute for the contents
    /// </summary>
    [Parameter]
    public bool Tooltip { get; set; }

    /// <summary>
    /// Gets or sets the value to be used as the tooltip and aria-label in this 
    /// </summary>
    [Parameter]
    public Func<string>? TooltipText { get; set; }

    ///// <summary>
    ///// Gets or sets the filter template.
    ///// </summary>
    //[Parameter]
    //public RenderFragment<DataFilterProperty<TItem>> Template { get; set; } = default!;

    /// <summary>
    /// Get or set property to filter.
    /// </summary>
    [Parameter, EditorRequired] public Expression<Func<TItem, TProp>> Property { get; set; } = default!;

    /// <summary>
    /// Gets or sets the function used to determine which text to display for each option.
    /// </summary>
    [Parameter]
    public Func<object?, string>? ValueDisplayText { get; set; }

    public LambdaExpression LambdaExpression => Property;

    protected override void OnParametersSet()
    {
        if (!DataFilter.Properties.Contains(this))
        {
            DataFilter.Properties.Add(this);
        }

        if (_lastAssignedProperty != Property)
        {
            _lastAssignedProperty = Property;
        }

        if (Property.Body is MemberExpression memberExpression)
        {
            PropertyInfo = (memberExpression.Member as PropertyInfo)!;
            if (Title is null)
            {
                var daText = memberExpression.Member.DeclaringType?.GetDisplayAttributeString(memberExpression.Member.Name);
                Title = !string.IsNullOrEmpty(daText)
                        ? daText
                        : memberExpression.Member.Name;
            }
        }
        else
        {
            throw new Exception("The property must be a MemberExpression");
        }
    }
}
