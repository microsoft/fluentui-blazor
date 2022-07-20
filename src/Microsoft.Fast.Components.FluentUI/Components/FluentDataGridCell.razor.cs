using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentDataGridCell : FluentComponentBase
{
    internal string CellId { get; } = Identifier.NewId();

    /// <summary>
    /// Gets or sets the cell type. See <see cref="DataGridCellType"/>
    /// </summary>
    [Parameter]
    public DataGridCellType? CellType { get; set; } = DataGridCellType.Default;

    /// <summary>
    /// Gets or sets the column where the cell should be displayed in
    /// </summary>
    [Parameter]
    public int GridColumn { get; set; }

    ///// <summary>
    ///// Gets or sets the owning <see cref="FluentDataGridRow{TItem}"/> component
    ///// </summary>
    //[CascadingParameter(Name = "OwningRow")]
    //public FluentDataGridRow<TItem> Owner { get; set; } = default!;

    //protected override void OnInitialized()
    //{
    //    Owner.Register(this);
    //}

    //public void Dispose()
    //{
    //    Owner.Unregister(this);
    //}

}
