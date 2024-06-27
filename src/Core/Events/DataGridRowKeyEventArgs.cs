using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.FluentUI.AspNetCore.Components;

public class DataGridRowKeyEventArgs<TGridItem> : KeyboardEventArgs
{
    public FluentDataGridRow<TGridItem> Row { get; set; } = default!;
}
