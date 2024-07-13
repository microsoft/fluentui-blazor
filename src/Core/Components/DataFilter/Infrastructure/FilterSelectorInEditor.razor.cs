using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace Microsoft.FluentUI.AspNetCore.Components.Components.DataFilter.Infrastructure;

public partial class FilterSelectorInEditor<TItem, TProp>
{
    [Parameter, EditorRequired]
    public FilterBase<TItem> Filter { get; set; } = default!;

    [Parameter, EditorRequired]
    public DataFilterCriteriaCondition<TItem> Condition { get; set; } = default!;

    [Parameter, EditorRequired]
    public bool ReadOnly { get; set; }

    private ICollection<TProp> Selected => (Condition.Value as ICollection<TProp>)!;

    private async Task SelectedOptionsChangedInAsync(IEnumerable<TProp> items) => await Filter.SetValueAsync(Condition, items);

    private void OnSearchIn(OptionsSearchEventArgs<TProp> e)
    {
        Expression<Func<TProp, bool>> func = a => a != null;

        //todo filter content string ????
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
