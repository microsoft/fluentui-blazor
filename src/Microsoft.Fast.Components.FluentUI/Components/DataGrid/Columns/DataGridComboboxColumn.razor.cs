using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.Fast.Components.FluentUI;

partial class DataGridComboboxColumn<TGridItem,TOption,TValue> where TGridItem : class
{

    private TGridItem? Item;
    private FluentCombobox<TOption>? element;
    private TOption? _selectedItem;

    [EditorRequired, Parameter]
    public IEnumerable<TOption> Items { get; set; } = default!;

    [Parameter]
    public Func<TOption, string> DisplayMember { get; set; } = default!;

    [Parameter]
    public Func<TOption, TValue> ValueMember { get; set; } = default!;

    //private FluentTextField? element;

    public DataGridComboboxColumn()
    {
        IsEditable = true;
    }

    public override object? GetCurrentValue()
    {
        if (_selectedItem is null)
            return null;
        if (ValueMember is null)
            return _selectedItem;
        else
           return ValueMember.Invoke(_selectedItem);
    }

    public override void SetFocuse()
    {
        element?.Element.FocusAsync();
    }

    public override void UpdateSource()
    {
        if (Item is null) return;
        if (PropertyInfo is not null)
        {
            PropertyInfo.SetValue(Item, GetCurrentValue());
        }
    }

    public override void BeginEdit(TGridItem item)
    {
        Item = item;
        var initialValue = _compiledProperty!.Invoke(Item);
        if (ValueMember is not null || Items is not null)
            _selectedItem = Items.FirstOrDefault(f => EqualityComparer<TValue>.Default.Equals(ValueMember!.Invoke(f), initialValue));
    }

}
