using System.ComponentModel;

namespace Microsoft.Fast.Components.FluentUI;

public class DataGridBeginEditEventArgs<TGridItem> : CancelEventArgs where TGridItem : class
{

    public DataGridBeginEditEventArgs(TGridItem item,string propertyName)
    {
        Item = item;
        PropertyName = propertyName; 
    }

    public TGridItem Item { get; set; }

	public string PropertyName { get; set; }

}
