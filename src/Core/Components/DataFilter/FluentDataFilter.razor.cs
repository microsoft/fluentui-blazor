using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

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
    /// Filter definition.
    /// </summary>
    [Parameter]
    public DataFilterDescriptor<TItem> Filter { get; set; } = new();

    /// <summary>
    /// Gets or sets a callback that filter changed.
    /// </summary>
    [Parameter]
    public EventCallback Changed { get; set; }

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
    /// Gets or sets the delay, in milliseconds, before to raise the <see cref="Changed"/> event.
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

    private string GetGroupStyle(int index) => GroupStyle?.Invoke(index) + "";

    private string GetConditionStyle(int groupIndex, int conditionIndex) => ConditionStyle?.Invoke(groupIndex, conditionIndex) + "";

    internal List<FilterBase<TItem>> Properties { get; set; } = [];

    private IEnumerable<FilterBase<TItem>> GetAvailableProperties(DataFilterDescriptorCondition<TItem> item)
        => Properties.Where(a => a == item.Filter || !a.Unique || (a.Unique && !Filter.Exists(a)));

    //private JsonSerializerOptions CreateJsonOptions()
    //{
    //    var options = new JsonSerializerOptions
    //    {
    //        WriteIndented = true,
    //    };

    //    foreach (var item in Properties.Where(a => a.JsonConverter != null).Select(a => a.JsonConverter))
    //    {
    //        options.Converters.Add(item);
    //    }
    //    return options;
    //}

    ///// <summary>
    ///// Populate filter from JSON.
    ///// </summary>
    ///// <param name="json"></param>
    //public void FromJson(string json) => Filter = JsonSerializer.Deserialize<DataFilterDescriptor<TItem>>(json, CreateJsonOptions())!;

    private async Task AddAsync(DataFilterDescriptor<TItem> group, string id)
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

    private async Task DeleteFilterAsync(DataFilterDescriptor<TItem> group, DataFilterDescriptorCondition<TItem> item)
    {
        group.Conditions.Remove(item);
        await FilterChangedAsync();
    }

    private async Task DeleteGroupAsync(DataFilterDescriptor<TItem> group, DataFilterDescriptor<TItem> parent)
    {
        parent.Groups.Remove(group);
        await FilterChangedAsync();
    }

    private string DisplayText(DataFilterLogicalOperator value)
        => LogicalOperatorDisplayText == null
                ? value.GetDisplayName()!
                : LogicalOperatorDisplayText.Invoke(value);

    internal async Task FilterChangedAsync()
    {
        StateHasChanged();

        if (Changed.HasDelegate)
        {
            await Changed.InvokeAsync();
        }
    }
}
