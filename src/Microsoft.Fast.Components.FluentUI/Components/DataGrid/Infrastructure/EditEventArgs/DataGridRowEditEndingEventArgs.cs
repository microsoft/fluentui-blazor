using System.ComponentModel;
using Microsoft.Fast.Components.FluentUI.DataGrid.Infrastructure;

namespace Microsoft.Fast.Components.FluentUI;

public class DataGridRowEditEndingEventArgs<TGridItem> : CancelEventArgs where TGridItem : class
{

    public DataGridRowEditEndingEventArgs(TGridItem item,EditActionEnum action) 
    {
        Item = item;
        EditAction = action;
    }

    public TGridItem Item { get; set; }

    public EditActionEnum EditAction { get;}

}
