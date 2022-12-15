using System.ComponentModel;
using Microsoft.Fast.Components.FluentUI.DataGrid.Infrastructure;

namespace Microsoft.Fast.Components.FluentUI;

public class DataGridCellEditEndedEventArgs<TGridItem> : EventArgs where TGridItem : class
{

    public DataGridCellEditEndedEventArgs(TGridItem item,string propertyName,EditActionEnum action) 
    {
        Item = item;
        PropertyName = propertyName; 
        EditAction = action;
    }

    public TGridItem Item { get; set; }

	public string? PropertyName { get; set; }

    public EditActionEnum EditAction { get;}

}
