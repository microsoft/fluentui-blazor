using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Components.DataFilter.Infrastructure;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

[CascadingTypeParameter(nameof(TItem))]
public partial class FluentDataFilter<TItem>
{
    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets logical operator.
    /// </summary>
    [Parameter]
    public IEnumerable<DataFilterLogicalOperator> LogicalOperators { get; set; } = [DataFilterLogicalOperator.And,
                                                                                    DataFilterLogicalOperator.NotAnd,
                                                                                    DataFilterLogicalOperator.Or,
                                                                                    DataFilterLogicalOperator.NotOr];

    /// <summary>
    /// Gets or sets a queryable source of data for populate value es. In/NotIn conditions.
    /// </summary>
    [Parameter]
    public IQueryable<TItem>? Items { get; set; }

    /// <summary>
    /// Gets or sets text menu add.
    /// </summary>
    [Parameter]
    public DataFilterCriteria<TItem> Criteria { get; set; } = new();

    /// <summary>
    /// Gets or sets a callback that filter changed.
    /// </summary>
    [Parameter]
    public EventCallback<DataFilterCriteria<TItem>> CriteriaChanged { get; set; }

    /// <summary>
    /// Gets or sets text menu add.
    /// </summary>
    [Parameter]
    public string TextMenuAdd { get; set; } = "Add";

    /// <summary>
    /// Gets or sets text menu add group.
    /// </summary>
    [Parameter]
    public string TextMenuAddGroup { get; set; } = "Group";

    /// <summary>
    /// Gets or sets text menu add condition.
    /// </summary>
    [Parameter]
    public string TextMenuAddCondition { get; set; } = "Condition";

    /// <summary>
    /// Gets or sets allow use logical operator.
    /// </summary>
    [Parameter]
    public bool AllowLogicalOperator { get; set; } = true;

    /// <summary>
    /// Gets or sets allow add group.
    /// </summary>
    [Parameter]
    public bool AllowAddGroup { get; set; } = true;

    /// <summary>
    /// Gets or sets allow add condition.
    /// </summary>
    [Parameter]
    public bool AllowAddCondition { get; set; } = true;

    /// <summary>
    /// Gets or sets comparison operator display text.
    /// </summary>
    [Parameter]
    public Func<DataFilterComparisonOperator, string>? ComparisonOperatorDisplayText { get; set; }

    /// <summary>
    /// Gets or sets logical operator display text.
    /// </summary>
    [Parameter]
    public Func<DataFilterLogicalOperator, string>? LogicalOperatorDisplayText { get; set; }

    /// <summary>
    /// Gets or sets the position of menu add.
    /// </summary>
    [Parameter]
    public DataFilterMenuAddPosition MenuAddPosition { get; set; } = DataFilterMenuAddPosition.Top;

    /// <summary>
    /// Gets or sets group style.
    /// </summary>
    [Parameter]
    public Func<int, string>? GroupStyle { get; set; }

    /// <summary>
    /// Gets or sets group style.
    /// </summary>
    [Parameter]
    public Func<int, int, string>? ConditionStyle { get; set; }

    /// <summary>
    /// When true, the control will be immutable by user interaction. <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Attributes/readonly">readonly</see> HTML attribute for more information.
    /// </summary>
    [Parameter]
    public bool ReadOnly { get; set; }

    private string GetGroupStyle(int index) => GroupStyle?.Invoke(index) + "";

    private string GetConditionStyle(int groupIndex, int conditionIndex) => ConditionStyle?.Invoke(groupIndex, conditionIndex) + "";

    internal List<FilterBase<TItem>> Filters { get; } = [];

    private IEnumerable<FilterBase<TItem>> GetAvailableFilters(DataFilterCriteriaCondition<TItem> item)
        => Filters.Where(a => a.Id == item.Field || !a.Unique || (a.Unique && !Criteria.IsUsed(a.Id!)));

    protected string? ClassValue => new CssBuilder(base.Class)
        .AddClass("fluent-data-filter")
        .Build();

    private async Task AddAsync(DataFilterCriteria<TItem> group, string id)
    {
        if (id == "Condition")
        {
            group.Conditions.Add(new());
        }
        else if (id == "Group")
        {
            group.Groups.Add(new());
        }

        await FilterChangedAsync();
    }

    private async Task DeleteFilterAsync(DataFilterCriteria<TItem> group, DataFilterCriteriaCondition<TItem> item)
    {
        group.Conditions.Remove(item);
        await FilterChangedAsync();
    }

    private async Task DeleteGroupAsync(DataFilterCriteria<TItem> group, DataFilterCriteria<TItem> parent)
    {
        parent.Groups.Remove(group);
        await FilterChangedAsync();
    }

    private string DisplayText(DataFilterLogicalOperator value) =>
        LogicalOperatorDisplayText == null
                ? value.GetDisplayName()!
                : LogicalOperatorDisplayText.Invoke(value);

    private FilterBase<TItem>? GetFilter(DataFilterCriteriaCondition<TItem> condition) =>
        Filters.FirstOrDefault(a => a.Id == condition.FilterId);

    private async Task SetFilterAsync(DataFilterCriteriaCondition<TItem> condition, FilterBase<TItem> filter)
    {
        condition.FilterId = filter?.Id!;

        var field = filter?.FieldPath!;
        if (condition.Field != field)
        {
            //change type set first operator
            condition.Operator = DataFilterHelper.GetOperators(TypeHelper.GetType<TItem>(field)!, false).FirstOrDefault();

            var type = TypeHelper.GetType<TItem>(field)!;

            if (condition.Value == null
                 || (condition.Value != null && condition.Value.GetType() != type))
            {
                condition.Value = TypeHelper.GetDefaultValue(type, false);
            }
        }

        condition.Field = filter?.FieldPath!;

        await FilterChangedAsync();
    }

    internal async Task FilterChangedAsync()
    {
        await InvokeAsync(StateHasChanged);

        if (CriteriaChanged.HasDelegate)
        {
            await CriteriaChanged.InvokeAsync(Criteria);
        }
    }
}
