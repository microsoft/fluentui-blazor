using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

[CascadingTypeParameter(nameof(TItem))]
public partial class FluentDataFilterProperty<TItem>
{
    /// <summary>
    /// Gets or sets the orientation of the stacked components. 
    /// </summary>
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Horizontal;


    public DataFilterDescriptorCondition<TItem> Condition { get; set; } = default!;


    ///// <summary>
    ///// Gets or sets the content to be rendered inside the component.
    ///// </summary>
    //[Parameter]
    //public RenderFragment? ChildContent { get; set; }

    ///// <summary>
    ///// Gets or sets allow use logical operator.
    ///// </summary>
    //[Parameter]
    //public bool AllowLogicalOperator { get; set; } = true;

    ///// <summary>
    ///// Gets or sets allow use not logical operator.
    ///// </summary>
    //[Parameter]
    //public bool AllowNotLogicalOperator { get; set; } = true;

    ///// <summary>
    ///// Gets or sets allow add group.
    ///// </summary>
    //[Parameter]
    //public bool AllowAddGroup { get; set; } = true;

    ///// <summary>
    ///// Gets or sets allow add condition.
    ///// </summary>
    //[Parameter]
    //public bool AllowAddCondition { get; set; } = true;

    ///// <summary>
    ///// Filter definition.
    ///// </summary>
    //[Parameter]
    //public DataFilterDescriptor<TItem> Filter { get; set; } = new();

    ///// <summary>
    ///// Gets or sets a callback that filter changed.
    ///// </summary>
    //[Parameter]
    //public EventCallback Changed { get; set; }

    ///// <summary>
    ///// Comparison operator display text.
    ///// </summary>
    //[Parameter]
    //public Func<DataFilterComparisonOperator, string>? ComparisonOperatorDisplayText { get; set; }

    ///// <summary>
    ///// Logical operator display text.
    ///// </summary>
    //[Parameter]
    //public Func<DataFilterLogicalOperator, string>? LogicalOperatorDisplayText { get; set; }

    ///// <summary>
    ///// Change the content of this input field when the user write text (based on 'OnInput' HTML event).
    ///// </summary>
    //[Parameter]
    //public bool Immediate { get; set; } = false;

    ///// <summary>
    ///// Gets or sets the delay, in milliseconds, before to raise the <see cref="Changed"/> event.
    ///// </summary>
    //[Parameter]
    //public int ImmediateDelay { get; set; } = 0;

    ///// <summary>
    ///// Gets or sets the position of menu add.
    ///// </summary>
    //[Parameter]
    //public DataFilterPosition MenuAddPosition { get; set; } = DataFilterPosition.Top;

    ///// <summary>
    ///// Gets or sets the groups level colors.
    ///// </summary>
    //[Parameter]
    //public IEnumerable<string> GroupsLevelColor { get; set; } = [];

    //private string GetStyleGroupsLevelColor(int level)
    //    => GroupsLevelColor.Any()
    //        ? $"background: {GroupsLevelColor.ToArray()[level % (GroupsLevelColor.Count() - 1)]};"
    //        : string.Empty;

    //internal List<PropertyFilterBase<TItem>> Properties { get; set; } = [];

    //private IEnumerable<PropertyFilterBase<TItem>> GetAvailableProperties(DataFilterDescriptorProperty<TItem> item)
    //    => Properties;// .Where(a => a != item.Property || !a.Unique || (a.Unique && !Filter.Exists(a))).ToList();

    //private async Task AddAsync(DataFilterDescriptor<TItem> group, string id)
    //{
    //    if (id == "Condition")
    //    {
    //        group.Filters.Add(new());
    //    }
    //    else if (id == "Group")
    //    {
    //        group.Groups.Add(new());
    //    }

    //    await FilterChangedAsync();
    //}

    //private async Task DeleteFilterAsync(DataFilterDescriptor<TItem> group, DataFilterDescriptorProperty<TItem> item)
    //{
    //    group.Filters.Remove(item);
    //    await FilterChangedAsync();
    //}

    //private async Task DeleteGroupAsync(DataFilterDescriptor<TItem> group, DataFilterDescriptor<TItem> parent)
    //{
    //    parent.Groups.Remove(group);
    //    await FilterChangedAsync();
    //}

    //private IEnumerable<DataFilterLogicalOperator> LogicalOperators
    //    => AllowNotLogicalOperator
    //            ? [DataFilterLogicalOperator.And, DataFilterLogicalOperator.NotAnd, DataFilterLogicalOperator.Or, DataFilterLogicalOperator.NotOr]
    //            : [DataFilterLogicalOperator.And, DataFilterLogicalOperator.Or];

    //private string DisplayText(DataFilterLogicalOperator value)
    //    => LogicalOperatorDisplayText == null
    //            ? value.GetDisplayName()!
    //            : LogicalOperatorDisplayText.Invoke(value);

    //internal async Task FilterChangedAsync()
    //{
    //    StateHasChanged();

    //    if (Changed.HasDelegate)
    //    {
    //        await Changed.InvokeAsync();
    //    }
    //}
}
