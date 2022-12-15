using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.Fast.Components.FluentUI;

partial class DataGridTextBoxColumn<TGridItem> where TGridItem : class
{

    private TGridItem? Item;
    private FluentTextField? element;

    [Parameter]
    public TextFieldType TextFieldType { get; set; } = TextFieldType.Text;

    public DataGridTextBoxColumn()
    {
        IsEditable = true;
    }

    //private string? _internalValue;
    private string? internalValue { get; set; }
    //{
    //    get { return _internalValue; }
    //    set
    //    {
    //        if (value != _internalValue)
    //        {
    //            _internalValue = value;
    //            UpdateSource();
    //        }
    //    }
    //}

    public override object? GetCurrentValue()
    {
        return internalValue;
    }

    public override void SetFocuse()
    {
        element?.FocusAsync();
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
        internalValue = _cellTextFunc!(item);
    }

}
