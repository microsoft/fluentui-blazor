using System.ComponentModel;
using Microsoft.Fast.Components.FluentUI.DataGrid.Infrastructure;

namespace Microsoft.Fast.Components.FluentUI;

public class DataGridCellEditEndingEventArgs<TGridItem> : CancelEventArgs where TGridItem : class
{

    public DataGridCellEditEndingEventArgs(TGridItem item,string propertyName,object? newValue,EditActionEnum action) 
    {
        Item = item;
        PropertyName = propertyName; 
        EditAction = action;
        NewValue = newValue;
    }

    public TGridItem Item { get; set; }

	public string PropertyName { get; set; }

    public EditActionEnum EditAction { get;}

    public object? NewValue { get; }

}
