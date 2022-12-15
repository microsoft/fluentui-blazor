using System.ComponentModel;
using Microsoft.Fast.Components.FluentUI.DataGrid.Infrastructure;

namespace Microsoft.Fast.Components.FluentUI;

public class DataGridRowEditEndedEventArgs<TGridItem> : EventArgs where TGridItem : class
{

    public DataGridRowEditEndedEventArgs(TGridItem item,EditActionEnum action) 
    {
        Item = item;
        EditAction = action;
    }

    public TGridItem Item { get; set; }

    public EditActionEnum EditAction { get;}

}
