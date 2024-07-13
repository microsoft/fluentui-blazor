using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components.Components.DataFilter.Infrastructure;

public partial class FilterNumericEditor<TItem, TProp>
{
    [Parameter, EditorRequired]
    public FilterBase<TItem> Filter { get; set; } = default!;

    [Parameter, EditorRequired]
    public DataFilterCriteriaCondition<TItem> Condition { get; set; } = default!;

    [Parameter, EditorRequired]
    public bool ReadOnly { get; set; }

    private TProp Value => (TProp)Convert.ChangeType(Condition.Value, typeof(TProp))!;

    private async Task ValueChangedAsync(TProp value) => await Filter.SetValueAsync(Condition, value);
}
