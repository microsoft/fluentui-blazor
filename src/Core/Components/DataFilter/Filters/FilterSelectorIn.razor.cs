using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FilterSelectorIn<TItem, TProp>
{
    [Parameter, EditorRequired]
    public FilterBase<TItem> Filter { get; set; } = default!;

    [Parameter, EditorRequired]
    public DataFilterCriteriaCondition<TItem> Condition { get; set; } = default!;

    [Parameter, EditorRequired]
    public bool ReadOnly { get; set; }

    private ICollection<TProp> Selected => (Condition.Value as ICollection<TProp>)!;

    private async Task SelectedOptionsChangedInAsync(IEnumerable<TProp> items)
    {
        Selected.Clear();
        foreach (var item in items)
        {
            Selected.Add(item);
        }

        await Filter.DataFilter.FilterChangedAsync();
    }

    private void OnSearchIn(OptionsSearchEventArgs<TProp> e)
    {
        Expression<Func<TProp, bool>> func = a => a != null;
        //&& a.ToString().Contains(e.Text, StringComparison.OrdinalIgnoreCase);
        var where = Filter.ExpressionDef.Make<TItem>(func);
        var selector = (Filter.ExpressionDef as Expression<Func<TItem, TProp>>)!;

        e.Items = Filter.DataFilter.Items!
                                   .Where(where)
                                   .Select(selector)
                                   .Distinct()
                                   .Order();
    }
}
