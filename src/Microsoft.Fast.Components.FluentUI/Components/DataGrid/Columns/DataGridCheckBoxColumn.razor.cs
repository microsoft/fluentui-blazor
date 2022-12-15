using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.Fast.Components.FluentUI;

partial class DataGridCheckBoxColumn<TGridItem> where TGridItem : class
{

    private TGridItem? Item;
    private Nullable<bool> internalValue;

    public DataGridCheckBoxColumn()
    {
        this.IsEditable = true;
    }

    public override object? GetCurrentValue()
    {
        return internalValue;
    }

    public override void UpdateSource()
    {
        if (Item is null) return;
        if (PropertyInfo is not null)
        {
            PropertyInfo.SetValue(Item, internalValue);
        }
    }

    public override void BeginEdit(TGridItem item)
    {
        Item = item;
        internalValue = _compiledProperty?.Invoke(item);
    }

}
