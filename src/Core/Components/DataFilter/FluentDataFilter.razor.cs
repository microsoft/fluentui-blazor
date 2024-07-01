using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace Microsoft.FluentUI.AspNetCore.Components;

[CascadingTypeParameter(nameof(TItem))]
public partial class FluentDataFilter<TItem>
{
    private readonly string _idAddCondition = Identifier.NewId();
    private readonly string _idAddGroup = Identifier.NewId();

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    //[Parameter]
    //public IQueryable<TItem>? Items { get; set; }

    [Parameter]
    public DataFilterGroup<TItem> Filter { get; set; } = new();

    /// <summary>
    /// Gets or sets a callback that filter changed.
    /// </summary>
    [Parameter]
    public EventCallback Changed { get; set; }

    [Parameter]
    public Func<DataFilterComparisonOperator, string>? ComparisonOperatorDisplayText { get; set; }

    [Parameter]
    public Func<DataFilterLogicalOperator, string>? LogicalOperatorDisplayText { get; set; }

    /// <summary>
    /// Change the content of this input field when the user write text (based on 'OnInput' HTML event).
    /// </summary>
    [Parameter]
    public bool Immediate { get; set; } = false;

    /// <summary>
    /// Gets or sets the delay, in milliseconds, before to raise the <see cref="Changed"/> event.
    /// </summary>
    [Parameter]
    public int ImmediateDelay { get; set; } = 0;

    internal List<IPropertyFilter<TItem>> Properties { get; set; } = [];

    private async Task AddAsync(DataFilterGroup<TItem> group, string id)
    {
        if (id == "Condition")
        {
            group.Filters.Add(new());
        }
        else if (id == "Group")
        {
            group.Groups.Add(new());
        }

        await FilterChangedAsync();
    }

    private async Task DeleteFilterAsync(DataFilterGroup<TItem> group, DataFilterProperty<TItem> item)
    {
        group.Filters.Remove(item);
        await FilterChangedAsync();
    }

    private async Task DeleteGroupAsync(DataFilterGroup<TItem> group, DataFilterGroup<TItem> parent)
    {
        parent.Groups.Remove(group);
        await FilterChangedAsync();
    }

    private string SetTooltip(DataFilterProperty<TItem> item)
        => item.Property?.Tooltip ?? false
                ? item.Property.TooltipText?.Invoke()!
                : "";

    private string SetValueDisplayText(DataFilterProperty<TItem> item, object? obj)
    {
        if (item.Property.ValueDisplayText != null)
        {
            return item.Property.ValueDisplayText.Invoke(obj);
        }
        else if (item.IsEnum)
        {
            return (obj as Enum)?.GetDisplayName() + "";
        }
        else
        {
            return obj + "";
        }
    }

    private string DisplayText(DataFilterLogicalOperator value)
        => LogicalOperatorDisplayText == null
                ? value.ToAttributeValue()!
                : LogicalOperatorDisplayText.Invoke(value);

    private string DisplayText(DataFilterComparisonOperator value)
        => ComparisonOperatorDisplayText == null
                ? value.ToAttributeValue()!
                : ComparisonOperatorDisplayText.Invoke(value);

    private async Task SetPropertyAsync(DataFilterProperty<TItem> item)
    {
        var operators = item.GetAvailableComparisonOperator().ToList();
        if (!operators.Contains(item.Operator))
        {
            item.Operator = operators[0];
        }

        if (item.Value != null && item.Value.GetType() != item.Type)
        {
            //set default value
            if (item.Type.IsValueType)
            {
                item.Value = Activator.CreateInstance(item.Type);
            }
        }

        await FilterChangedAsync();
    }

    private async Task FilterChangedAsync()
    {
        StateHasChanged();

        if (Changed.HasDelegate)
        {
            await Changed.InvokeAsync();
        }
    }

    private async Task SetValueAsync(DataFilterProperty<TItem> item, object value)
    {
        item.Value = value;
        await FilterChangedAsync();
    }
}
