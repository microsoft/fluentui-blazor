using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using System.Text.Json;
using System.Text.Json.Serialization;

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
    /// Change the content of this input field when the user write text (based on 'OnInput' HTML event).
    /// </summary>
    [Parameter]
    public bool Immediate { get; set; } = false;

    /// <summary>
    /// Gets or sets the delay, in milliseconds, before to raise the <see cref="CriteriaChanged"/> event.
    /// </summary>
    [Parameter]
    public int ImmediateDelay { get; set; } = 0;

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

    /// <summary>
    /// Clear criteria.
    /// </summary>
    public void ClearCriteria() => Criteria.Clear();

    protected string? ClassValue => new CssBuilder(base.Class)
        .AddClass("fluent-data-filter")
        .Build();

    /// <summary>
    /// From criteria.
    /// </summary>
    /// <param name="criteria"></param>
    public async Task FromCriteriaAsync(DataFilterCriteria<TItem> criteria)
    {
        PopulateFrom(criteria, Criteria, null!);
        await FilterChangedAsync();
    }

    /// <summary>
    /// Populate filter from JSON.
    /// </summary>
    /// <param name="json"></param>
    public async Task FromJsonAsync(string json)
    {
        var options = new JsonSerializerOptions
        {
            Converters =
            {
                new JsonStringEnumConverter()
            },
        };
        var data = JsonSerializer.Deserialize<DataFilterCriteria<TItem>>(json, options)!;
        PopulateFrom(data, Criteria, options);
        await FilterChangedAsync();
    }

    private void PopulateFrom(DataFilterCriteria<TItem> source,
                              DataFilterCriteria<TItem> destination,
                              JsonSerializerOptions options)
    {
        destination.Operator = source.Operator;
        destination.Conditions.Clear();
        foreach (var item in source.Conditions)
        {
            var filter = Filters.Where(a => a.Id == item.Field).FirstOrDefault();
            if (filter != null)
            {
                var condition = new DataFilterCriteriaCondition<TItem>()
                {
                    Filter = filter,
                    Operator = item.Operator,
                };

                condition.Value = item.Value is JsonElement
                                    ? ((JsonElement)item.Value!).Deserialize(condition.Value!.GetType(), options)
                                    : item.Value;

                destination.Conditions.Add(condition);
            }
        }

        destination.Groups.Clear();
        foreach (var item in source.Groups)
        {
            var group = new DataFilterCriteria<TItem>();
            destination.Groups.Add(group);
            PopulateFrom(item, group, options);
        }
    }

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

    private string DisplayText(DataFilterLogicalOperator value)
        => LogicalOperatorDisplayText == null
                ? value.GetDisplayName()!
                : LogicalOperatorDisplayText.Invoke(value);

    private async Task SetFilterAsync(FilterBase<TItem> filter, DataFilterCriteriaCondition<TItem> condition, DataFilterCriteria<TItem> group)
    {
        if (filter != null)
        {
            condition.Filter = filter;
            await FilterChangedAsync();
        }
        else
        {
            condition.Filter = condition.Filter;
            await FilterChangedAsync();
            var aa = 1;
        }
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
